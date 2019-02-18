using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Markdig.Wpf;
using Markdig.Wpf.Extensions;
using MaterialDesignThemes.Wpf;
using tesla_wpf.Extensions;
using tesla_wpf.Helper;
using tesla_wpf.Model;
using tesla_wpf.Model.Game;
using tesla_wpf.Model.GameTop;
using tesla_wpf.Rest;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Extensions;
using Vera.Wpf.Lib.Helper;
using Vera.Wpf.Lib.Mvvm;
using Vera.Wpf.Lib.Toolkit;

namespace tesla_wpf.Route.View {
    /// <summary>
    /// GameDetailEdit.xaml 的交互逻辑
    /// </summary>
    public partial class GameDetailEdit : UserControl, IDynamicMenu, IMenuInit, IMenuAssureDestroy, INotifyPropertyChanged {

        /// <summary>
        ///  当前编辑的游戏数据
        /// </summary>
        public Game Game { get; set; }

        /// <summary>
        /// 注入初始化数据
        /// </summary>
        /// <param name="Game"></param>
        public GameDetailEdit(Game game) {
            InitializeComponent();
            Game = game;
            DataContext = this;
        }

        /// <summary>
        /// 排行数据列表
        /// </summary>
        private List<RankType> rankTypes;
        public List<RankType> RankTypes {
            get => rankTypes;
            set {
                if (rankTypes != value) {
                    rankTypes = value;
                    onPropertyChanged(nameof(RankTypes));
                }
            }
        }


        public ICommand EditRankType => new MdCommand(editRankTypeExec);
        public ICommand DelRankType => new MdCommand(delRankTypeExec);
        public ICommand AddRankType => new MdCommand(addRankTypeExec);
        public ICommand UpdateGameInfoCmd => new MdCommand(updateGameInfoExec, canUpdateGameInfo);



        /// <summary>
        /// 添加排行
        /// </summary>
        /// <param name="obj"></param>
        private async void addRankTypeExec(object obj) {
            var items = buildRankTypeEdits();
            var dialog = new EditDialog("添加排行", items, new RankType());
            if (!(await DialogHost.Show(dialog) is RankType rankType)) {
                return;
            }
            try {
                BaseDataErrorInfo.AssertAttrIsValid(rankType);
            } catch (Exception e) {
                await DialogHostExtension.ShowInMainThread(new ConfirmDialog(e.Message));
                return;
            }
            DialogHostExtension.ShowInMainThread(new LoadingDialog(Visibility.Visible, "正在上传中..."));
            rankType.GameName = Game.Name;
            var rest = await HttpRestService.ForAuthApi<RsGameTopApi>().AddRankType(rankType);
            if (HttpRestService.ForData(rest, out var rs)) {
                refreshRankTypes();
                NotifyHelper.ShowSuccessMessage("添加排行成功");
            } else {
                NotifyHelper.ShowErrorMessage("添加排行失败");
            }
            DialogHostExtension.CloseInMainThread(null, null);
        }

        /// <summary>
        /// 生成添加排行的时候的一些参数选项
        /// </summary>
        /// <returns></returns>
        private List<Edit> buildRankTypeEdits() {
            return Edit.GenerateByType<RankType>(prop => {
                if (prop == PropertyHelper<RankType>.GetProperty(x => x.RankMethod)) {
                    return RankType.RankMethodItems;
                }
                return null;
            });

        }

        /// <summary>
        /// 删除某项排行
        /// </summary>
        /// <param name="obj"></param>
        private void delRankTypeExec(object obj) {

        }

        /// <summary>
        /// 编辑某项排行
        /// </summary>
        /// <param name="obj"></param>
        private void editRankTypeExec(object obj) {

        }


