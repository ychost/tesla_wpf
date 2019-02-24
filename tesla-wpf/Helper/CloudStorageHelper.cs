using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using tesla_wpf.Model.System.Storage;
using tesla_wpf.Toolkit;
using Vera.Wpf.Lib.Helper;

namespace tesla_wpf.Helper {
    /// <summary>
    /// 云存储工具
    /// <date>2019-2-24</date>
    /// </summary>
    public static class CloudStorageHelper {
        private static readonly ICloudStorage storage;
        static CloudStorageHelper() {
            storage = new TecentStorage();
        }
        /// <summary>
        /// 获取云存储助手
        /// </summary>
        /// <returns></returns>
        public static ICloudStorage GetCloudStorage() {
            return storage;
        }

        /// <summary>
        /// 构建云图片 key
        /// </summary>
        /// <param name="fileSrc"></param>
        /// <returns></returns>
        public static string BuildImageFileKey(string fileSrc) {
            var str = fileSrc + TimeHelper.GetTimestampeMs(DateTime.Now);
            StringBuilder builder = new StringBuilder();
            builder.Append("/images/");
            using (var sha1 = new SHA1Managed()) {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(str));
                builder.Append(BitConverter.ToString(hash).Replace("-", "").ToLower());
                if (Path.HasExtension(fileSrc)) builder.Append(Path.GetExtension(fileSrc));
            }
            return builder.ToString();
        }
    }
}
