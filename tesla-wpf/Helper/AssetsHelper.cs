using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace tesla_wpf.Helper {
    /// <summary>
    /// 资源文件管理
    /// <date>2019-1-22</date>
    /// </summary>
    public static class AssetsHelper {
        /// <summary>
        /// 默认的用户头像
        /// </summary>
        public static ImageSource UserImaggeSource = new BitmapImage(GetAssets("user.ico"));

        /// <summary>
        /// 默认的 not found oops!
        /// </summary>
        public static ImageSource OopsImageSource = new BitmapImage(GetAssets("oops.png"));

        /// <summary>
        /// 获取 Assets 文件夹下面的资源
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Uri GetAssets(string fileName) {
            return new Uri("pack://application:,,,/tesla-wpf;component/Assets/" + fileName, UriKind.Absolute);
        }


        /// <summary>
        /// 从网络上面下载图片，转换成 BitMapImage
        /// </summary>
        /// <param name="url">图片的 url 地址</param>
        /// <param name="bufferSize">读取图片开辟的缓存大小，默认 512K</param>
        /// <returns></returns>
        public static ImageSource FetchImage(string url) {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(url, UriKind.Absolute);
            image.EndInit();
            return image;
        }
    }
}
