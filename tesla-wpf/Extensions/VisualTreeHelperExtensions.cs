using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace tesla_wpf.Extensions {
    /// <summary>
    /// 扩展 VisualTreeHelper 的一些方法
    /// <date>2019-1-22</date>
    /// </summary>
    public static class VisualTreeHelperExtensions {
        /// <summary>
        /// 获取父类视图控件，可以指定父类控件类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetParent<T>(DependencyObject obj) where T : DependencyObject {
            var parent = VisualTreeHelper.GetParent(obj);
            if (parent is T) {
                return parent as T;
            }
            return GetParent<T>(parent);
        }
    }
}
