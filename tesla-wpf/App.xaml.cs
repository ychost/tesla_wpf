using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Markdig.Wpf;
using NLog;
using tesla_wpf.Helper;
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
        /// 日志工具，可输出到终端、文件、数据库，见 NLog.config
        /// </summary>
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        static App() {
            initLogVar();
        }

        /// <summary>
        /// 初始化数据库啥的
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            ConsoleHelper.Show();
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
                // 更新日志用户数据
                LogManager.Configuration.Variables["userName"] = user.Name;
            }

        }


        /// <summary>
        /// 获取 http 请求的必备 Token
        /// </summary>
        /// <returns></returns>
        public static string GetHttpToken() {
            return User.Token;
        }

        /// <summary>
        /// 配置一些日志里面用到的变量
        /// </summary>
        private static void initLogVar() {
            // 用户名
            LogManager.Configuration.Variables["userName"] = "Unknown";
            // 系统版本
            var osVersion = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                             select x.GetPropertyValue("Caption")).FirstOrDefault() ?? "Unknown";
            LogManager.Configuration.Variables["osVersion"] = osVersion.ToString() + "_" + (Environment.Is64BitOperatingSystem ? "x64" : "x86");
            // .net 版本
            LogManager.Configuration.Variables["netVersion"] = Environment.Version.ToString();
        }

    }
}
