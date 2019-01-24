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
        public static readonly IDictionary<string, Type> MenuConfig = new Dictionary<string, Type>() {
            ["/home"] = typeof(HomeView),
            ["/game-top/list"] = typeof(GameTopList)
        };
    }
}
