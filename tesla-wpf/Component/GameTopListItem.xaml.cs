using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MaterialDesignThemes.Wpf;
using tesla_wpf.Helper;
using tesla_wpf.Model.Setting;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Helper;

namespace tesla_wpf.Component {
    /// <summary>
    /// GameTopListItem.xaml 的交互逻辑
    /// </summary>
    public partial class GameTopListItem : UserControl {
        /// <summary>
        /// 
        /// </summary>
        public GameTopListItem() {
            InitializeComponent();
        }

        /// <summary>
        /// 排行前三的用户
        /// </summary>
        public List<User> Top3Users {
            get { return (List<User>)GetValue(Top3UsersProperty); }
            set { SetValue(Top3UsersProperty, value); }
        }


        /// <summary>
        /// 游戏封面
        /// </summary>
        public ImageSource Cover {
            get { return (ImageSource)GetValue(CoverProperty); }
            set { SetValue(CoverProperty, value); }
        }


        /// <summary>
        /// 游戏标题
        /// </summary>
        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        /// <summary>
        ///  游戏描述
        /// </summary>
        public string Description {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }


        /// <summary>
        /// 用户点击了查看游戏
        /// </summary>
        public ICommand OnViewGameCmd {
            get { return (ICommand)GetValue(OnViewGameCmdProperty); }
            set { SetValue(OnViewGameCmdProperty, value); }
        }

        /// <summary>
        /// 用户点击了编辑游戏
        /// </summary>
        public ICommand OnEditGameCmd {
            get { return (ICommand)GetValue(OnEditGameCmdProperty); }
            set { SetValue(OnEditGameCmdProperty, value); }
        }

        /// <summary>
        /// 是否具有编辑权限
        /// </summary>
        public bool CanEdit {
            get { return (bool)GetValue(CanEditProperty); }
            set { SetValue(CanEditProperty, value); }
        }

        
        private void ViewGame_Click(object sender, RoutedEventArgs e) {
            OnViewGameCmd?.Execute(this.DataContext);
        }

        private void EditGame_Click(object sender, RoutedEventArgs e) {
            OnEditGameCmd?.Execute(null);
        }



        // Using a DependencyProperty as the backing store for CanEdit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanEditProperty =
            DependencyProperty.Register("CanEdit", typeof(bool), typeof(GameTopListItem), new PropertyMetadata(false, (d, e) => {
                var view = d as GameTopListItem;
                view.EditButton.IsEnabled = view.CanEdit;
            }));


        // Using a DependencyProperty as the backing store for OnEditGameCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnEditGameCmdProperty =
            DependencyProperty.Register("OnEditGameCmd", typeof(ICommand), typeof(GameTopListItem), new PropertyMetadata(null));




        // Using a DependencyProperty as the backing store for OnViewGameCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnViewGameCmdProperty =
            DependencyProperty.Register("OnViewGameCmd", typeof(ICommand), typeof(GameTopListItem), new PropertyMetadata(null));


        // Using a DependencyProperty as the backing store for Top3Users.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Top3UsersProperty =
            DependencyProperty.Register("Top3Users", typeof(List<User>), typeof(GameTopListItem), new PropertyMetadata(null, (d, e) => {
                var view = d as GameTopListItem;
                view.AvatarList.Users = view.Top3Users;
            }));

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(GameTopListItem), new PropertyMetadata(null, (d, e) => {
                var view = d as GameTopListItem;
                view.DescriptionTextBlock.Text = view.Description;
            }));



        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(GameTopListItem), new PropertyMetadata(null, (d, e) => {
                var view = d as GameTopListItem;
                view.TitleTextBlock.Text = view.Title;
            }));


        // Using a DependencyProperty as the backing store for Cover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoverProperty =
            DependencyProperty.Register("Cover", typeof(ImageSource), typeof(GameTopListItem), new PropertyMetadata(null, (d, e) => {
                var view = d as GameTopListItem;
                view.CoverImage.Source = view.Cover;
            }));


    }
}
