
using System;

namespace tesla_wpf.Model.System.Storage {
    /// <summary>
    /// 云存储临时账号
    /// <date>2019-2-24</date>
    /// </summary>
    public class StorageTmpCredential {
        public string SessionToken { get; set; }
        public string TmpSecretId { get; set; }
        public string TmpSecretKey { get; set; }
        public DateTime ExpiredTime { get; set; }
        public DateTime StartTime { get; set; }
        public string Region { get; set; }
        public string Bucket { get; set; }
        public string AppId { get; set; }
    }
}
