using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// <date>2019-1-20</date>
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged {
        /// <summary>
        /// 登录的历史用户
        /// </summary>
        private List<LoginHistory> loginHistories;
        /// <summary>
        /// 登录框绑定数据模型
        /// </summary>
        public RsUserLogin UserLogin { get; set; } = new RsUserLogin();
        /// <summary>
        /// 登录命令
        /// </summary>
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

        #region 网络
        private Timer checkNetworkTimer;
        private bool networkIsOk;
        public bool NetworkIsOk {
            get => networkIsOk;
            set {
                if (networkIsOk != value) {
                    networkIsOk = value;
                    onPropertyChanged(nameof(NetworkIsOk));
                }
            }
        }
        private int networkRetryTimes = 0;
        private string networkMessage = "正在检查网络...";
        public string NetworkMessage {
            get => networkMessage;
            set {
                if (networkMessage != value) {
                    networkMessage = value;
                    onPropertyChanged(nameof(NetworkMessage));
                }
            }
        }
        #endregion

        public LoginWindow() {
            InitializeComponent();
            DataContext = this;
            SqliteHelper.Exec(db => {
                loginHistories = db.Query<LoginHistory>($"Select * From {nameof(LoginHistory)}");
            });
            // 网络初始化成功后自动登录
            initNetwork(autoLogin);
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
            initNetwork();
        }

        /// <summary>
        /// 检查网络连接
        /// </summary>
        void initNetwork(Action action = null) {
            checkNetworkTimer = TimeHelper.SetInterval(30000, () => {
                if (NetworkHelper.CheckNetwork(out var message)) {
                    NetworkIsOk = true;
                    TimeHelper.ClearTimeout(checkNetworkTimer);
                    networkRetryTimes = 0;
                    NetworkMessage = string.Empty;
                    action?.Invoke();
                } else {
                    networkRetryTimes += 1;
                    NetworkMessage = $"正在尝试第 {networkRetryTimes} 次重连...";
                }
            }, true);
        }

        /// <summary>
        /// 自动登录逻辑
        /// </summary>
        async void autoLogin() {
            IsLogining = true;
            User user = null;
            SqliteHelper.Exec(db => {
                // 有用户登录过
                user = db.Query<User>($"Select * From {nameof(User)}").LastOrDefault();
            });
            if (user != null) {
                try {
                    var rest = await HttpRestService.ForAuthApi<RsSystemApi>().ValidToken(user.Token);
                    if (HttpRestService.ForData(rest, out var message)) {
                        gotoMainWindow(user);
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
            IsLogining = false;
        }

        /// <summary>
        /// 登录逻辑
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
                gotoMainWindow(user);
            }
            IsLogining = false;
        }

        /// <summary>
        /// 校验登录输入
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canLogin(object arg) {
            return BaseDataErrorInfo.IsValid(UserLogin, out var message);
        }

        /// <summary>
        /// 跳转到主窗体
        /// </summary>
        private void gotoMainWindow(User user) {
            App.User = user;
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
                Avatar.ImageSource = AssetsHelper.UserImaggeSource;
            }
        }

        /// <summary>
        /// 移动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Move(object sender, MouseEventArgs e) {
            this.DragMove();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Enter 按键登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Detected_KeyDown(object sender, KeyEventArgs e) {
            if (networkIsOk && !isLogining) {
                if (e.Key == Key.Enter) {
                    if (canLogin(null)) {
                        loginExec(null);
                    }
                }
            }
        }
    }
}
