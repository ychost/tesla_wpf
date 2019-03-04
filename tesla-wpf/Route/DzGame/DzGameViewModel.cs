﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;
using tesla_wpf.Helper;
using Vera.Wpf.Lib.Helper;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Route.DzGame {
    /// <summary>
    /// 打字游戏
    /// <date>2019-3-3</date>
    /// </summary>
    public class DzGameViewModel : BaseViewModel {
        /// <summary>
        /// 当前选中的单词，第一行的索引
        /// </summary>
        private int currentIndex;
        /// <summary>
        /// 错误的击键次数
        /// </summary>
        private int wrongKeystore;
        /// <summary>
        /// 正确的击键次数
        /// </summary>
        private int correctKeystore;
        /// <summary>
        /// 是否开始了
        /// </summary>
        public bool IsStarted;

        public Timer CalcTimer;

        public DzGameViewModel() {
        }

        public List<DzGameWord> Line1Words { get => GetProperty<List<DzGameWord>>(); set => SetProperty(value); }
        public List<DzGameWord> Line2Words { get => GetProperty<List<DzGameWord>>(); set => SetProperty(value); }
        /// <summary>
        /// 时间
        /// </summary>
        public string TimeStr { get => GetProperty<string>(); set => SetProperty(value); }
        /// <summary>
        /// 剩余的秒数
        /// </summary>
        public int RestSec;

        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh() {
            TimeStr = "1:00";
            RestSec = 60;
            IsStarted = false;
            TimeHelper.ClearTimeout(CalcTimer);
        }

        /// <summary>
        /// 移动到下一个单词
        /// </summary>
        /// <param name="inputedWord">上个单词输入数据</param>
        public void MoveNextWord(string inputedWord) {
            var word = Line1Words[currentIndex];
            // 单词失败
            if (word.Word != inputedWord) {
                word.WordState = WordState.Wrong;
                wrongKeystore += inputedWord.Length;
                // 单词正确
            } else {
                word.WordState = WordState.Corrected;
                correctKeystore += inputedWord.Length;
            }
            // 空格是正确的
            correctKeystore += 1;
            // 换行了
            if (currentIndex == Line1Words.Count - 1) {
                currentIndex = 0;
                Line1Words = Line2Words;
                Line2Words = FetchOneLineWords();
            } else {
                currentIndex += 1;
            }
            Line1Words[currentIndex].WordState = WordState.Typing;
        }

        /// <summary>
        /// 处理当前单词
        /// </summary>
        /// <param name="inputedWord"></param>
        public void HandleCurrentWord(string inputedWord) {
            if (!IsStarted) {
                IsStarted = true;
                CalcTimer = TimeHelper.SetInterval(1000, () => {
                    RestSec -= 1;
                    TimeStr = "0:" + RestSec.ToString("00");
                    // 一次游戏完成
                    if (RestSec == 0) {
                        TimeHelper.ClearTimeout(CalcTimer);
                        Complete();
                    }
                });
            }
            var word = Line1Words[currentIndex];
            if (!word.Word.StartsWith(inputedWord)) {
                word.WordState = WordState.WrongTyping;
            } else {
                word.WordState = WordState.Typing;
            }
        }

        /// <summary>
        /// 从 10fastfingers 上面获取分数图片
        /// </summary>
        /// <returns></returns>
        public async Task<ImageSource> FetchWpmScoreImage(int score) {
            var str = Convert.ToString(score, 24);
            var image = await AssetsHelper.FetchNetworkImage(str);
            return image;
        }


        /// <summary>
        /// 完成一次游戏
        /// </summary>
        public void Complete() {

        }

        bool flag = true;
        /// <summary>
        /// 获取一行的单词数据
        /// </summary>
        /// <returns></returns>
        private List<DzGameWord> FetchOneLineWords() {
            var words = new List<DzGameWord>();
            if (flag) {
                words.Add(new DzGameWord() { Word = "animal" });
                words.Add(new DzGameWord() { Word = "than" });
                words.Add(new DzGameWord() { Word = "name" });
                words.Add(new DzGameWord() { Word = "up" });
                words.Add(new DzGameWord() { Word = "never" });
                words.Add(new DzGameWord() { Word = "does" });
                words.Add(new DzGameWord() { Word = "big" });
                words.Add(new DzGameWord() { Word = "talk" });
                words.Add(new DzGameWord() { Word = "close" });
                words.Add(new DzGameWord() { Word = "the" });
                words.Add(new DzGameWord() { Word = "now" });
                words.Add(new DzGameWord() { Word = "who" });
                words.Add(new DzGameWord() { Word = "have" });
                words.Add(new DzGameWord() { Word = "times" });
            } else {
                words.Add(new DzGameWord() { Word = "sometimes" });
                words.Add(new DzGameWord() { Word = "under" });
                words.Add(new DzGameWord() { Word = "without" });
                words.Add(new DzGameWord() { Word = "way" });
                words.Add(new DzGameWord() { Word = "me" });
                words.Add(new DzGameWord() { Word = "earth" });
                words.Add(new DzGameWord() { Word = "want" });
                words.Add(new DzGameWord() { Word = "many" });
                words.Add(new DzGameWord() { Word = "stop" });
                words.Add(new DzGameWord() { Word = "enough" });
                words.Add(new DzGameWord() { Word = "all" });
                words.Add(new DzGameWord() { Word = "mother" });
            }
            flag = !flag;
            return words;
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        protected override void InitRuntimeData() {
            Line1Words = FetchOneLineWords();
            Line1Words[0].WordState = WordState.Typing;
            Line2Words = FetchOneLineWords();
            Refresh();
        }


        protected override void InitDesignData() {
            Line1Words = FetchOneLineWords();
            Line1Words[0].WordState = WordState.Typing;
            Line2Words = FetchOneLineWords();
            Refresh();
        }
    }
}
