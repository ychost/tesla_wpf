using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Vera.Wpf.Lib.Component;

namespace tesla_wpf.Model.GameTop {
    /// <summary>
    /// 添加排行记录的模型
    /// <date>2019-2-19</date>
    /// </summary>
    public class RankInfoAdd : RankInfo {
        /// <summary>
        /// 所属的排行类别名字
        /// </summary>
        [Edit(ItemName = "排行", ItemIcon = PackIconKind.FormatListBulleted, IsReadonly = true)]
        public string rankTypeName { get; set; }

        /// <summary>
        /// 游戏名字
        /// </summary>
        [Edit(ItemName = "游戏", ItemIcon = PackIconKind.Games, IsReadonly = true)]
        public string GameName { get; set; }
    }
}
