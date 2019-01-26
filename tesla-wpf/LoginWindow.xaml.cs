using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using tesla_wpf.Extensions;
using tesla_wpf.Helper;
using tesla_wpf.Model.Setting;
using tesla_wpf.Rest;
using tesla_wpf.Route.View;
using tesla_wpf.Toolkit;
using Vera.Wpf.Lib.Component;
using Vera.Wpf.Lib.Helper;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged {
        private List<LoginHistory> loginHistories;
        public LoginWindow() {
            InitializeComponent();
            DataContext = this;
            SqliteHelper.Exec(db => {
                loginHistories = db.Query<LoginHistory>($"Select * From {nameof(LoginHistory)}");
            });
            autoLogin();
        }

        /// <summary>
        /// 从其他地方跳转而来，那么不需要自动登录
        /// </summary>
        /// <param name="param"></param>
        public LoginWindow(object param) {
            InitializeComponent();
            DataContext = this;
            SqliteHelper.Exec(db => {
                loginHistories = db.Query<LoginHistory>($"Select * From {nameof(LoginHistory)}");
            });
        }

        async void autoLogin() {
            User user = null;
            SqliteHelper.Exec(db => {
                // 有用户登录过
                user = db.Query<User>($"Select * From {nameof(User)}").LastOrDefault();
            });

            if (user != null) {
                try {
                    var rest = await HttpRestService.ForAuthApi<RsSystemApi>().ValidToken(user.Token);
                    if (HttpRestService.ForData(rest, out var message)) {
                        App.User = user;
                        gotoMainWindow();
                    }
                } catch {
                    await Dispatcher.BeginInvoke(new Action(() => {
                        NotifyHelper.ShowWarnMessage("用户身份已失效，请重新登录");
                    }));
                    SqliteHelper.Exec(db => {
                        db.Delete(user);
                    });
                }
            }
        }

        public RsUserLogin UserLogin { get; set; } = new RsUserLogin();
        public ICommand LoginCmd => new MdCommand(loginExec, canLogin);
        /// <summary>
        /// 正在登录中
        /// </summary>
        private bool isLogining;
        public bool IsLogining {
            get => isLogining;
            set {
                if (isLogining != value) {
                    isLogining = value;
                    onPropertyChanged(nameof(IsLogining));
                }
            }
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="obj"></param>
        private async void loginExec(object obj) {
            IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(PasswordBox.SecurePassword);
            // 使用.NET内部算法把IntPtr指向处的字符集合转换成字符串  
            UserLogin.Password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
            if (string.IsNullOrEmpty(UserLogin.Password)) {
                NotifyHelper.ShowErrorMessage("密码不能为空");
                return;
            }
            IsLogining = true;
            await Task.Delay(1000);


            var client = HttpRestService.ForAnonymousApi<RsSystemApi>();
            var rest = await client.Login(UserLogin);
            if (HttpRestService.ForData(rest, out var msg)) {
                var user = ConvertToolkit.ConvertUser(rest.Data);
                // 存入登录信息
                App.User = user;
                gotoMainWindow();
            }
            IsLogining = false;
        }

        private bool canLogin(object arg) {
            return BaseDataErrorInfo.IsValid(UserLogin, out var message);
        }

        /// <summary>
        /// 跳转到主窗体
        /// </summary>
        private void gotoMainWindow() {
            var window = new MainWindow();
            Application.Current.MainWindow = window;
            NotifyHelper.UpdateNotifierWindow();
            Close();
            window.Show();
        }

        /// <summary>
        /// 拖动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Move(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimize_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e) {
            Close();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 随着用户名的改变，如果出现历史用户则显示登录头像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserName_TextChanged(object sender, TextChangedEventArgs e) {
            var tb = e.Source as TextBox;
            var user = (from u in loginHistories where u.Name == tb.Text select u).LastOrDefault();
            if (user != null) {
                Avatar.ImageSource = user.AvatarImageSource;
            } else {
                Avatar.ImageSource = new BitmapImage(AssetsHelper.GetAssets("user.png"));
            }
        }
    }
}