        /// <summary>
        /// 提示用户有文档未保存
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AssureDestroy() {
            if (!MdEditor.IsSaved) {
                var res = (await DialogHost.Show(new ConfirmDialog("文档尚未保存，是否关闭？"))) as bool?;
                return res == true;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public void OnInit(object param = null) {
            MdPreview.MdText = Game.MarkdownContent;
            MdEditor.SetContent(Game.MarkdownContent);
            // 绑定事件
            // 上传 Markdown 图片到服务器
            MdEditor.UploadImageAction = UploadImage;
            // 保存文档到服务器
            MdEditor.SaveClickEvent += UpdateGameInfo;
            // 预览 Markdown 内容
            MdEditor.PreviewClickEvent += text => {
                MdPreview.MdText = text;
            };
            refreshRankTypes();
        }

        /// <summary>
        /// 刷新排行列表数据
        /// </summary>
        private async void refreshRankTypes() {
            // 拉取排行数据
            var rest = await HttpRestService.ForAuthApi<RsGameTopApi>().FetchRankTypes(Game.Name);
            if (HttpRestService.ForData(rest, out var rankTypes)) {
                for (int i = 0; i < rankTypes.Count; i++) {
                    rankTypes[i].Index = i + 1;
                }
                RankTypes = rankTypes;
            } else {
                NotifyHelper.ShowErrorMessage(rest.Message);
            }
        }

        /// <summary>
        /// 上传 Markdown 在编辑的时候插入的图片
        /// </summary>
        /// <param name="mdImage"></param>
        /// <returns></returns>
        async Task<string> UploadImage(MdImage mdImage) {
            // todo 暂时用 Web 端代理上传
            // 等腾讯云 COS 上线了 C# 的 SDK 后再替换
            var stream = new FileStream(mdImage.ImagePath, FileMode.Open, FileAccess.Read);
            var rest = await HttpRestService.ForAuthApi<RsSystemApi>().UploadImage(
                            new Refit.StreamPart(stream, System.IO.Path.GetFileName(mdImage.ImagePath), "image/jepg"
                        ));

            if (rest.Code != HttpRestcode.Success) {
                throw new Exception(rest.Message);
            }
            return $"{rest.Data}?width={mdImage.Width}&height={mdImage.Height}";
        }

        /// <summary>
        /// 更新游戏信息
        /// </summary>
        /// <param name="markdownContent"></param>
        async Task<bool> UpdateGameInfo(string markdownContent) {
            Game.MarkdownContent = markdownContent;
            DialogHost.Show(new LoadingDialog(Visibility.Visible, "正在保存..."));
            try {
                var rest = await HttpRestService.ForAuthApi<RsGameTopApi>().UpdateGameInfo(Game);
                await Task.Delay(400);
                var res = rest.Code == HttpRestcode.Success;
                var message = res == true ? "保存成功！" : "保存到服务器失败, 请检查网络连接";
                Application.Current.Dispatcher.Invoke(() => {
                    DialogHost.CloseDialogCommand.Execute(null, null);
                    //DialogHost.Show(new ConfirmDialog(message));
                    if (res) {
                        NotifyHelper.ShowSuccessMessage(message);
                    } else {
                        NotifyHelper.ShowErrorMessage(message);
                    }
                });
                return res;
            } catch (Exception e) {
                return false;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 显示排行列表编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditDetail_Click(object sender, RoutedEventArgs e) {
            MdGrid.Visibility = Visibility.Collapsed;
            DetailGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 显示 markdown 内容编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditMdContent_Click(object sender, RoutedEventArgs e) {
            DetailGrid.Visibility = Visibility.Collapsed;
            MdGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 选择封面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseCover_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = AssetsHelper.ImageFileter;
            dialog.Title = "选择封面";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                RestProxy.Builder().Try(() => Task.Run(async () => {
                    var file = File.OpenRead(dialog.FileName);
                    var rest = await HttpRestService.ForAuthApi<RsSystemApi>()
                        .UploadImage(new Refit.StreamPart(file, System.IO.Path.GetFileName(dialog.FileName), "image/jepg"));
                    if (!HttpRestService.ForData(rest, out var url)) {
                        throw new Exception(rest.Message);
                    }
                    Game.CoverUrl = url;
                }))
                .Catch(typeof(Exception))
                .LoadingMessage("正在上传封面...")
                .SuccessMessage("上传封面成功")
                .FailedMessage("上传封面失败")
                .Build()
                .Exec();
            }
        }

        /// <summary>
        /// 校验游戏字段
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canUpdateGameInfo(object arg) {
            try {
                BaseDataErrorInfo.AssertAttrIsValid(Game);
            } catch {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新游戏内容
        /// </summary>
        /// <param name="obj"></param>
        private void updateGameInfoExec(object obj) {
            RestProxy.Builder().Try(() =>
               Task.Run(async () => {
                   var rest = await HttpRestService.ForAuthApi<RsGameTopApi>().UpdateGameInfo(Game);
                   if (!(HttpRestService.ForData(rest, out var rs) && rs)) {
                       throw new Exception(rest.Message);
                   }
               }))
             .Catch(typeof(Exception))
            .LoadingMessage("正在上传中...")
            .SuccessMessage("更新成功")
            .FailedMessage("更新失败")
            .Build()
            .Exec();
        }

        /// <summary>
        /// 扩展 Markdown 渲染属性
        /// <date>2019-2-15</date>
        /// </summary>
        internal class RenderHook : IWpfRenderHook {
            public Image RenderImage(string url) {
                if (url.StartsWith("/images/")) {
                    url = "https://tesla-1252572735.cos.ap-chengdu.myqcloud.com" + url;
                }
                // 后面的都是本地解析的属性数据
                url = url.Split('?')[0];
                try {
                    using (var stream = FileCacheHelper.Hit(url)) {
                        var source = new BitmapImage();
                        source.BeginInit();
                        source.StreamSource = stream;
                        source.CacheOption = BitmapCacheOption.OnLoad;
                        source.EndInit();
                        source.Freeze();
                        return new Image() { Source = source };
                    }
                } catch (Exception e) {
                    return new Image();
                }
            }
        }
    }
}
