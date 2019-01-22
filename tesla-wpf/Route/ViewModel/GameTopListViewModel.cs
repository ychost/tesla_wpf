using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tesla_wpf.Model.Game;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Route.ViewModel {
    /// <summary>
    /// 游戏列表页面模型
    /// <date></date>
    /// </summary>
    public class GameTopListViewModel : BaseViewModel {
        public List<Game> Games { get => GetProperty<List<Game>>(); set => SetProperty(value); }

        protected override void InitRuntimeData() {
            InitDesignData();
        }

        protected override void InitDesignData() {
            Games = new List<Game>() {
                new Game() {
                    Name = "泰拉瑞亚",
                    Description = "我是牛逼的沙盒游戏，宇宙第一"
                },
                new Game() {
                    Name = "Factorio",
                    Description = "我才是宇宙最屌的沙盒游戏"
                },
                new Game() {
                    Name = "泰拉瑞亚",
                    Description = "我是牛逼的沙盒游戏，宇宙第一"
                },
                new Game() {
                    Name = "Factorio",
                    Description = "我才是宇宙最屌的沙盒游戏"
                },new Game() {
                    Name = "泰拉瑞亚",
                    Description = "我是牛逼的沙盒游戏，宇宙第一"
                },
                new Game() {
                    Name = "Factorio",
                    Description = "我才是宇宙最屌的沙盒游戏"
                },
                new Game() {
                    Name = "泰拉瑞亚",
                    Description = "我是牛逼的沙盒游戏，宇宙第一"
                },
                new Game() {
                    Name = "Factorio",
                    Description = "我才是宇宙最屌的沙盒游戏"
                },
                new Game() {
                    Name = "泰拉瑞亚",
                    Description = "我是牛逼的沙盒游戏，宇宙第一"
                },
                new Game() {
                    Name = "Factorio",
                    Description = "我才是宇宙最屌的沙盒游戏"
                },new Game() {
                    Name = "泰拉瑞亚",
                    Description = "我是牛逼的沙盒游戏，宇宙第一"
                },
                new Game() {
                    Name = "Factorio",
                    Description = "我才是宇宙最屌的沙盒游戏"
                }

            };

        }
    }
}
