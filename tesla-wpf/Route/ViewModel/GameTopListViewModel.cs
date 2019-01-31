using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using tesla_wpf.Extensions;
using tesla_wpf.Model.Game;
using tesla_wpf.Model.GameTop;
using tesla_wpf.Model.Setting;
using tesla_wpf.Rest;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Exceptions;
using Vera.Wpf.Lib.Extensions;
using Vera.Wpf.Lib.Helper;
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
            var editGame = new GameAdd() {
                Name = "泰拉瑞亚",
                Description = "沙盒游戏",
                OfficialWebsite = "haha",
                Remark = "remark"
            };
            var dialog = new EditDialog("添加游戏", items, editGame, 200);
            var game = await DialogHost.Show(dialog) as GameAdd;
            if (game == null) {
                return;
            }
            FileStream coverStream = null;
            try {
                DialogHost.Show(LoadingDialog.Create(Visibility.Visible, "正在上传..."));
                coverStream = new FileStream(game.CoverPath, FileMode.Open, FileAccess.Read);
                var rest = await HttpRestService.ForAuthApi<RsGameTopApi>().UploadCover(
                    game.Name?.Escape(), game.Description?.Escape(), game.Remark?.Escape(), game.OfficialWebsite?.Escape(),
                     new Refit.StreamPart(coverStream, Path.GetFileName(game.CoverPath), "image/jpeg"));
                if (HttpRestService.ForData(rest, out var data)) {
                    if (!data) {
                        throw new UploadException(rest.Message);
                    }
                    NotifyHelper.ShowSuccessMessage("上传成功!");
                }
            } catch (UploadException e) {
                NotifyHelper.ShowErrorMessage(e.Message);
                return;
            } catch (Exception e) {
                NotifyHelper.ShowErrorMessage("上传失败!");
                return;
            } finally {
                coverStream?.Close();
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
        }

        protected async override void InitRuntimeData() {
            try {
                var rest = await HttpRestService.ForAuthApi<RsGameTopApi>().FetchGames();
                if (HttpRestService.ForData(rest, out var games)) {
                    Application.Current.Dispatcher.Invoke(() => {
                        Games = games;
                    });
                } else {
                    NotifyHelper.ShowErrorMessage(rest.Message);
                }
            } catch (Exception e) {
                NotifyHelper.ShowErrorMessage("加载游戏数据失败!");
            }
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
                    Description = "我是牛逼的沙盒游戏，宇宙第一，" +
                    "呜哈哈哈哈哈,我是牛逼的沙盒游戏，宇宙第一，呜哈哈哈哈哈,我是牛逼的沙盒游戏，" +
                    "宇宙第一，呜哈哈哈哈哈,我是牛逼的沙盒游戏，宇宙第一，呜哈哈哈哈哈,我是牛逼的沙盒游戏，" +
                    "宇宙第一，呜哈哈哈哈哈,我是牛逼的沙盒游戏，宇宙第一，呜哈哈哈哈哈,",
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
