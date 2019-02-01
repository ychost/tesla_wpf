using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tesla_wpf.Model;

namespace tesla_wpf.Event {
    /// <summary>
    /// 添加一个 Tab
    /// </summary>
    public class AddTabEvent {
        /// <summary>
        /// 是否马上切换到该 Tab
        /// </summary>
        public bool IsSwitchIt { get; set; } = true;

        /// <summary>
        /// Tab 标题
        /// </summary>
        public string TabName { get; set; }
        /// <summary>
        /// Tab 内容 
        /// </summary>
        public IMenu TabContent { get; set; }
    }
}
