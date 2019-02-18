using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tesla_wpf.Model.Game;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Route.ViewModel {
    /// <summary>
    /// 单个游戏详情数据
    /// <date>2019-2-1</date>
    /// </summary>
    public class GameTopDetailViewModel : BaseViewModel {
        public GameTopDetailViewModel() {

        }
        /// <summary>
        /// 
        /// </summary>
        public string MdText { get => GetProperty<string>(); set => SetProperty(value); }
        public Game Game { get; set; }

        public GameTopDetailViewModel(Game game) {
            Game = game;
            MdText = game.MarkdownContent;
        }

        protected override void InitRuntimeData() {

        }

        protected override void InitDesignData() {

        }
    }
}
