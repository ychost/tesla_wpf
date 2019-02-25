using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SQLite;
using tesla_wpf.Helper;
using Vera.Wpf.Lib.Model;

namespace tesla_wpf.Model.Setting {
    [Table(nameof(User))]
    public class User : BaseSqliteEntity, INotifyPropertyChanged {
        public string Token { get; set; }
        private string avatar;
        public string Avatar {
            get => avatar;
            set {
                if (avatar != value) {
                    avatar = value;
                    onPropertyChanged(nameof(Avatar));
                    initAvatarImage(value);
                }
            }
        }

        [Unique(Unique = true)]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// 异步获取头像图片
        /// </summary>
        /// <returns></returns>
        private ImageSource avatarImageSource;
        [Ignore]
        public ImageSource AvatarImageSource {
            get => avatarImageSource;
            set {
                if (avatarImageSource != value) {
                    avatarImageSource = value;
                    onPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 异步初始化头像图片
        /// </summary>
        /// <param name="url"></param>
        private async void initAvatarImage(string url) {
            if (AvatarImageSource == null) {
                try {
                    AvatarImageSource = await AssetsHelper.FetchImage(url);
                    //avatarImageSource = AssetsHelper.LoadLocalAvatar(Name);
                } catch (Exception e) {
                    App.Logger.Error("下载头像失败：" + e.Message);
                    AvatarImageSource = AssetsHelper.UserImaggeSource;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
