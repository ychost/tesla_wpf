using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using tesla_wpf.Model.Game;
using tesla_wpf.Model.GameTop;

namespace tesla_wpf.Rest {
    /// <summary>
    /// 游戏排行, 后台 Api
    /// <date></date>
    /// </summary>
    public interface RsGameTopApi {
        /// <summary>
        /// 上传游戏封面图片
        /// </summary>
        /// <param name="game"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        [Multipart]
        [Post("/tesla-cassette/rank/game/add?name={name}&&description={description}&&remark={remark}&&officialWebsite={officialWebsite}")]
        Task<Rest<bool>> UploadCover(string name, string description, string remark, string officialWebsite, [AliasAs("cover")] StreamPart stream);

        [Get("/tesla-cassette/rank/game/list")]
        Task<Rest<List<Game>>> FetchGames();
    }
}
