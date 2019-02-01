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
    public partial class GameTopDetail : UserControl, IMenu {
        public GameTopDetail(string content, bool canEdit = true) {
            InitializeComponent();
            DataContext = new GameTopDetailViewModel();
            //Editor.ContentHtml = "<p><b>Smith Html Editor</b></p><p><a href=\"http://smithhtmleditor.codeplex.com\">http://smithhtmleditor.codeplex.com/</a></p>";
            //Editor.IsEnabled = canEdit;
            Editor.Loaded += (s, e) => {
                Editor.ContentHtml= "<p> hello world </p>";
            };
        }
    }
}
