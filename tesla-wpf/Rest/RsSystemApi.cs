using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using tesla_wpf.Model.Game;
using tesla_wpf.Model.System;
using tesla_wpf.Model.System.Storage;

namespace tesla_wpf.Rest {
    /// <summary>
    /// 用户，菜单等数据相关
    /// <date>2019-1-25</date>
    /// </summary>
    public interface RsSystemApi {
        /// <summary>
        /// 获取用户配置
        /// </summary>
        /// <returns></returns>
        [Get("/tesla-system/user/settings/")]
        Task<Rest<RsUserSettings>> FetchUserSettings();

        /// <summary>
        /// 校验 Token
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        [Get("/tesla-system/tool/token/valid/")]
        Task<Rest<bool?>> ValidToken([Header("Authorization")]  string authorization);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [Post("/tesla-system/user/login?app=wpf")]
        Task<Rest<RsUser>> Login([Body] RsUserLogin userLogin);

        /// <summary>
        /// 上传图片到云存储，通过后台中专
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [Multipart]
        [Post("/tesla-system/cloud/image/upload")]
        Task<Rest<string>> UploadImage([AliasAs("image")]StreamPart stream);

        /// <summary>
        /// 获取云存临时储账号（下载图片权限）
        /// </summary>
        /// <returns></returns>
        [Get("/tesla-system/cloud/credential/getImage")]
        Task<Rest<StorageTmpCredential>> FetchStorageGetImageCred();

        /// <summary>
        /// 获取云临时存储账号（上传图片权限）
        /// </summary>
        /// <returns></returns>
        [Get("/tesla-system/cloud/credential/putImage")]
        Task<Rest<StorageTmpCredential>> FetchStoragePutImageCred();

        /// <summary>
        /// 获取云临时存储账号（删除图片权限）
        /// </summary>
        /// <param name="fileKey">云文件名</param>
        /// <returns></returns>
        [Get("/tesla-system/cloud/credential/delImage")]
        Task<Rest<StorageTmpCredential>> FetchStorageDelImageCred(string fileKey);
    }


}
