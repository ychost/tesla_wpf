using System;
using System.Collections.Generic;
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
using tesla_wpf.Model;
using tesla_wpf.Model.Game;
using tesla_wpf.Route.ViewModel;

namespace tesla_wpf.Route.View {
    /// <summary>
    /// GameTopDetail.xaml 的交互逻辑
    /// </summary>
    public partial class GameTopDetail : UserControl, IDynamicMenu, IMenuInit {
        private Game game;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public GameTopDetail(Game game) {
            this.game = game;
        }

        public void OnInit(object param = null) {
            InitializeComponent();
            this.DataContext = new GameTopDetailViewModel(game);
        }


        /// <summary>
        /// 按住 Control 按钮可以打开官网
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowserNav_Click(object sender, MouseButtonEventArgs e) {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) {
                var tb = sender as TextBlock;
                Process.Start(new ProcessStartInfo(new Uri(tb.Text).AbsoluteUri));
            }
            e.Handled = true;
        }
    }
}
