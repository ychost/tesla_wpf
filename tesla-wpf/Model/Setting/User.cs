using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SQLite;
using tesla_wpf.Helper;
using Vera.Wpf.Lib.Model;

namespace tesla_wpf.Model.Setting {
    [Table("User")]
    public class User : BaseSqliteEntity {
        public string Token { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }


        private ImageSource avatarImageSource;
        /// <summary>
        /// 异步获取头像图片
        /// </summary>
        /// <returns></returns>
        public ImageSource AvatarImageSource {
            get {
                if (avatarImageSource != null) {
                    return avatarImageSource;
                }
                if (avatarImageSource == null) {
                    try {
                        avatarImageSource = AssetsHelper.FetchImage(Avatar);
                    } catch (Exception e) {
                        Console.WriteLine(e);
                        return AssetsHelper.UserImaggeSource;
                    }
                }
                return avatarImageSource;
            }
        }
    }
}
