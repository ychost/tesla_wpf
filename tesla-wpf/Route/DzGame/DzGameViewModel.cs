using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Route.DzGame {
    /// <summary>
    /// 打字游戏
    /// <date>2019-3-3</date>
    /// </summary>
    public class DzGameViewModel : BaseViewModel {
        public DzGameViewModel() {
        }
        public List<DzGameWord> Line1Words { get => GetProperty<List<DzGameWord>>(); set => SetProperty(value); }
        public List<DzGameWord> Line2Words { get => GetProperty<List<DzGameWord>>(); set => SetProperty(value); }
        protected override void InitDesignData() {

            Line1Words = new List<DzGameWord>();
            Line2Words = new List<DzGameWord>();
            Line1Words.Add(new DzGameWord() { Word = "animal", WordState = WordState.Corrected });
            Line1Words.Add(new DzGameWord() { Word = "than", WordState = WordState.Wrong });
            Line1Words.Add(new DzGameWord() { Word = "name", WordState = WordState.Typing });
            Line1Words.Add(new DzGameWord() { Word = "up" });
            Line1Words.Add(new DzGameWord() { Word = "never" });
            Line1Words.Add(new DzGameWord() { Word = "does" });
            Line1Words.Add(new DzGameWord() { Word = "big" });
            Line1Words.Add(new DzGameWord() { Word = "talk" });
            Line1Words.Add(new DzGameWord() { Word = "close" });
            Line1Words.Add(new DzGameWord() { Word = "the" });
            Line1Words.Add(new DzGameWord() { Word = "now" });
            Line1Words.Add(new DzGameWord() { Word = "who" });
            Line1Words.Add(new DzGameWord() { Word = "have" });

            Line2Words.Add(new DzGameWord() { Word = "sometimes" });
            Line2Words.Add(new DzGameWord() { Word = "under" });
            Line2Words.Add(new DzGameWord() { Word = "without" });
            Line2Words.Add(new DzGameWord() { Word = "way" });
            Line2Words.Add(new DzGameWord() { Word = "me" });
            Line2Words.Add(new DzGameWord() { Word = "earth" });
            Line2Words.Add(new DzGameWord() { Word = "want" });
            Line2Words.Add(new DzGameWord() { Word = "many" });
            Line2Words.Add(new DzGameWord() { Word = "stop" });
            Line2Words.Add(new DzGameWord() { Word = "enough" });
            Line2Words.Add(new DzGameWord() { Word = "all" });
            Line2Words.Add(new DzGameWord() { Word = "mother" });
        }
    }
}
