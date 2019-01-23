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
using tesla_wpf.Route.ViewModel;
using Vera.Wpf.Lib.Component;

namespace tesla_wpf.Route.View {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() : this("default token") {

        }
        public MainWindow(string token) {
            var vm = new MainWindowViewModel(token);
            DataContext = vm;
            InitializeComponent();
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

    }
}
