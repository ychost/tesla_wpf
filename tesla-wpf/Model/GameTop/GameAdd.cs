
using System.ComponentModel.DataAnnotations;
using System.IO;
using MaterialDesignThemes.Wpf;
using Vera.Wpf.Lib.Component;

namespace tesla_wpf.Model.GameTop {
    /// <summary>
    /// 添加游戏
    /// <date>2019-1-30</date>
    /// </summary>
    public class GameAdd {
        /// <summary>
        /// 游戏名
        /// </summary>
        [Edit(ItemName = "名称", ItemIcon = PackIconKind.Gamepad, Order = 1)]
        [StringLength(maximumLength: 50, MinimumLength = 1, ErrorMessage = "名称长度为 [1-50]")]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Edit(ItemName = "描述", ItemIcon = PackIconKind.Details, ItemType = EditItemType.MultiInput, Order = 5)]
        [StringLength(maximumLength: 140, ErrorMessage = "长度不能超过 140")]
        public string Description { get; set; }

        /// <summary>
        /// 游戏官网
        /// </summary>
        [Edit(ItemName = "官网", ItemIcon = PackIconKind.Link, Order = 2)]
        [StringLength(maximumLength: 128, ErrorMessage = "连接长度不能超过 128")]
        public string OfficialWebsite { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Edit(ItemName = "备注", ItemIcon = PackIconKind.Note, Order = 3)]
        public string Remark { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        [Edit(ItemName = "封面", ItemIcon = PackIconKind.FileImage, ItemType = EditItemType.FileChoose, FileChooseFilter = "PNG图片|*.png|JPG图片| *.jpg", Order = 4)]
        [Required(ErrorMessage = "游戏封面不能为空")]
        public string CoverPath { get; set; }
    }
}
