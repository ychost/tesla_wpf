

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using tesla_wpf.Helper;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Model.GameTop {
    /// <summary>
    /// 一项排行数据
    /// <date>2019-2-18</date>
    /// </summary>
    public class RankInfo : BaseViewModel {
        /// <summary>
        /// 用户头像
        /// </summary>
        //public string Avatar { get; set; }
        public string Avatar {
            get => GetProperty<string>(); set {
                if (SetProperty(value)) {
                    initAvatar(value);
                }
            }
        }


        [JsonIgnore]
        public ImageSource AvatarImage { get => GetProperty<ImageSource>(); set => SetProperty(value); }

        /// <summary>
        /// 由于下载图片方法是异步的，所以只能自己手工初始化头像
        /// </summary>
        /// <param name="url"></param>
        private async void initAvatar(string url) {
            if (AvatarImage == null) {
                try {
                    AvatarImage = await AssetsHelper.FetchImage(url);
                } catch {
                    AvatarImage = AssetsHelper.UserImaggeSource;
                }
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 计算的得分
        /// </summary>
        public double CalcScore { get; set; }

        /// <summary>
        /// 原始得分
        /// </summary>
        [Edit(ItemName = "分数", ItemIcon = PackIconKind.Score)]
        [Required]
        public string GameScore { get; set; }


        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int RanNo { get; set; }

        /// <summary>
        /// 分数截图证据
        /// </summary>
        [Edit(ItemName = "截图", ItemIcon = PackIconKind.Image, ItemType = EditItemType.FileChoose,
            FileChooseFilter = "(*.jpg *.png *.jpeg) | *.jpg;*.png;*.jpeg", FileChooseTitle = "选择截图")]
        [Required]
        public string EvidenceImage { get; set; }

    }
}
