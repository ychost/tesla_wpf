using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace tesla_wpf.Helper {
    /// <summary>
    /// 网络相关辅助方法
    /// <date>2019-1-26</date>
    /// </summary>
    public static class NetworkHelper {
        private static readonly string server = "api.sudoyc.com";

        /// <summary>
        /// 检查网络是否连接正常
        /// </summary>
        /// <param name="message">异常提示</param>
        /// <returns></returns>
        public static bool CheckNetwork(out string message, int timeoutMs = 5000) {
            var ping = new Ping();
            message = string.Empty;
            try {
                var status = ping.Send(server, timeoutMs).Status;
                if (status == IPStatus.Success) {
                    return true;
                } else {
                    message = "网络连接失败";
                }
            } catch (Exception e) {
                message = e.Message;
            }
            return false;
        }
    }

}
