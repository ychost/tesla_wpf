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

        [Required,StringLength(maximumLength:512, ErrorMessage = "长度不能超过 512")]
        public string Description { get => GetProperty(() => Description); set => SetProperty(() => Description, value); }


        [Required(ErrorMessage = "游戏封面不能为空")]
        public string CoverUrl { get => GetProperty(() => CoverUrl); set => SetProperty(() => CoverUrl, value); }

        [StringLength(maximumLength: 128, ErrorMessage = "连接长度不能超过 128")]
        public string OfficialWebsite { get => GetProperty(() => OfficialWebsite); set => SetProperty(() => OfficialWebsite, value); }

        public string MarkdownContent { get => GetProperty(() => MarkdownContent); set => SetProperty(() => MarkdownContent, value); }
        public string Remark { get => GetProperty(() => Remark); set => SetProperty(() => Remark, value); }
        /// <summary>
        /// 排行前三的用户
        /// </summary>
        [JsonIgnore]
        public List<User> Top3Users { get; set; }
        private ImageSource coverImage;
        [JsonIgnore]
        public ImageSource CoverImage {
            get {
                try {
                    if (coverImage == null) {
                        coverImage = AssetsHelper.FetchCloudImage(CoverUrl);
                    }
                } catch (Exception e) {
                    return AssetsHelper.MountainImageSource;
                }
                return coverImage;
            }
        }
    }
}
