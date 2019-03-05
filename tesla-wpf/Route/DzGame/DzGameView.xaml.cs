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
using YEvent;

namespace tesla_wpf.Route.DzGame {
    /// <summary>
    /// DzGame.xaml 的交互逻辑
    /// <date>2019-3-3</date>
    /// </summary>
    public partial class DzGameView : UserControl, IMenu, IMenuDestroy {
        /// <summary>
        /// 视图模型
        /// </summary>
        public DzGameViewModel ViewModel { get; set; }
        /// <summary>
        /// 取消对游戏完成事件的订阅
        /// </summary>
        Unsubscibe UnsubscribeCompleteEvent;

        /// <summary>
        /// 绑定视图模型，订阅游戏完成事件
        /// </summary>
        public DzGameView() {
            ViewModel = new DzGameViewModel();
            DataContext = ViewModel;
            InitializeComponent();
            // 订阅完成游戏事件
            UnsubscribeCompleteEvent = App.Store.Subscribe(typeof(DzGameCompleteEvent), (s, e) => {
                Dispatcher.Invoke(() => {
                    ResultStackpanel.Visibility = Visibility.Visible;
                    WordStackpanel.Visibility = Visibility.Collapsed;
                    LoadingBar.Visibility = Visibility.Collapsed;
                });
            }, false);
        }

        /// <summary>
        /// 当用户关闭标签
        /// </summary>
        /// <param name="param"></param>
        public void OnDestroy(object param = null) {
            // 取消订阅
            UnsubscribeCompleteEvent?.Invoke();
        }

        /// <summary>
        /// 输入框文字发生了变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputBox_TextChanged(object sender, TextChangedEventArgs e) {
            // 未刷新出单词敲击是无意义的
            if (WordStackpanel.Visibility != Visibility.Visible || !ViewModel.HasPrepared()) {
                return;
            }
            var inputBox = sender as TextBox;
            if (inputBox.Text == null) {
                return;
            }
            // 忽略掉无意义的空格
            var word = inputBox.Text;
            if (word.StartsWith(" ")) {
                inputBox.Text = null;
                return;
            }
            // 只有当包含了空格才对单词进行处理
            if (word.Contains(" ")) {
                var inputWords = word.Split(' ');
                ViewModel.MoveNextWord(inputWords[0]);
                // 递归处理剩下的部分
                var restText = new StringBuilder();
                for (int i = 1; i < inputWords.Length; i++) {
                    restText.Append(inputWords[i]);
                }
                // 这里会触发 InputBox_TextChanged 方法
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
            InputBox.Text = null;
            WordStackpanel.Visibility = Visibility.Collapsed;
            LoadingBar.Visibility = Visibility.Visible;
            ViewModel.Refresh();
            await Task.Delay(2000);
            Application.Current.Dispatcher.Invoke(() => {
                WordStackpanel.Visibility = Visibility.Visible;
                LoadingBar.Visibility = Visibility.Collapsed;
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
