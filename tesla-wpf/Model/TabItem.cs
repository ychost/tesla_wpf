using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Model {
    /// <summary>
    /// Tab 切换的项
    /// <date>2019-1-22</date>
    /// </summary>
    public class TabItem : BaseViewModel {
        public string Header { get => GetProperty<string>(); set => SetProperty(value); }
        public IMenu Content { get => GetProperty<IMenu>(); set => SetProperty(value); } 
        /// <summary>
        /// 关联的菜单 Id
        /// </summary>
        public int BindMenuId { get; set; }

    }
}
