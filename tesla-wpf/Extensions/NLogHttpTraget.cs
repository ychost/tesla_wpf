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
        /// 
        /// </summary>
        /// <param name="logEvent"></param>
        protected override void Write(LogEventInfo logEvent) {
            var message = Layout.Render(logEvent);
            try {
                // 解决 logstash 乱码
                var gb2312 = Encoding.GetEncoding("gb2312");
                var data = Encoding.Convert(gb2312, Encoding.UTF8, gb2312.GetBytes(message));
                message = gb2312.GetString(data);
                using (var web = new WebClient()) {
                    web.Headers["Authorization"] = "Basic dGVzbGE6MTEyMjEx";
                    var res = web.UploadString("http://ks.sudoyc.com:33256", message);
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
