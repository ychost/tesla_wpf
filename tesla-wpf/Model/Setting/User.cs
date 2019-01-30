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
    [Table(nameof(User))]
    public class User : BaseSqliteEntity {
        public string Token { get; set; }
        public string Avatar { get; set; }
        [Unique(Unique = true)]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        private ImageSource avatarImageSource;
        /// <summary>
        /// 异步获取头像图片
        /// </summary>
        /// <returns></returns>
        [Ignore]
        public ImageSource AvatarImageSource {
            get {
                if (avatarImageSource == null) {
                    try {
                        avatarImageSource = AssetsHelper.FetchImage(Avatar);
                        //avatarImageSource = AssetsHelper.LoadLocalAvatar(Name);
                    } catch (Exception e) {
                        return AssetsHelper.UserImaggeSource;
                    }
                }
                return avatarImageSource;
            }
        }
    }
}
