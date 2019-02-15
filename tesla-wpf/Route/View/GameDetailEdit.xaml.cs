using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using tesla_wpf.Extensions;
using tesla_wpf.Model;
using tesla_wpf.Model.Game;
using tesla_wpf.Rest;
using Vera.Wpf.Lib.Helper;

namespace tesla_wpf.Route.View {
    /// <summary>
    /// GameDetailEdit.xaml 的交互逻辑
    /// </summary>
    public partial class GameDetailEdit : UserControl, IDynamicMenu, IMenuInit {
        private Game game;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public GameDetailEdit(Game game) {
            InitializeComponent();
            this.game = game;
            MdPreview.MdText = game.MarkdownContent;
        }

        public void OnInit(object param = null) {
            MarkdownViewer.SetRenderHook(new RenderHook());
            MdEditor.UploadImageAction = async mdImage => {
                //todo 暂时用 Web 端代理上传
                // 等腾讯云 COS 上线了 C# 的 SDK 后再替换
                var stream = new FileStream(mdImage.ImagePath, FileMode.Open, FileAccess.Read);
                var rest = await HttpRestService.ForAuthApi<RsSystemApi>()
                            .UploadImage(new Refit.StreamPart(stream,
                                        System.IO.Path.GetFileName(mdImage.ImagePath),
                                        "image/jepg"
                            ));
                if (rest.Code != 0) {
                    throw new Exception(rest.Message);
                }
                return $"{rest.Data}?width={mdImage.Width}&height={mdImage.Height}";
            };
            MdEditor.PreviewClickEvent += text => {
                this.MdPreview.MdText = text;
            };
        }
    }

    /// <summary>
    /// 扩展 Markdown 渲染属性
    /// <date>2019-2-15</date>
    /// </summary>
    internal class RenderHook : IWpfRenderHook {
        public Image RenderImage(string url) {
            url = "https://tesla-1252572735.cos.ap-chengdu.myqcloud.com" + url;
            // 后面的都是本地解析的属性数据
            url = url.Split('?')[0];
            using (var stream = FileCacheHelper.Hit(url)) {
                var source = new BitmapImage();
                source.BeginInit();
                source.StreamSource = stream;
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.EndInit();
                source.Freeze();
                return new Image() { Source = source };
            }
        }
    }
}
