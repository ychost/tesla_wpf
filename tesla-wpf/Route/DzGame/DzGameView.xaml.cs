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
using MaterialDesignThemes.Wpf.Transitions;
using tesla_wpf.Model;

namespace tesla_wpf.Route.DzGame {
    /// <summary>
    /// DzGame.xaml 的交互逻辑
    /// </summary>
    public partial class DzGameView : UserControl, IMenu, IMenuDestroy {
        public DzGameViewModel ViewModel { get; set; }
        public DzGameView() {
            ViewModel = new DzGameViewModel();
            DataContext = ViewModel;
            InitializeComponent();
            Transitioner.MoveFirstCommand.Execute(null, DzTrasitioner);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public void OnDestroy(object param = null) {

        }

        /// <summary>
        /// 输入框文字发生了变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputBox_TextChanged(object sender, TextChangedEventArgs e) {
            var inputBox = sender as TextBox;
            if (inputBox.Text == null) {
                return;
            }
            var word = inputBox.Text;
            if (word.StartsWith(" ")) {
                inputBox.Text = null;
                return;
            }
            if (word.Contains(" ")) {
                var inputWords = word.Split(' ');
                ViewModel.MoveNextWord(inputWords[0]);
                var restText = new StringBuilder();
                for (int i = 1; i < inputWords.Length; i++) {
                    restText.Append(inputWords[i]);
                }
                inputBox.Text = restText.ToString();
            } else {
                ViewModel.HandleCurrentWord(word);
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Refresh_Click(object sender, MouseButtonEventArgs e) {
            Transitioner.MoveNextCommand.Execute(null, DzTrasitioner);
            ViewModel.Refresh();
            await Task.Delay(2000);
            Application.Current.Dispatcher.Invoke(() => {
                Transitioner.MovePreviousCommand.Execute(null, DzTrasitioner);
            });
        }

        /// <summary>
        /// 显示/隐藏时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleTime_Click(object sender, MouseButtonEventArgs e) {
            if (TimeTextBlock.Visibility == Visibility.Hidden) {
                TimeTextBlock.Visibility = Visibility.Visible;
            } else {
                TimeTextBlock.Visibility = Visibility.Hidden;
            }
        }
    }

    /// <summary>
    /// 单词的字体颜色
    /// </summary>
    public class WordStyleConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var element = values[0] as UserControl;
            var state = values[1] as WordState?;
            if (state == null) {
                return null;
            }
            Style style = null;
            if (state == WordState.Default) {
            } else if (state == WordState.Typing) {
                style = element.FindResource("TypingWord") as Style;
            } else if (state == WordState.Corrected) {
                style = element.FindResource("CorrectedWord") as Style;
            } else if (state == WordState.Wrong) {
                style = element.FindResource("WrongWord") as Style;
            }
            return style;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 单词的背景颜色
    /// </summary>
    public class WordBorderStyleConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var element = values[0] as UserControl;
            var state = values[1] as WordState?;
            if (state == null) {
                return null;
            }
            Style style = null;
            if (state == WordState.Typing) {
                style = element.FindResource("TypingWordBorder") as Style;
            } else if (state == WordState.WrongTyping) {
                style = element.FindResource("WrongTypingWordBorder") as Style;
            } else {
                style = element.FindResource("NotTypingWordBorder") as Style;
            }
            return style;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
