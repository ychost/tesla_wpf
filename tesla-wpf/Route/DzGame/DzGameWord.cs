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
        /// <summary>
        /// 
        /// </summary>
        public WordState WordState { get => GetProperty<WordState>(); set => SetProperty(value); }
        /// <summary>
        /// 单词的语言
        /// </summary>
        public WordLanguage Language { get; set; }
        /// <summary>
        /// 单词类型，比如简单，困难
        /// </summary>
        public WordType WordType { get; set; }

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
        // 一个单词未完成，但是中途错误了
        WrongTyping,
        // 单词已完成，敲打错误
        Wrong,
        // 敲打成功
        Corrected
    }

    public enum WordLanguage {
        English = 100,
        Chinese = 110
    }

    public enum WordType {
        Easy = 100,
        Normal = 110,
        Hard = 120,
    }
}
