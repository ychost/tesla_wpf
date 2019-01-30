using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using tesla_wpf.Model.Game;
using tesla_wpf.Model.GameTop;
using tesla_wpf.Model.Setting;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Route.ViewModel {
    /// <summary>
    /// 游戏列表页面模型
    /// <date></date>
    /// </summary>
    public class GameTopListViewModel : BaseViewModel {
        public List<Game> Games { get => GetProperty<List<Game>>(); set => SetProperty(value); }
        public ICommand AddGameCmd => new MdCommand(addGameExec);

        /// <summary>
        /// 添加游戏
        /// </summary>
        /// <param name="obj"></param>
        private async void addGameExec(object obj) {
            var items = Edit.GenerateByType<GameAdd>();
            var dialog = new EditDialog("添加游戏", items, new GameAdd(), 200);
            var game = await DialogHost.Show(dialog) as GameAdd;
            // 上传游戏内容
            Console.WriteLine(game);
        }

        protected override void InitRuntimeData() {
            InitDesignData();
        }

        protected override void InitDesignData() {
            var top3Users = new List<User>() {
                new User() {
                    Name = "金木",
                    Avatar = "https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=585681951,2855353546&fm=26&gp=0.jpg"
                },
            };
            Games = new List<Game>() {
                new Game() {
                    Name = "泰拉瑞亚",
                    Description = "我是牛逼的沙盒游戏，宇宙第一，呜哈哈哈哈哈",
                    Top3Users = top3Users
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
