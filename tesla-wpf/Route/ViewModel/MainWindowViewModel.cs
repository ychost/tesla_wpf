using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dragablz;
using MaterialDesignThemes.Wpf;
using NLog;
using RestSharp;
using tesla_wpf.Event;
using tesla_wpf.Extensions;
using tesla_wpf.Helper;
using tesla_wpf.Model;
using tesla_wpf.Model.Setting;
using tesla_wpf.Rest;
using tesla_wpf.Route.View;
using tesla_wpf.Toolkit;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Extensions;
using Vera.Wpf.Lib.Helper;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Route.ViewModel {
    /// <summary>
    /// <date>2019-1-20</date>
    /// </summary>
    public class MainWindowViewModel : BaseViewModel {
        /// <summary>
        /// Tab 列表
        /// </summary>
        public ObservableCollection<TabItem> TabItems { get; set; } = new ObservableCollection<TabItem>();
        /// <summary>
        /// 用户信息
        /// </summary>
        public User User { get => GetProperty<User>(); set => SetProperty(value); }
        /// <summary>
        /// 菜单是否处于打开状态
        /// </summary>
        public bool MenuIsChecked { get => GetProperty<bool>(); set => SetProperty(value); }

        public readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 菜单列表
        /// </summary>
        public ObservableCollection<MenuItem> MenuItems { get => GetProperty<ObservableCollection<MenuItem>>(); set => SetProperty(value); }

        public ICommand CloseMenuCmd => new MdCommand(closeMenuExec);

        /// <summary>
        /// 关闭菜单
        /// </summary>
        /// <param name="obj"></param>
        private void closeMenuExec(object obj) {
            MenuIsChecked = false;
        }


        /// <summary>
        /// 用户切换了菜单，那么也要切换 Tab 选项
        /// 通过菜单名字来关联 tab
        /// </summary>
        public MenuItem SelectedMenu {
            get => GetProperty<MenuItem>(); set {
                setSelectedMenu(value);
            }
        }

        private async Task setSelectedMenu(MenuItem value) {
            if (value == null) {
                return;
            }
            if (!(value.Content is System.Windows.Controls.UserControl view)) {
                return;
            }
            // 通过 Tag 来标记，进行延迟初始化
            if (view.Tag == null || (bool)view.Tag == false) {
                view.Tag = true;
                await ViewHelper.ExecWithLoadingDialog(value.Content.Initialize);
                //Content.Initialize();
            }
            if (!SetProperty(value, nameof(SelectedMenu))) {
                return;
            }
            App.Current.Dispatcher.Invoke(() => {
                // 关闭菜单
                MenuIsChecked = false;
                foreach (var item in TabItems) {
                    if (item.Header.ToString() == value.Name) {
                        SelectedTab = item;
                        return;
                    }
                }
                var tabItem = value.ToTabItem();
                TabItems.Add(tabItem);
                SelectedTab = tabItem;
            });
        }
        /// <summary>
        /// 显示的 Tab
        /// </summary>
        public TabItem SelectedTab {
            get => GetProperty<TabItem>(); set => setTab(value);
        }

        public ItemActionCallback ClosingTabCallback => new ItemActionCallback(async arg => {
            // 由于 cancel 只支持同步
            // 所以这里通过手工从集合移除的方式
            // 取消库里面的移除方法
            arg.Cancel();
            var tab = arg.DragablzItem.Content as TabItem;
            if (tab == null) {
                return;
            }
            if (tab.Content is IMenuAssureDestroy assure) {
                if (await assure.AssureDestroy() == false) {
                    return;
                }
            }
            Application.Current.Dispatcher.Invoke(() => {
                TabItems.Remove(tab);
            });

        });

        /// <summary>
        /// 设置 Tab
        /// </summary>
        /// <param name="value"></param>
        private async void setTab(TabItem value) {
            var oldTab = GetProperty<TabItem>();
            // 上个 Tab 冻结
            // 当前 Tab 激活
            if (SetProperty(value, nameof(SelectedTab))) {
                oldTab?.Content?.InActive(null);
                if (value == null) {
                    return;
                }
                // 如果是动态菜单那么, 这里首先初始化一次
                if (value.Content is IDynamicMenu) {
                    if (!(value.Content is System.Windows.Controls.UserControl view)) {
                        return;
                    }
                    // 通过 Tag 来标记，进行延迟初始化
                    if (view.Tag == null || (bool)view.Tag == false) {
                        view.Tag = true;
                        await ViewHelper.ExecWithLoadingDialog(value.Content.Initialize);
                    }
                }
                value.Content.Active(null);
                var (menu, _) = MenuItem.GetMenu(MenuItems, value.BindMenuId, null);
                if (menu != null) {
                    SelectedMenu = menu;
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void InitRuntimeData() {
            // 获取用户
            User = App.User;
            handleUserSettings();
            App.Store.Subscribe(typeof(AddTabEvent), (s, e) => {
                var tabEvent = e.Payload as AddTabEvent;
                var tab = new TabItem() {
                    Header = tabEvent.TabName,
                    Content = tabEvent.TabContent,
                    // 不和 Menu 绑定
                    // 动态添加的
                    BindMenuId = -1,
                };
                TabItems.Add(tab);
                if (tabEvent.IsSwitchIt) {
                    SelectedTab = tab;
                }
            }, true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        async void handleUserSettings() {
            var rest = await HttpRestService.ForAuthApi<RsSystemApi>().FetchUserSettings();
            try {
                if (HttpRestService.ForData(rest, out var settings)) {
                    var history = new LoginHistory() {
                        Name = User.Name,
                        Avatar = User.Avatar
                    };
                    // 更新用户数据
                    SqliteHelper.Exec(db => {
                        var oldUser = db.Query<User>($"Select * From {nameof(User)} where Name='{User.Name}'").FirstOrDefault();
                        var oldHistory = db.Query<LoginHistory>($"Select * From {nameof(LoginHistory)} where Name='{history.Name}'").FirstOrDefault();
                        //fixme: 用 InserOrReplace 会导致 Token 无法写入
                        //可能是 Sqlite-net 的 Bug
                        if (oldUser != null) {
                            db.Delete(oldUser);
                        }
                        if (oldHistory != null) {
                            db.Delete(oldHistory);
                            history.CreateTime = oldHistory.CreateTime;
                        }
                        db.Insert(User);
                        history.Avatar = AssetsHelper.DownloadAvatar(User.Avatar, User.Name);
                        db.Insert(history);
                    });
                    // 渲染菜单项
                    MenuItems = ConvertToolkit.ConvertMenus(settings.Menus);
                    Application.Current.Dispatcher.Invoke(() => {
                        TabItems.CollectionChanged += disptachTabEvent;
                        setSelectedMenu(MenuItems[0].SubMenus[0]);
                    });
                } else {
                    await Application.Current.Dispatcher.Invoke(async () => {
                        await DialogHost.Show(new ConfirmDialog(rest.Message));
                    });
                    Application.Current.MainWindow.Close();
                }
            } catch (Exception e) {
                NotifyHelper.ShowErrorMessage("系统错误" + e.Message);
                Logger.Error(e, "加载菜单失败");
            }
        }

        /// <summary>
        /// 派遣 Tab 的一些 Add,Remove 等事件
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        async void disptachTabEvent(object s, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Remove) {
                // 这里必须要等到 UI 更新了之后 SelectedTab 才会刷新
                var tab = e.OldItems[0] as TabItem;
                await Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    var (menu, _) = MenuItem.GetMenu(MenuItems, tab.BindMenuId, null);
                    if (menu == null) {
                        return;
                    }
                    destroyMenuContent(menu);
                }), System.Windows.Threading.DispatcherPriority.Loaded);
            }
        }
        /// <summary>
        /// 当某个 Tab 被销毁了之后，跟着销毁对应菜单的 Content
        /// </summary>
        void destroyMenuContent(MenuItem menu) {
            // 复位标记
            (menu.Content as System.Windows.Controls.UserControl).Tag = false;
            // 销毁视图
            menu.Content.Destroy();
            //回收垃圾
            GC.Collect();
        }

        protected override void InitDesignData() {
            MenuItems = new ObservableCollection<MenuItem> {
                new MenuItem("tab1"){
                    SubMenus = new ObservableCollection<MenuItem>() {
                        new MenuItem("tab1_sub"){
                            SubMenus = new ObservableCollection<MenuItem>() {
                                new MenuItem("tab1_sub_sub",new HomeView())
                            }
                        },
                    }
                },
                new MenuItem("tab2",new HomeView()),
                new MenuItem("tab3",new HomeView()),
                new MenuItem("tab4",new HomeView()),
            };
            TabItems.Add(MenuItems[0].ToTabItem());
            SelectedMenu = MenuItems[0];
            MenuIsChecked = true;
        }
    }


}
