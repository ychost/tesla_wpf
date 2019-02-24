using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tesla_wpf.Model.System.Storage {

    /// <summary>
    /// 抽象的云存储器
    /// <date>2019-2-24</date>
    /// </summary>
    public interface ICloudStorage {
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="fileSrc">本地文件路径</param>
        /// <param name="fileKey">上传的图片 key</param>
        /// <returns></returns>
        Task PutImage(string fileSrc, string fileKey);
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="fileKey">图片 key</param>
        /// <param name="localDir">本地文件夹</param>
        /// <param name="localFileName">本地文件名</param>
        /// <returns></returns>
        Task GetImage(string fileKey, string localDir, string localFileName);
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="fileKey">图片 key</param>
        /// <returns></returns>
        Task DelImage(string fileKey);


    }
}
