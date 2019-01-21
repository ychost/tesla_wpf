using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        /// 菜单名
        /// </summary>
        public string Name { get => GetProperty<string>(); set => SetProperty(value); }
        /// <summary>
        /// 菜单的 View
        /// </summary>
        public DependencyObject Content { get => GetProperty<DependencyObject>(); set => SetProperty(value); }

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
        public MenuItem(string name) {
            Name = name;
            MenuType = MenuType.Category;
            Icon = PackIconKind.MixerSettings;
        }

        public MenuItem(string name, DependencyObject content) {
            Name = name;
            Content = content;
            MenuType = MenuType.View;
            Icon = PackIconKind.MixerSettings;
        }

        /// <summary>
        /// 子菜单
        /// </summary>
        public ObservableCollection<MenuItem> SubMenus { get => GetProperty<ObservableCollection<MenuItem>>(); set => SetProperty(value); }
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

}
