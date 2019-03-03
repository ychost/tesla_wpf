using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using NLog.Targets;

namespace tesla_wpf.Extensions {
    /// <summary>
    /// Nlog 的 http target
    /// 主要将数据 post 到 logstash 中去
    /// <date>2019-3-1</date>
    /// </summary>
    [Target("LogstashHttp")]
    public class NLogHttpTraget : TargetWithLayout {
        /// <summary>
        /// 写入数据到 Logstash 这里是同步方法
        /// </summary>
        /// <param name="logEvent"></param>
        protected override void Write(LogEventInfo logEvent) {
            var message = Layout.Render(logEvent);
            try {
                using (var web = new WebClient()) {
                    web.Headers["Authorization"] = "Basic dGVzbGE6MTEyMjEx";
                    // 解决 logstash 乱码
                    var resBytes = web.UploadData("http://ks.sudoyc.com:33256", Encoding.UTF8.GetBytes(message));
                    var res = Encoding.UTF8.GetString(resBytes);
                    if (res != "ok") {
                        throw new Exception(res);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine("日志上传 Logstash 失败：" + e.Message);
            }
        }

    }
}
