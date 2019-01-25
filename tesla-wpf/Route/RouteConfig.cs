using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tesla_wpf.Model;
using tesla_wpf.Route.View;

namespace tesla_wpf.Route {
    /// <summary>
    /// 菜单 Link 和 View 绑定
    /// <date>2019-1-24</date>
    /// </summary>
    public static class RouteConfig {
        public static readonly IDictionary<string, RouteItem> MenuConfig = new Dictionary<string, RouteItem>() {
            ["/home"] = new RouteItem(typeof(HomeView)),
            ["/game-top/list"] = new RouteItem(typeof(GameTopList))
        };

    }
    public class RouteItem {
        public static int LastRouteOrder = 0;
        /// <summary>
        /// 序号学校越靠前
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// UserControl 的类型
        /// </summary>
        public Type ViewType { get; set; }

        public RouteItem(int order, Type type) {
            Order = order;
            ViewType = type;
            LastRouteOrder = order > LastRouteOrder ? order : LastRouteOrder;
        }

        /// <summary>
        /// 自然顺序
        /// </summary>
        /// <param name="type"></param>
        public RouteItem(Type type) {
            ViewType = type;
            Order = ++LastRouteOrder;
        }
    }

}
