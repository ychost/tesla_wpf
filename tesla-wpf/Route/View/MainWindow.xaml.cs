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
using MaterialDesignThemes.Wpf;
using tesla_wpf.Model.Setting;
using tesla_wpf.Route.ViewModel;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Helper;

namespace tesla_wpf.Route.View {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            var vm = new MainWindowViewModel();
            DataContext = vm;
            InitializeComponent();
            ShowInTaskbar = false;
        }


        private void MaxWindow_DoubleClick(object sender, MouseButtonEventArgs e) {
            if (WindowState == WindowState.Maximized) {
                WindowState = WindowState.Normal;
            } else {
                WindowState = WindowState.Maximized;
            }
        }

        private void MinWindow(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void MaxWindow(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Maximized;
        }

        private void NormalWindow(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Normal;
        }

        private void DragWindow_LeftButton(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            DragMove();
        }

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
            Close();
            loginWindow.Show();
        }

        private void MenuSetting_Click(object sender, EventArgs e) {

        }

        private void MenuExit_Click(object sender, EventArgs e) {
            this.NotifyIcon.Close();
            Close();
        }

        private void NotifyIcon_DbClick(object sender, MouseButtonEventArgs e) {
            Topmost = false;
            Topmost = true;
            Topmost = false;
        }
    }
}
