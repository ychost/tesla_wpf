using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tesla_wpf.Route.View;

namespace tesla_wpf {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window {

        public double Scaling {
            get { return (double)GetValue(ScalingProperty); }
            set { SetValue(ScalingProperty, value); }
        }
        public LoginWindow() {
            InitializeComponent();
        }

        private void Window_Move(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        /// <summary>
        /// 登录，切换到 MainWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e) {
            var window = new MainWindow("default token");
            Application.Current.MainWindow = window;
            Close();
            window.Show();
        }

        public static readonly DependencyProperty ScalingProperty =
            DependencyProperty.Register("Scaling", typeof(double), typeof(LoginWindow), new PropertyMetadata(0.0));

    }
}
