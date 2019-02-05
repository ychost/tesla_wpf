using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vera.Wpf.Lib.Mvvm;

namespace tesla_wpf.Route.ViewModel {
    /// <summary>
    /// 单个游戏详情数据
    /// <date>2019-2-1</date>
    /// </summary>
    public class GameTopDetailViewModel : BaseViewModel {
        /// <summary>
        /// 
        /// </summary>
        public string MdText { get => GetProperty<string>(); set => SetProperty(value); }

        public GameTopDetailViewModel() {

        }

        public GameTopDetailViewModel(string text) {
            MdText = text;
        }

        protected override void InitDesignData() {

        }
    }
}
