using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
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

        /// <summary>
        /// 测试的 Token
        /// </summary>
        public static readonly string TestToken = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhcHAiOiJ0ZXNsYSIsImV4cGlyZSI6IjIxMTgtMTItMDYgMjM6MzI6NTUiLCJleHAiOjQ2OTk3ODM5NzUsImdlbmVyYXRlVGltZSI6IjIwMTgtMTItMDYgMjM6MzI6NTUiLCJ1c2VyIjoieWNob3N0In0.dAZF0ZbjxrHrH1kTLBuyx9aiyve1f3B3tShurzkkAHY";
    }
}
