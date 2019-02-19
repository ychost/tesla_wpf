

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
        public string Avatar { get; set; }

        private ImageSource avatarImage;
        [JsonIgnore]
        public ImageSource AvatarImage {
            get {
                if (avatarImage == null) {
                    try {
                        avatarImage = AssetsHelper.FetchImage(Avatar);
                    } catch {
                        return AssetsHelper.UserImaggeSource;
                    }
                }
                return avatarImage;
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

    }
}
