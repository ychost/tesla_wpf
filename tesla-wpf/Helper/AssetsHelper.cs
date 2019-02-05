using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Vera.Wpf.Lib.Helper;

namespace tesla_wpf.Helper {
    /// <summary>
    /// 资源文件管理
    /// <date>2019-1-22</date>
    /// </summary>
    public static class AssetsHelper {
        /// <summary>
        /// 默认的用户头像
        /// </summary>
        public static ImageSource UserImaggeSource = new BitmapImage(GetAssets("user.png"));

        /// <summary>
        /// 默认的 not found oops!
        /// </summary>
        public static ImageSource OopsImageSource = new BitmapImage(GetAssets("oops.png"));
        public static ImageSource MountainImageSource = new BitmapImage(GetAssets("mountains.jpg"));

        /// <summary>
        /// 获取 Assets 文件夹下面的资源
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Uri GetAssets(string fileName) {
            return new Uri("pack://application:,,,/tesla-wpf;component/Assets/" + fileName, UriKind.Absolute);
        }

        /// <summary>
        /// 获取相对路径文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Uri GetRelativeUri(string fileName) {
            return new Uri(Path.Combine(Directory.GetCurrentDirectory(), fileName));
        }

        /// <summary>
        /// 从网络上面下载图片，转换成 BitMapImage
        /// </summary>
        /// <param name="url">图片的 url 地址</param>
        /// <param name="bufferSize">读取图片开辟的缓存大小，默认 512K</param>
        /// <returns></returns>
        public static ImageSource FetchImage(string url) {
            return LoadImage(new Uri(url, UriKind.Absolute));
        }

        /// <summary>
        /// 缓存有图片则从缓存加载
        /// 否则 加载云图片,比如 腾讯云 Cos,然后再缓存下来
        /// </summary>
        /// <param name="cloudKey">Cos 里面的 key</param>
        /// <returns></returns>
        public static ImageSource FetchCloudImageAsync(string cloudKey) {
            var url = "https://tesla-1252572735.cos.ap-chengdu.myqcloud.com" + cloudKey;
            var stream = FileCacheHelper.Hit(url);
            return LoadImageStream(stream);
        }

        /// <summary>
        /// 从流中生产图片
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static ImageSource LoadImageStream(Stream stream) {
            using (stream) {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }


        /// <summary>
        /// 从本地加载缓存文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static Uri LoadLocalCache(string key) {
            //var path = 
            return null;
        }

        /// <summary>
        /// 加载本地头像
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ImageSource LoadLocalAvatar(string name) {
            var path = Path.Combine(Environment.CurrentDirectory, @"avatars\" + name);
            return LoadImage(new Uri(path));
        }

        /// <summary>
        /// 普通的通过 URI 转图片，支持本地文件，网络图片等等给
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static ImageSource LoadImage(Uri uri) {
            var image = new BitmapImage();
            image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);
            image.BeginInit();
            image.UriSource = uri;
            //update 2019-1-27
            //这样就可以删除 UriSource 文件
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            return image;
        }

        /// <summary>
        /// 下载头像文件 
        /// </summary>
        /// <param name="url">头像连接</param>
        /// <param name="name">头像名字</param>
        /// <returns>图片名字（带扩展名）</returns>
        public static string DownloadAvatar(string url, string name) {
            var extension = Path.GetExtension(url);
            using (var client = new WebClient()) {
                // 保证路径存在
                var folder = Path.Combine(Environment.CurrentDirectory, @".\avatars\");
                if (!Directory.Exists(folder)) {
                    Directory.CreateDirectory(folder);
                }
                // 如果图片存在则覆盖
                var savePath = Path.Combine(folder, name + extension);
                if (File.Exists(savePath)) {
                    File.Delete(savePath);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                client.DownloadFile(new Uri(url), savePath);
                return name + extension;
            }
        }




    }
}
