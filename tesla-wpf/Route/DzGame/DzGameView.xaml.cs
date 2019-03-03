using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace tesla_wpf.Route.DzGame {
    /// <summary>
    /// DzGame.xaml 的交互逻辑
    /// </summary>
    public partial class DzGameView : UserControl {
        public DzGameView() {
            InitializeComponent();
        }
    }

    /// <summary>
    /// 被选中的单词的样式
    /// </summary>
    public class WordStyleConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var element = values[0] as UserControl;
            var word = values[1] as DzGameWord;
            if (word == null) {
                return null;
            }
            Style style = null;
            if (word.WordState == WordState.Default) {
            } else if (word.WordState == WordState.Typing) {
                style = element.FindResource("TypingWord") as Style;
            } else if (word.WordState == WordState.Corrected) {
                style = element.FindResource("CorrectedWord") as Style;
            } else if (word.WordState == WordState.Wrong) {
                style = element.FindResource("WrongWord") as Style;
            }
            return style;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public class WordBorderStyleConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var element = values[0] as UserControl;
            var word = values[1] as DzGameWord;
            if (word == null) {
                return null;
            }
            Style style = null;
            if(word.WordState == WordState.Typing) {
                style = element.FindResource("TypingWordBorder") as Style;
            }else {
                style = element.FindResource("NotTypingWordBorder") as Style;
            }
            return style;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
