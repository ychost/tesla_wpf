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
        Task<Rest<bool?>> UploadCover(string name, string description, string remark, string officialWebsite, [AliasAs("cover")] StreamPart stream);

        [Get("/tesla-cassette/rank/game/list")]
        Task<Rest<List<Game>>> FetchGames();

        /// <summary>
        /// 更新游戏内容
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        [Post("/tesla-cassette/rank/game/info/update")]
        Task<Rest<bool?>> UpdateGameInfo(Game game);

        /// <summary>
        /// 获取排行简介列表
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        [Get("/tesla-cassette/rank/type/list")]
        Task<Rest<List<RankType>>> FetchRankTypes(string gameName);

        /// <summary>
        /// 获取某项具体的排行项目下面的排行数据
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="rankTypeName"></param>
        /// <returns></returns>
        [Get("/tesla-cassette/rank/info/list")]
        Task<Rest<List<RankInfo>>> FetchRankInfos(string gameName, string rankTypeName);

        /// <summary>
        /// 添加游戏排行的一条记录
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        [Post("/tesla-cassette/rank/info/add")]
        Task<Rest<bool?>> AddRankInfo(RankInfoAdd rank);

        /// <summary>
        /// 添加一个排行
        /// </summary>
        /// <param name="rankType"></param>
        /// <returns></returns>
        [Post("/tesla-cassette/rank/type/add")]
        Task<Rest<bool?>> AddRankType(RankType rankType);
    }
}
