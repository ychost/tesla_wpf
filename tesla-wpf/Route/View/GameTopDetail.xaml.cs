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
using tesla_wpf.Route.ViewModel;

namespace tesla_wpf.Route.View {
    /// <summary>
    /// GameTopDetail.xaml 的交互逻辑
    /// </summary>
    public partial class GameTopDetail : UserControl, IDynamicMenu, IMenuInit {
        private string text;

        public GameTopDetail() {
        }

        public GameTopDetail(string content) : this() {
            this.text = content;
        }

        public void OnInit(object param = null) {
            InitializeComponent();
            this.DataContext = new GameTopDetailViewModel(text);
        }
    }
}
