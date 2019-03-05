using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Route.DzGame {
    /// <summary>
    /// 成绩得分
    /// <date>2019-3-5</date>
    /// </summary>
    public class DzGameScore : BaseViewModel {
        /// <summary>
        /// 正确击键次数
        /// </summary>
        public int Correctstrokes {
            get => GetProperty<int>(); set {
                SetProperty(value);
                RaisePropertiesChanged(nameof(Keystrokes), nameof(WPM), nameof(Accuracy));
            }
        }

        /// <summary>
        /// 错误击键次数
        /// </summary>
        public int WrongKeystrokes {
            get => GetProperty<int>(); set {
                SetProperty(value);
                RaisePropertiesChanged(nameof(Keystrokes), nameof(WPM), nameof(Accuracy));
            }
        }

        /// <summary>
        /// 每分钟的单词量
        /// </summary>
        public int WPM => Correctstrokes / 5;

        /// <summary>
        /// 总的击键次数
        /// </summary>
        public int Keystrokes => Correctstrokes + WrongKeystrokes;

        /// <summary>
        /// 准确度
        /// </summary>
        public double Accuracy => (double)Correctstrokes / Keystrokes;

        /// <summary>
        /// 敲正确的单词数
        /// </summary>
        public int CorrectWords { get => GetProperty<int>(); set => SetProperty(value); }

        /// <summary>
        /// 敲错误的单词数
        /// </summary>
        public int WrongWords { get => GetProperty<int>(); set => SetProperty(value); }
    }
}
