using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Markdig.Wpf;
using tesla_wpf.Model.Setting;
using Vera.Wpf.Lib.Helper;
using YEvent;
using static tesla_wpf.Route.View.GameDetailEdit;

namespace tesla_wpf {
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application {
        /// <summary>
        /// 事件中心
        /// </summary>
        public static YEventStore Store = YEventStore.create();

        /// <summary>
        /// 初始化数据库啥的
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            MarkdownViewer.SetRenderHook(new RenderHook());
            SqliteHelper.Init(Path.GetFullPath("./tesla.db"));
            FileCacheHelper.AppCacheDirectory = string.Format("{0}\\{1}\\Cache\\",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Process.GetCurrentProcess().ProcessName);
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
