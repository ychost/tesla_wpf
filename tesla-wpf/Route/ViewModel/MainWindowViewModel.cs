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
using RestSharp;
using tesla_wpf.Extensions;
using tesla_wpf.Model;
using tesla_wpf.Rest;
using tesla_wpf.Route.View;
using tesla_wpf.Toolkit;
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
        /// 菜单是否处于打开状态
        /// </summary>
        public bool MenuIsChecked { get => GetProperty<bool>(); set => SetProperty(value); }

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

        private async void setSelectedMenu(MenuItem value) {
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
            get => GetProperty<TabItem>(); set {
                var oldTab = GetProperty<TabItem>();
                // 上个 Tab 冻结
                // 当前 Tab 激活
                if (SetProperty(value)) {
                    oldTab?.Content?.InActive(null);
                    value?.Content.Active(null);
                    if (value != null) {
                        (SelectedMenu, _) = MenuItem.GetMenu(MenuItems, value.BindMenuId, null);
                    }
                }
            }
        }

        public string Token { get; }
        /// <summary>
        /// 注入 Token
        /// </summary>
        /// <param name="token"></param>
        public MainWindowViewModel(string token) {
            Token = token;
        }

        public MainWindowViewModel() {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void InitRuntimeData() {
            //todo 服务器请求
            MenuItems = convertToMenus(null);
            TabItems.Add(MenuItems[0].ToTabItem());
            // 处理用户删除了某个 Tab
            TabItems.CollectionChanged += disptachTabEvent;
        }


        RsUserSettings fetchUserSettings() {
            var client = new RestClient("http://test.sudoyc.com:1002");
            client.UseSerializer(new RestJsonSerializer());
            var request = new RestRequest(RestApi.FetchUserSettings);
            request.AddHeader("Authorization", TokenToolkit.GetToken());
            var response = client.Get<Rest<RsUserSettings>>(request);

            return response.Data.Data;
        }

        /// <summary>
        /// 从服务器拉取的菜单数据转换成普通的菜单数据
        /// </summary>
        /// <param name="rsMenus"></param>
        /// <returns></returns>
        ObservableCollection<MenuItem> convertToMenus(List<RsMenu> rsMenus) {
            var menuItems = new ObservableCollection<MenuItem>();
            foreach (var rm in rsMenus) {
                // 子菜单
                if (rm.Children == null || rm.Children.Count == 0) {
                    if (RouteConfig.MenuConfig.TryGetValue(rm.Link, out var type)) {
                        var view = (IMenu)Activator.CreateInstance(type);
                        var item = new MenuItem(rm.Text, view, rm.Icon);
                        menuItems.Add(item);
                    } else {
                        throw new Exception($"菜单 $[{rm.Text}] 不存在");
                    }
                    // 父菜单
                } else {
                    menuItems.Add(new MenuItem(rm.Text, rm.Icon) {
                        SubMenus = convertToMenus(rm.Children)
                    });
                }
            }
            return menuItems;
        }


        /// <summary>
        /// 派遣 Tab 的一些 Add,Remove 等事件
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        void disptachTabEvent(object s, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Remove) {
                // 这里必须要等到 UI 更新了之后 SelectedTab 才会刷新
                App.Current.Dispatcher.BeginInvoke(new Action(() => {
                    destroyMenuContent(e.OldItems[0] as TabItem);
                }), System.Windows.Threading.DispatcherPriority.Loaded);
            }
        }
        /// <summary>
        /// 当某个 Tab 被销毁了之后，跟着销毁对应菜单的 Content
        /// </summary>
        void destroyMenuContent(TabItem tab) {
            var (menu, _) = MenuItem.GetMenu(MenuItems, tab.BindMenuId, null);
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
