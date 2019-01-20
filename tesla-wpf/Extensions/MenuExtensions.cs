using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dragablz;
using tesla_wpf.Model;

namespace tesla_wpf.Extensions {
    /// <summary>
    /// Menu 和 Tab的转换
    /// <date>2019-1-20</date>
    /// </summary>
    public static class MenuExtensions {
        /// <summary>
        /// 转换扩展
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static HeaderedItemViewModel ToTabItem(this MenuItem menu) {
            return new HeaderedItemViewModel() {
                Header = menu.Name,
                Content = menu.Content
            };
        }
    }
}
