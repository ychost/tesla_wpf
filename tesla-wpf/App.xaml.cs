using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using tesla_wpf.Model.Setting;
using Vera.Wpf.Lib.Helper;

namespace tesla_wpf {
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            SqliteHelper.Init(Path.GetFullPath("./tesla.db"));
        }

        /// </summary>
        /// <summary>
        /// 登录的用户信息
        /// </summary>
        private static User user;
        public static User User {
            get {
                if (user == null) {
                    throw new Exception("用户数据为空");
                }
                return user;
            }
            set {
                user = value;
            }
        }


        /// <summary>
        /// 获取 http 请求的必备 Token
        /// </summary>
        /// <returns></returns>
        public static string GetHttpToken() {
            return User.Token;
        }
    }
}
