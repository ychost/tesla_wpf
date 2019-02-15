using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using MaterialDesignThemes.Wpf;
using tesla_wpf.Model;
using tesla_wpf.Route.ViewModel;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Helper;

namespace tesla_wpf.Route.View {
    /// <summary>
    /// GameTopList.xaml 的交互逻辑
    /// </summary>
    public partial class GameTopList : UserControl, IMenuInit, IMenuActive, IMenuInActive, IMenuDestroy, IMenuAssureDestroy {
        public async Task<bool> AssureDestroy() {
            var assure = await DialogHost.Show(new ConfirmDialog("确定退出?")) as bool?;
            return assure == true;
        }

        public void OnActive(object param = null) {
            Console.WriteLine("Active");
        }

        public void OnDestroy(object param = null) {
            this.DataContext = null;
            Console.WriteLine("Destroy");
        }

        public void OnInActive(object param = null) {
            Console.WriteLine("InActive");
        }

        public void OnInit(object param = null) {
            Console.WriteLine("Init");
            InitializeComponent();
            this.DataContext = new GameTopListViewModel();
        }
    }
}
