using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Route.ViewModel {
    /// <summary>
    /// 
    /// </summary>
    public class TypingGameViewModel : BaseViewModel {
        public string CurrentWord { get => GetProperty<string>(); set => SetProperty(value); }
        /// <summary>
        /// 第一行的单词
        /// </summary>
        public List<string> Row1Words { get => GetProperty<List<string>>(); set => SetProperty(value); }
        /// <summary>
        /// 第二行的单词
        /// </summary>
        public List<string> Row2Words { get => GetProperty<List<string>>(); set => SetProperty(value); }

        /// <summary>
        /// 运行时初始化
        /// </summary>
        protected override void InitRuntimeData() {
        }


        /// <summary>
        /// 设计时初始化
        /// </summary>
        protected override void InitDesignData() {
            Row1Words = new List<string>() {
                    "hello","world"
                };
        }

    }
}
