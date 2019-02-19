using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using tesla_wpf.Extensions;
using tesla_wpf.Model.Game;
using tesla_wpf.Model.GameTop;
using tesla_wpf.Rest;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Exceptions;
using Vera.Wpf.Lib.Helper;
using Vera.Wpf.Lib.Mvvm;
using Vera.Wpf.Lib.Toolkit;

namespace tesla_wpf.Route.ViewModel {
    /// <summary>
    /// 单个游戏详情数据
    /// <date>2019-2-1</date>
    /// </summary>
    public class GameTopDetailViewModel : BaseViewModel {
        /// <summary>
        /// Design Mode 需要无参构造函数
        /// </summary>
        public GameTopDetailViewModel() {

        }
        /// <summary>
        /// 简介的 markdown 文本
        /// </summary>
        public string MdText { get => GetProperty<string>(); set => SetProperty(value); }
        /// <summary>
        /// 游戏数据
        /// </summary>
        public Game Game { get; set; }
        /// <summary>
        /// 排行数据
        /// </summary>
        public List<RankInfo> RankInfos { get => GetProperty<List<RankInfo>>(); set => SetProperty(value); }
        /// <summary>
        /// 排行类别
        /// </summary>
        public List<RankType> RankTypes { get => GetProperty<List<RankType>>(); set => SetProperty(value); }
        /// <summary>
        /// 切换选中的排行
        /// </summary>
        public ICommand SwitchRankTypeCmd => new MdCommand(switchRankTypeExec);
        public ICommand AddRankInfoCmd => new MdCommand(addRankInfoExec);
        public ICommand DelRankInfoCmd => new MdCommand(delRankInfoExec);

        /// <summary>
        /// 排行内容缓存
        /// 用户每切换一个排行类别,那么就会缓存一次
        /// </summary>
        public IDictionary<string, List<RankInfo>> RankInfosCache = new Dictionary<string, List<RankInfo>>();

        /// <summary>
        /// 注入 Game 数据
        /// </summary>
        /// <param name="game"></param>
        public GameTopDetailViewModel(Game game) {
            Game = game;
            MdText = game.MarkdownContent;
        }

        /// <summary>
        /// 拉取排行数据等等
        /// </summary>
        protected override void InitRuntimeData() {
            RestProxy.Builder().Try(() => Task.Run(async () => {
                var rest = await HttpRestService.ForAuthApi<RsGameTopApi>().FetchRankTypes(Game.Name);
                if (HttpRestService.ForData(rest, out var data)) {
                    if (data == null || data.Count == 0) {
                        return;
                    }
                    RankTypes = data;
                    RankTypes[0].IsChecked = true;
                    refreshRankInfo(RankTypes[0].Name);
                } else {
                    throw new RestFailedException(rest.Message);
                }
            }))
            .Catch(typeof(Exception))
            .LoadingMessage("加载中...")
            .FailedMessage("加载排行数据失败")
            .Build()
            .Exec();
        }

        protected override void InitDesignData() {

        }

        /// <summary>
        /// 切换选中的排行
        /// </summary>
        /// <param name="obj"></param>
        private void switchRankTypeExec(object obj) {
            var rankType = obj as RankType;
            if (rankType == null) {
                return;
            }
            foreach (var type in RankTypes) {
                type.IsChecked = false;
            }
            rankType.IsChecked = true;
            refreshRankInfo(rankType.Name);
        }

        /// <summary>
        /// 刷新具体的排行内容,根据排行名
        /// </summary>
        /// <param name="rankType"></param>
        private void refreshRankInfo(string rankTypeName) {
            if (RankInfosCache.TryGetValue(rankTypeName, out var infos)) {
                RankInfos = infos;
                return;
            }
            RestProxy.Builder().Try(() => Task.Run(async () => {
                var rest = await HttpRestService.ForAuthApi<RsGameTopApi>().FetchRankInfos(Game.Name, rankTypeName);
                if (HttpRestService.ForData(rest, out var fetchInfos)) {
                    RankInfosCache[rankTypeName] = fetchInfos;
                    RankInfos = fetchInfos;
                } else {
                    throw new RestFailedException(rest.Message);
                }
            }))
            .Catch(typeof(Exception))
            .FailedMessage("加载排行数据失败")
            .Build()
            .Exec();
        }

        /// <summary>
        /// 添加一个排名数据
        /// </summary>
        /// <param name="obj"></param>
        private async void addRankInfoExec(object obj) {
            if (SelectedRankType == null) {
                NotifyHelper.ShowWarnMessage("请先完善游戏的排行属性");
                return;
            }
            var items = Edit.GenerateByType<RankInfoAdd>();
            var infoAdd = new RankInfoAdd() {
                GameName = Game.Name,
                rankTypeName = SelectedRankType.Name,
                User = App.User.Name
            };
            var dialog = new EditDialog("添加排行记录", items, infoAdd);
            if (!(await DialogHost.Show(dialog) is RankInfoAdd infoAddRes)) {
                return;
            }
            RestProxy.Builder().Try(() => Task.Run(async () => {
                var rest = await HttpRestService.ForAuthApi<RsGameTopApi>().AddRankInfo(infoAddRes);
                if (!(HttpRestService.ForData(rest, out var rs) && rs.Value)) {
                    throw new RestFailedException(rest.Message);
                }
            }))
            .Catch(typeof(Exception))
            .LoadingMessage("上传中...")
            .SuccessMessage("添加成功")
            .FailedMessage("添加失败")
            .SuccessAction(() => refreshRankInfo(SelectedRankType.Name))
            .Build()
            .Exec();
        }

        /// <summary>
        /// 删除某个排名数据
        /// </summary>
        /// <param name="obj"></param>
        private void delRankInfoExec(object obj) {

        }

        /// <summary>
        /// 当前选中的排行类别
        /// </summary>
        public RankType SelectedRankType {
            get {
                if (RankTypes == null) {
                    return null;
                }
                return (from r in RankTypes where r.IsChecked == true select r).First();
            }
        }

    }
}
