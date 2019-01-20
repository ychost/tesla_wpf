using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public object Content { get => GetProperty<object>(); set => SetProperty(value); }

        public MenuItem(string name, object content) {
            Name = name;
            Content = content;
        }
    }

}
