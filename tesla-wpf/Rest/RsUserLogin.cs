using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vera.Wpf.Lib.Model;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Rest {
    /// <summary>
    /// 用户登录
    /// <date>2019-1-25</date>
    /// </summary>
    public class RsUserLogin : BaseViewModel {
        /// <summary>
        /// 登录名
        /// </summary>
        [RegularExpression(@"^[a-zA-Z0-9]{5,10}$", ErrorMessage = "用户名有误，只能是 5-10 位的英文或数字组成")]
        public string UserName { get => GetProperty<string>(); set => SetProperty(value); }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get => GetProperty<string>(); set => SetProperty(value); }
    }
}
