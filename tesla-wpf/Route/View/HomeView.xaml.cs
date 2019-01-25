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
using tesla_wpf.Model;

namespace tesla_wpf.Route.View {
    /// <summary>
    /// HomeView.xaml 的交互逻辑
    /// </summary>
    public partial class HomeView : UserControl, IMenu, IMenuInit, IMenuActive, IMenuInActive, IMenuDestroy {
        public void OnActive(object param = null) {
            Console.WriteLine("_Active");
        }

        public void OnDestroy(object param = null) {
            Console.WriteLine("_Destroy");
        }

        public void OnInActive(object param = null) {
            Console.WriteLine("_InActive");
        }

        public void OnInit(object param = null) {
            InitializeComponent();
            Console.WriteLine("_Init");
        }
    }
}
