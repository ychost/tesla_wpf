using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dragablz;
using tesla_wpf.Model;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Extensions {
    /// <summary>
    /// Menu 和 Tab的转换
    /// 由于 IMenu 是个接口很多实现无法写在里面（C#8 可以）
    /// 这里只能通过扩展的方式实现默认接口
    /// <date>2019-1-20</date>
    /// </summary>
    public static class MenuExtensions {
        /// <summary>
        /// 转换扩展
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static TabItem ToTabItem(this MenuItem menu) {
            return new TabItem() {
                Header = menu.Name,
                Content = menu.Content,
                BindMenuId = menu.Id
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        public static void Initialize(this IMenu menu) {
            Initialize(menu, null);
        }

        /// <summary>
        /// 初始化，绑定 Context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="menu"></param>
        /// <param name="context"></param>
        public static void Initialize(this IMenu menu, object param) {
            // 使用自定义的 Init
            if (menu is IMenuInit menuInit) {
                menuInit.OnInit(param);
                // 默认的初始化
            } else {
                //InitializeComponent()
                menu.GetType().GetMethod("InitializeComponent").Invoke(menu, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="param"></param>
        public static void Destroy(this IMenu menu) {
            Destroy(menu, null);
        }
        /// <summary>
        /// 销毁 Context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="menu"></param>
        public static void Destroy(this IMenu menu, object param = null) {
            // 使用自定义的 OnDestroy
            if (menu is IMenuDestroy menuDes) {
                menuDes.OnDestroy(param); ;
                // 执行默认的 Destroy
            } else {
                // 销毁 Context
                var context = menu.GetType().GetProperty("DataContext").GetValue(menu);
                if (context is BaseViewModel bvm) {
                    bvm.OnDestroy();
                }
                // 清除关联
                menu.GetType().GetProperty("DataContext").SetValue(menu, null);
            }
        }

        /// <summary>
        /// 激活当前 Tab 所对应的菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="param"></param>
        public static void Active(this IMenu menu, object param = null) {
            if (menu is IMenuActive ma) {
                ma.OnActive(param);
            }
        }

        /// <summary>
        /// Tab 从激活状态转移到非激活状态
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="param"></param>
        public static void InActive(this IMenu menu, object param = null) {
            if (menu is IMenuInActive ma) {
                ma.OnInActive(param);
            }
        }
    }
}
