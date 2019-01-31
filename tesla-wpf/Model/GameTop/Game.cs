using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using tesla_wpf.Helper;
using tesla_wpf.Model.Setting;

namespace tesla_wpf.Model.Game {
    /// <summary>
    /// 游戏模型
    /// </summary>
    public class Game {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string OfficialWebsite;
        public string Remark;
        /// <summary>
        /// 排行前三的用户
        /// </summary>
        public List<User> Top3Users { get; set; }

        private ImageSource coverImage;
        public ImageSource CoverImage {
            get {
                try {
                    if (coverImage == null) {
                        coverImage = AssetsHelper.FetchCloudImageAsync(CoverUrl);
                    }
                } catch (Exception e) {
                    return AssetsHelper.MountainImageSource;
                }
                return coverImage;
            }
        }



    }
}
