using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dragablz;
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
        public ObservableCollection<HeaderedItemViewModel> TabItems { get; set; } = new ObservableCollection<HeaderedItemViewModel>();

        /// <summary>
        /// 菜单是否处于打开状态
        /// </summary>
        public bool MenuIsChecked { get => GetProperty<bool>(); set => SetProperty(value); }

        /// <summary>
        /// 菜单列表
        /// </summary>
        public MenuItem[] MenuItems { get => GetProperty<MenuItem[]>(); set => SetProperty(value); }

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
                if (!SetProperty(value)) {
                    return;
                }
                foreach (var item in TabItems) {
                    if (item.Header.ToString() == value.Name) {
                        SelectedTab = item;
                        return;
                    }
                }
                var tabItem = new HeaderedItemViewModel() {
                    Header = value.Name,
                    Content = value.Content
                };
                TabItems.Add(tabItem);
                SelectedTab = tabItem;
            }
        }
        /// <summary>
        /// 显示的 Tab
        /// </summary>
        public HeaderedItemViewModel SelectedTab { get => GetProperty<HeaderedItemViewModel>(); set => SetProperty(value); }

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
            InitDesignData();
        }

        protected override void InitDesignData() {
            MenuItems = new MenuItem[] {
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
            MenuIsChecked = false;
        }
    }


}
