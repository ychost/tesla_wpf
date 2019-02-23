using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Hardcodet.Wpf.TaskbarNotification;
using NLog;
using tesla_wpf.Extensions;
using tesla_wpf.Helper;
using tesla_wpf.Model.Setting;
using tesla_wpf.Rest;
using tesla_wpf.Route.View;
using tesla_wpf.Toolkit;
using Vera.Wpf.Lib.Helper;
using Vera.Wpf.Lib.Mvvm;
using YEvent;

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
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 登录命令
        /// </summary>
        public ICommand LoginCmd => new MdCommand(loginExec, canLogin);
        private ImageSource avatar;
        public ImageSource Avatar {
            get => avatar;
            set {
                if (avatar != value) {
                    avatar = value;
                    onPropertyChanged(nameof(Avatar));
                }
            }
        }


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
            Avatar = AssetsHelper.UserImaggeSource;
            DataContext = this;
            SqliteHelper.Exec(db => {
                loginHistories = db.Query<LoginHistory>($"Select * From {nameof(LoginHistory)}");
            });
            // 网络初始化成功后自动登录
            initNetwork(autoLogin);
            ShowInTaskbar = false;
        }


        /// <summary>
        /// 从其他地方跳转而来，那么不需要自动登录
        /// </summary>
        /// <param name="param"></param>
        public LoginWindow(object param) {
            InitializeComponent();
            Avatar = AssetsHelper.UserImaggeSource;
            DataContext = this;
            SqliteHelper.Exec(db => {
                loginHistories = db.Query<LoginHistory>($"Select * From {nameof(LoginHistory)}");
            });
            initNetwork();
            ShowInTaskbar = false;
        }

        /// <summary>
        /// 检查网络连接
        /// </summary>
        void initNetwork(Action action = null) {
            initTaskbarIcon();
            NetworkMessage = string.Empty;
            NetworkIsOk = true;
            action?.Invoke();
            //checkNetworkTimer = TimeHelper.SetInterval(30000, () => {
            //    if (NetworkHelper.CheckNetwork(out var message)) {
            //        NetworkIsOk = true;
            //        TimeHelper.ClearTimeout(checkNetworkTimer);
            //        networkRetryTimes = 0;
            //        NetworkMessage = string.Empty;
            //        action?.Invoke();
            //    } else {
            //        networkRetryTimes += 1;
            //        NetworkMessage = $"正在尝试第 {networkRetryTimes} 次重连...";
            //    }
            //}, true);
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
                UserLogin.UserName = user.Name;
                PasswordBox.Password = "xxxxxxxx";
                try {
                    var rest = await HttpRestService.ForAuthApi<RsSystemApi>().ValidToken(user.Token);
                    if (HttpRestService.ForData(rest, out var message)) {
                        gotoMainWindow(user);
                    }
                } catch (Exception e) {
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
            IsLogining = true;
            IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(PasswordBox.SecurePassword);
            // 使用.NET内部算法把IntPtr指向处的字符集合转换成字符串  
            UserLogin.Password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
            if (string.IsNullOrEmpty(UserLogin.Password)) {
                NotifyHelper.ShowErrorMessage("密码不能为空");
                IsLogining = false;
                return;
            }
            var client = HttpRestService.ForAnonymousApi<RsSystemApi>();
            try {
                var rest = await client.Login(UserLogin);
                if (HttpRestService.ForData(rest, out var rsUser)) {
                    var user = ConvertToolkit.ConvertUser(rsUser);
                    gotoMainWindow(user);
                } else {
                    NotifyHelper.ShowErrorMessage(rest.Message);
                }
            } catch (Exception e) {
                logger.Error<Exception>("登录异常", e);
                NotifyHelper.ShowErrorMessage("网络错误");
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
            App.Store = YEventStore.create();
            App.User = user;
            // 关闭网络检查器
            // 这里不关后面可能还会运行，导致内存泄漏
            checkNetworkTimer?.Close();
            var window = new MainWindow();
            Application.Current.MainWindow = window;
            NotifyHelper.UpdateNotifierWindow();
            exit();
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
            exit();
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
                Avatar = user.AvatarImageSource;
            } else {
                Avatar = AssetsHelper.UserImaggeSource;
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
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuExit_Click(object sender, EventArgs e) {
            exit();
        }
        /// <summary>
        /// 
        /// </summary>
        private void exit() {
            taskbar.Dispose();
            Close();
            GC.Collect();
            GC.WaitForFullGCApproach();
        }
        /// <summary>
        /// 初始化右下角的托盘显示
        /// </summary>
        TaskbarIcon taskbar;
        private void initTaskbarIcon() {
            taskbar = new TaskbarIcon();
            taskbar.Icon = new Icon(Application.GetResourceStream(AssetsHelper.GetAssets("fly.ico")).Stream);
            taskbar.ContextMenu = (ContextMenu)FindResource("SysTrayMenu");
            taskbar.DoubleClickCommand = new MdCommand(maximazeExec);
        }
        /// <summary>
        /// 双击最大化
        /// </summary>
        /// <param name="obj"></param>
        private void maximazeExec(object obj) {
            WindowState = WindowState.Normal;
            Topmost = false;
            Topmost = true;
            Topmost = false;
        }

        private void Setting_Click(object sender, RoutedEventArgs e) {
            taskbar.ShowBalloonTip("提示", "设置功能正在开发中", BalloonIcon.Info);
        }
    }
}
