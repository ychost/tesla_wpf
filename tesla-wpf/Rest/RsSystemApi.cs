using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using tesla_wpf.Model.Game;

namespace tesla_wpf.Rest {
    /// <summary>
    /// 用户，菜单等数据相关
    /// <date>2019-1-25</date>
    /// </summary>
    public interface RsSystemApi {
        [Get("/tesla-system/user/settings/")]
        Task<Rest<RsUserSettings>> FetchUserSettings();

        [Get("/tesla-system/tool/token/valid/")]
        Task<Rest<bool?>> ValidToken([Header("Authorization")]  string authorization);

        [Post("/tesla-system/user/login?app=wpf")]
        Task<Rest<RsUser>> Login([Body] RsUserLogin userLogin);

        [Multipart]
        [Post("/tesla-system/cloud/image/upload")]
        Task<Rest<string>> UploadImage([AliasAs("image")]StreamPart stream);
    }

   
}
