

using System.Collections.Generic;

namespace tesla_wpf.Rest {
    public class RsUser {
        public string NickName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Avatar { get; set; }
    }
    /// <summary>
    /// 服务端传入的菜单模型
    /// <date>2019-1-24</date>
    /// </summary>
    public class RsMenu {
        /**
         * 菜单名
         */
        public string Text { get; set; }
        /**
         * 标识该菜单是否为一个分组
         */
        public bool Group { get; set; }

        /**
         * 菜单连接的前端路由
         */
        public string Link { get; set; }
        /**
         * 菜单图标
         */
        public string Icon { get; set; }
        /**
         * 是否在根目录下面显示快捷入口
         */
        public bool ShortcutRoot { get; set; }
        /**
         * 子菜单
         */
        public List<RsMenu> Children { get; set; }
    }
    public class RsApp {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class RsUserSettings {
        public RsUser User { get; set; }
        public App App { get; set; }
        public List<RsMenu> menus { get; set; }
    }
}
