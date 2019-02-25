using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;
using tesla_wpf.Helper;
using tesla_wpf.Model.Setting;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Model.Game {
    /// <summary>
    /// 游戏模型
    /// </summary>
    public class Game : BaseViewModel {

        [Required, StringLength(maximumLength: 50, MinimumLength = 1, ErrorMessage = "名称长度为 [1-50]")]
        public string Name { get => GetProperty(() => Name); set => SetProperty(() => Name, value); }

        [Required, StringLength(maximumLength: 512, ErrorMessage = "长度不能超过 512")]
        public string Description { get => GetProperty(() => Description); set => SetProperty(() => Description, value); }


        [Required(ErrorMessage = "游戏封面不能为空")]
        //public string CoverUrl { get => GetProperty(() => CoverUrl); set => SetProperty(() => CoverUrl, value); }
        public string CoverUrl {
            get => GetProperty<string>(); set {
                if (SetProperty(value)) {
                    initCoverImage(value);
                }
            }
        }

        [StringLength(maximumLength: 128, ErrorMessage = "连接长度不能超过 128")]
        public string OfficialWebsite { get => GetProperty(() => OfficialWebsite); set => SetProperty(() => OfficialWebsite, value); }

        public string MarkdownContent { get => GetProperty(() => MarkdownContent); set => SetProperty(() => MarkdownContent, value); }
        public string Remark { get => GetProperty(() => Remark); set => SetProperty(() => Remark, value); }
        /// <summary>
        /// 排行前三的用户
        /// </summary>
        [JsonIgnore]
        public List<User> Top3Users { get; set; }

        public ImageSource CoverImage { get => GetProperty<ImageSource>(); set => SetProperty(value); }

        /// <summary>
        /// 异步初始化封面图片
        /// </summary>
        /// <param name="url"></param>
        private async void initCoverImage(string url) {
            try {
                if (CoverImage == null) {
                    CoverImage = await AssetsHelper.FetchImage(CoverUrl);
                }
            } catch (Exception e) {
                App.Logger.Error("加载封面图片失败：" + e.Message);
                CoverImage = AssetsHelper.MountainImageSource;
            }
        }
    }
}
