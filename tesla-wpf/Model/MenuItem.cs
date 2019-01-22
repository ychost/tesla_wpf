using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using tesla_wpf.Extensions;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Model {
    /// <summary>
    /// 菜单项，含菜单命和内容
    /// </summary>
    public class MenuItem : BaseViewModel {
        /// <summary>
        /// 菜单总数量
        /// </summary>
        public static int MenuCount;
        /// <summary>
        /// 每个 Menu 特有的标识
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// 菜单名
        /// </summary>
        public string Name { get => GetProperty<string>(); set => SetProperty(value); }
        /// <summary>
        /// 菜单的 View
        /// </summary>
        public IMenuView Content { get => GetProperty<IMenuView>(); set => SetProperty(value); }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType MenuType { get => GetProperty<MenuType>(); set => SetProperty(value); }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public PackIconKind Icon { get => GetProperty<PackIconKind>(); set => SetProperty(value); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public MenuItem(string name, PackIconKind icon = PackIconKind.Mixcloud) : this(name, null, icon) {
            MenuType = MenuType.Category;
        }

        public MenuItem(string name, IMenuView content, PackIconKind icon = PackIconKind.Mixcloud) {
            Name = name;
            Content = content;
            MenuType = MenuType.View;
            Icon = icon;
            Id = MenuCount++;
        }

        /// <summary>
        /// 子菜单
        /// </summary>
        public ObservableCollection<MenuItem> SubMenus { get => GetProperty<ObservableCollection<MenuItem>>(); set => SetProperty(value); }

        /// <summary>
        /// 递归获取 (Menu,SuperMenu) 通过 Id
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static (MenuItem, MenuItem) GetMenu(IList<MenuItem> menus, int id, MenuItem superMenu) {
            if (menus == null) {
                return (null, null);
            }
            foreach (var menu in menus) {
                if (menu.Id == id) {
                    return (menu, superMenu);
                }
                var (findMenu, findSuperMenu) = GetMenu(menu.SubMenus?.ToArray(), id, menu);
                if (findMenu != null) {
                    return (findMenu, findSuperMenu);
                }
            }
            return (null, null);
        }
    }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuType {
        // 仅仅是个分类，有子菜单的那种
        Category,
        // 菜单有视图
        View,
        // 菜单是个 URL 连接
        // todo 菜单连接， 目前未实现
        Link,
    }

    /// <summary>
    /// 所有菜单的路由视图都得实现这个接口
    /// </summary>
    public interface IMenuView {
        /// <summary>
        /// 延迟初始化，只有该路由被激活了才调用
        /// </summary>
        void LazyInitialize();
    }

    /// <summary>
    /// TreeView 只能打补丁实现 SelectedItem，所以要包装一下
    /// <date>2019-1-22</date>
    /// </summary>
    public class MenuItemTreeWrapper : TreeViewItem, INotifyPropertyChanged {
        public string Test { get; set; } = "Test";
        public MenuItemTreeWrapper(string name, IMenuView content, PackIconKind icon = PackIconKind.Mixcloud) {
            MenuItem = new MenuItem(name, content, icon);
        }
        public MenuItemTreeWrapper(string name, PackIconKind icon = PackIconKind.Mixcloud) {
            MenuItem = new MenuItem(name, icon);
        }

        private MenuItem menuItem;
        public MenuItem MenuItem {
            get => menuItem;
            set {
                if (menuItem != value) {
                    menuItem = value;
                    onPropertyChanged();
                }
            }
        }




        private ObservableCollection<MenuItemTreeWrapper> subMenus;
        public ObservableCollection<MenuItemTreeWrapper> SubMenus {
            get => subMenus;
            set {
                if (subMenus != value) {
                    subMenus = value;
                    onPropertyChanged();
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




        public static (MenuItemTreeWrapper, MenuItemTreeWrapper) GetMenu(IList<MenuItemTreeWrapper> menus, int id, MenuItemTreeWrapper superMenu) {
            if (menus == null) {
                return (null, null);
            }
            foreach (var menu in menus) {
                if (menu.MenuItem.Id == id) {
                    return (menu, superMenu);
                }
                var (findMenu, findSuperMenu) = GetMenu(menu.SubMenus?.ToArray(), id, menu);
                if (findMenu != null) {
                    return (findMenu, findSuperMenu);
                }
            }
            return (null, null);
        }
    }



}
