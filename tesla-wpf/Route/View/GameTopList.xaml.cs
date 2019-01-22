using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using tesla_wpf.Model;
using tesla_wpf.Route.ViewModel;
using Vera.Wpf.Lib.Helper;

namespace tesla_wpf.Route.View {
    /// <summary>
    /// GameTopList.xaml 的交互逻辑
    /// </summary>
    public partial class GameTopList : UserControl, IMenuView {

        /// <summary>
        /// 
        /// </summary>
        public void LazyInitialize() {
            ViewHelper.ExecWithLoadingDialog(initialize);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void initialize() {
            InitializeComponent();
            DataContext = new GameTopListViewModel();
        }
    }
}
