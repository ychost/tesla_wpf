using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;
using MaterialDesignThemes.Wpf;
using tesla_wpf.Helper;
using tesla_wpf.Model.Setting;
using tesla_wpf.Route.ViewModel;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Helper;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Route.View {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            var vm = new MainWindowViewModel();
            InitializeComponent();
            DataContext = vm;
            ShowInTaskbar = true;
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            int oldStyle = Win32Helper.GetWindowLong(hwnd, Win32Helper.GWL_STYLE);
            Win32Helper.SetWindowLong(hwnd, Win32Helper.GWL_STYLE, oldStyle & ~Win32Helper.WS_BORDER & ~Win32Helper.WS_CAPTION & ~Win32Helper.WS_DLGFRAME);
            initTaskbarIcon();
        }

        /// <summary>
        /// 双击窗体边缘，切换最大化和最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaxWindow_DoubleClick(object sender, MouseButtonEventArgs e) {
            if (WindowState == WindowState.Maximized) {
                WindowState = WindowState.Normal;
            } else {
                WindowState = WindowState.Maximized;
            }
        }
        /// <summary>
        /// 最小化按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinWindow(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// 退出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow(object sender, RoutedEventArgs e) {
            exit();
        }
        /// <summary>
        /// 最大化窗体大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaxWindow(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Maximized;
        }
        /// <summary>
        /// 普通化窗体大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NormalWindow(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Normal;
        }
        /// <summary>
        /// 移动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragWindow_LeftButton(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            DragMove();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null) {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitLogin(object sender, RoutedEventArgs e) {
            var loginWindow = new LoginWindow(null);
            Application.Current.MainWindow = loginWindow;
            NotifyHelper.UpdateNotifierWindow();
            // 删除用户记录
            SqliteHelper.Exec(db => {
                //db.Execute($"delete from {nameof(User)} where Name='{App.User.Name}'");
                db.Delete(App.User);
            });
            exit();
            loginWindow.Show();
        }
        /// <summary>
        /// 初始化右下角的托盘提示
        /// </summary>
        private TaskbarIcon taskbar;
        private void initTaskbarIcon() {
            taskbar = new TaskbarIcon();
            taskbar.Icon = new System.Drawing.Icon(Application.GetResourceStream(AssetsHelper.GetAssets("fly.ico")).Stream);
            taskbar.ContextMenu = (ContextMenu)FindResource("SysTrayMenu");
            taskbar.DoubleClickCommand = new MdCommand(maximazeExec);
        }
        /// <summary>
        /// 用户双击了托盘图标，最大化
        /// </summary>
        /// <param name="obj"></param>
        private void maximazeExec(object obj) {
            WindowState = WindowState.Normal;
            Topmost = false;
            Topmost = true;
            Topmost = false;
        }
        /// <summary>
        /// 托盘->设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Setting_Click(object sender, RoutedEventArgs e) {
            taskbar.ShowBalloonTip("提示", "设置功能开发中...", BalloonIcon.Warning);
        }
        /// <summary>
        /// 托盘->退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e) {
            exit();
        }
        /// <summary>
        /// 退出的时候注销托盘
        /// </summary>
        private void exit() {
            taskbar.Dispose();
            Close();
        }
    }
}
