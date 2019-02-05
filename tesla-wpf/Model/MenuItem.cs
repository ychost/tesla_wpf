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
        public int Id { get; set; }
        /// <summary>
        /// 菜单名
        /// </summary>
        public string Name { get => GetProperty<string>(); set => SetProperty(value); }
        /// <summary>
        /// 菜单的 View
        /// </summary>
        public IMenu Content { get => GetProperty<IMenu>(); set => SetProperty(value); }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType MenuType { get => GetProperty<MenuType>(); set => SetProperty(value); }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public PackIconKind Icon { get => GetProperty<PackIconKind>(); set => SetProperty(value); }


        /// <summary>
        /// String 类型的 Icon
        /// </summary>
        /// <param name="name"></param>
        /// <param name="icon"></param>
        public MenuItem(string name, IMenu content, string icon) : this(name, content, parseIcon(icon)) {
        }

        /// <summary>
        /// String 类型的 Icon，父菜单
        /// </summary>
        /// <param name="name"></param>
        /// <param name="icon"></param>
        public MenuItem(string name, string icon) : this(name, null, icon) {

        }

        /// <summary>
        ///  普通 Icon 父菜单
        /// </summary>
        /// <param name="name"></param>
        public MenuItem(string name, PackIconKind icon = PackIconKind.Mixcloud) : this(name, null, icon) {
            MenuType = MenuType.Category;
        }

        /// <summary>
        /// 普通类型 Icon 子菜单
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <param name="icon"></param>
        public MenuItem(string name, IMenu content, PackIconKind icon = PackIconKind.Mixcloud) {
            Name = name;
            Content = content;
            MenuType = MenuType.View;
            Icon = icon;
            //Id = MenuCount++;
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


        private static PackIconKind parseIcon(string icon) {
            if (Enum.TryParse<PackIconKind>(icon, out var packIcon)) {
                return packIcon;
            }
            return PackIconKind.Mixcloud;
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
    public interface IMenu {

    }

    /// <summary>
    /// 初始化生命周期
    /// </summary>
    public interface IMenuInit : IMenu {
        /// <summary>
        /// 绑定 Context
        /// </summary>
        /// <returns></returns>
        void OnInit(object param = null);

    }

    /// <summary>
    /// 动态菜单, 不在菜单列表里面的,动态生成的
    /// 也支持这些扩展的方法
    /// </summary>
    public interface IDynamicMenu : IMenu {
        
    }


    /// <summary>
    /// 菜单被关闭，生命周期结束
    /// </summary>
    public interface IMenuDestroy : IMenu {
        /// <summary>
        /// 销毁 Contextx
        /// </summary>
        void OnDestroy(object param = null);
    }

    /// <summary>
    /// Tab 页面被激活
    /// </summary>
    public interface IMenuActive : IMenu {
        void OnActive(object param = null);
    }

    /// <summary>
    /// Tab 页面从激活到失效
    /// </summary>
    public interface IMenuInActive : IMenu {
        void OnInActive(object param = null);
    }
}
