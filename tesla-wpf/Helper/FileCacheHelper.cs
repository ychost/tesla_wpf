using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace tesla_wpf.Helper {
    /// <summary>
    /// 文件缓存到本地
    /// <date>2019-1-31</date>
    /// <url>https://github.com/floydpink/CachedImage/blob/master/source/FileCache.cs</url>
    /// </summary>
    public static class FileCacheHelper {

        static FileCacheHelper() {
            // default cache directory - can be changed if needed from App.xaml
            AppCacheDirectory = string.Format("{0}\\{1}\\Cache\\",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Process.GetCurrentProcess().ProcessName);
        }

        /// <summary>
        ///     Gets or sets the path to the folder that stores the cache file. Only works when AppCacheMode is
        ///     CacheMode.Dedicated.
        /// </summary>
        public static string AppCacheDirectory { get; set; }

        /// <summary>
        /// 1. 将url编码 -> 文件名
        /// 2. 本地存在  -> 返回
        ///    本地不存在 -> 从网络下载到本地
        /// 3. 返回文件流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream Hit(string url) {
            if (!Directory.Exists(AppCacheDirectory)) {
                Directory.CreateDirectory(AppCacheDirectory);
            }
            var uri = new Uri(url);
            var fileNameBuilder = new StringBuilder();
            using (var sha1 = new SHA1Managed()) {
                var canonicalUrl = uri.ToString();
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(canonicalUrl));
                fileNameBuilder.Append(BitConverter.ToString(hash).Replace("-", "").ToLower());
                if (Path.HasExtension(canonicalUrl))
                    fileNameBuilder.Append(Path.GetExtension(canonicalUrl));
            }
            var name = fileNameBuilder.ToString();
            var localFile = string.Format("{0}\\{1}", AppCacheDirectory, name);

            if (File.Exists(localFile)) {
                return new FileStream(localFile, FileMode.Open, FileAccess.Read);
            }
            try {
                using (var client = new WebClient()) {
                    client.DownloadFile(url, localFile);
                    return new FileStream(localFile, FileMode.Open, FileAccess.Read);
                }
            } catch (Exception e) {
                return null;
            }
        }
    }
}
