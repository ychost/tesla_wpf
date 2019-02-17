

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MaterialDesignThemes.Wpf;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Model.GameTop {
    /// <summary>
    /// 一个游戏排行的项目
    /// <date>2019-2-16</date>
    /// </summary>
    public class RankType : BaseViewModel {
        /// <summary>
        /// 序号, 显示用的
        /// </summary>
        public int Index { get => GetProperty(() => Index); set => SetProperty(() => Index, value); }
        /// <summary>
        /// 排行项目名
        /// </summary>
        [Edit(ItemName = "名称", ItemIcon = PackIconKind.Edit)]
        [Required, StringLength(50, MinimumLength = 1, ErrorMessage = "长度应在[1,50]")]
        public string Name { get => GetProperty<string>(); set => SetProperty(value); }

        /// <summary>
        /// 描述
        /// </summary>
        [Edit(ItemName = "描述", ItemIcon = PackIconKind.Details)]
        [Required, StringLength(140, MinimumLength = 1, ErrorMessage = "长度应在[1,140]")]
        public string Description { get => GetProperty<string>(); set => SetProperty(value); }

        /// <summary>
        /// 所属游戏名字
        /// </summary>
        public string GameName { get => GetProperty(() => GameName); set => SetProperty(() => GameName, value); }

        /// <summary>
        /// 备注
        /// </summary>
        [Edit(ItemName = "备注", ItemIcon = PackIconKind.Note)]
        public string Remark { get => GetProperty(() => Remark); set => SetProperty(() => Remark, value); }

        /// <summary>
        /// 排行方法
        /// </summary>
        [Edit(ItemName = "方式", ItemType = EditItemType.Combobox, ItemIcon = PackIconKind.Meteor)]
        [Required]
        public int RankMethod {
            get => GetProperty(() => RankMethod); set {
                SetProperty(() => RankMethod, value);
                RankMethodStr = (from item in RankMethodItems where (int)item.Item == RankMethod select item.Name).FirstOrDefault() ?? "未知";
            }
        }
        /// <summary>
        /// RankMethod 的显示内容
        /// </summary>
        public string RankMethodStr { get => GetProperty(() => RankMethodStr); set => SetProperty(() => RankMethodStr, value); }




        public static List<EditComboboxItem> RankMethodItems;
        static RankType() {
            RankMethodItems = BuildRankMethodItems();
        }
        /// <summary>
        /// 构建 RankMethod 的选项
        /// </summary>
        /// <returns></returns>
        public static List<EditComboboxItem> BuildRankMethodItems() {
            var item1 = new EditComboboxItem() {
                Name = "分数越高越牛逼",
                Item = 1,
                IsSelected = true
            };
            var item2 = new EditComboboxItem() {
                Name = "分数越低越牛逼",
                Item = 2,
            };
            var item3 = new EditComboboxItem() {
                Name = "时间越长越牛逼",
                Item = 3,
            };

            var item4 = new EditComboboxItem() {
                Name = "时间越短越牛逼",
                Item = 4
            };
            return new List<EditComboboxItem>() {
                        item1,item2,item3,item4
                    };
        }
    }
}
