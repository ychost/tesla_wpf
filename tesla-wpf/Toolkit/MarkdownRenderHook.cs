using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Markdig.Wpf.Extensions;
using Vera.Wpf.Lib.Helper;
using System.Windows.Controls;
using tesla_wpf.Helper;
using System.Web;

namespace tesla_wpf.Toolkit {
    /// <summary>
    /// Markdown 渲染的钩子
    /// </summary>
    public class MarkdownRenderHook : IWpfRenderHook {

        /// <summary>
        /// 设置图片渲染缓存
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Image RenderImage(string url) {
            return new Image() { Source = AssetsHelper.FetchCloudImage(url) };
        }
    }
}
