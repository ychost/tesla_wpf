using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Route.DzGame {
    /// <summary>
    /// 打字游戏的单词属性
    /// </summary>
    public class DzGameWord : BaseViewModel {
        /// <summary>
        /// 单词本身
        /// </summary>
        public string Word { get => GetProperty<string>(); set => SetProperty(value); }
        public WordState WordState { get => GetProperty<WordState>(); set => SetProperty(value); }

        public DzGameWord() {
            WordState = WordState.Default;
        }

    }

    /// <summary>
    /// 当前单词状态
    /// </summary>
    public enum WordState {
        // 默认状态
        Default,
        // 正在敲打
        Typing,
        // 敲打错误
        Wrong,
        // 敲打成功
        Corrected
    }
}
