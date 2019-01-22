using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dragablz;
using MaterialDesignThemes.Wpf;
using tesla_wpf.Extensions;
using tesla_wpf.Model;
using tesla_wpf.Route.View;
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
                if (!(value.Content is System.Windows.Controls.UserControl view)) {
                    return;
                }
                // 通过 Tag 来标记，进行延迟初始化
                if (view.Tag == null || (bool)view.Tag == false) {
                    view.Tag = true;
                    value?.Content.LazyInitialize();
                }
                if (!SetProperty(value)) {
                    return;
                }
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
            }
        }
        /// <summary>
        /// 显示的 Tab
        /// </summary>
        public TabItem SelectedTab { get => GetProperty<TabItem>(); set => SetProperty(value); }

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

        protected override void InitRuntimeData() {
            MenuItems = new ObservableCollection<MenuItem> {
                new MenuItem("主页",new HomeView(),PackIconKind.Home),
                new MenuItem("游戏",PackIconKind.Gamepad) {
                    SubMenus = new ObservableCollection<MenuItem>() {
                        new MenuItem("游戏排行",new GameTopList(),PackIconKind.GamepadVariant)
                    }
                }
            };
            TabItems.Add(MenuItems[0].ToTabItem());
            SelectedMenu = MenuItems[0];
            // 处理用户删除了某个 Tab
            TabItems.CollectionChanged += (s, e) => {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
                    var tab = e.OldItems[0] as TabItem;
                    var (menu, _) = MenuItem.GetMenu(MenuItems, tab.BindMenuId, null);
                    (menu.Content as System.Windows.Controls.UserControl).Tag = false;
                    // 这里必须要等到 UI 更新了之后 SelectedTab 才会刷新
                    App.Current.Dispatcher.BeginInvoke(new Action(() => {
                        (SelectedMenu, _) = MenuItem.GetMenu(MenuItems, SelectedTab.BindMenuId, null);
                    }), System.Windows.Threading.DispatcherPriority.Loaded);
                }
            };
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
            SelectedMenu = MenuItems[1];
            MenuIsChecked = true;
        }
    }


}
