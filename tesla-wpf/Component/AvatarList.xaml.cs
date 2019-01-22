using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tesla_wpf.Extensions;
using tesla_wpf.Helper;
using tesla_wpf.Model.Setting;

namespace tesla_wpf.Component {
    /// <summary>
    /// AvatarList.xaml 的交互逻辑
    /// </summary>
    public partial class AvatarList : UserControl {
        public AvatarList() {
            InitializeComponent();
        }




        public List<User> Users {
            get { return (List<User>)GetValue(UsersProperty); }
            set { SetValue(UsersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Users.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsersProperty =
            DependencyProperty.Register("Users", typeof(List<User>), typeof(AvatarList), new PropertyMetadata(null, (d, e) => {
                var view = d as AvatarList;
                SetAvatar(view.Avatar1, view.AvatarGrid1, view.Users, 0, AssetsHelper.UserImaggeSource);
                SetAvatar(view.Avatar2, view.AvatarGrid2, view.Users, 1, AssetsHelper.UserImaggeSource);
                SetAvatar(view.Avatar3, view.AvatarGrid3, view.Users, 2, AssetsHelper.UserImaggeSource);
            }));

        /// <summary>
        /// 设置头像
        /// </summary>
        /// <param name="imageControl">头像控件</param>
        /// <param name="users">用户列表</param>
        /// <param name="index">用户列表中的序号</param>
        /// <param name="failedImage">设置失败的时候头像</param>
        public static void SetAvatar(ImageBrush imageControl, Grid avatarGrid, List<User> users, int index, ImageSource failedImage) {
            if (users.Count > index) {
                var image = users[index].AvatarImageSource;
                avatarGrid.ToolTip = users[index].Name;
                imageControl.ImageSource = image;
            } else {
                imageControl.ImageSource = failedImage;
            }
        }
    }

}
