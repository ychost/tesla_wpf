using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using Refit;
using tesla_wpf.Rest;
using Vera.Wpf.Lib.Helper;

namespace tesla_wpf.Extensions {
    /// <summary>
    /// Refit 扩展
    /// <date>2019-1-25</date>
    /// </summary>
    public static class HttpRestService {

        static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings() {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        static readonly string apiUrl = "http://reg.sudoyc.com:1002";
        // 每次请求的最长时间
        static readonly TimeSpan RequestTimeout = TimeSpan.FromMinutes(5);

        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 需要注入 Token 认证的 api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ForAuthApi<T>() {
            return RestService.For<T>(
              new HttpClient(new AuthenticatedHttpClientHandler()) {
                  BaseAddress = new Uri(apiUrl),
                  Timeout = RequestTimeout
              },
              new RefitSettings() {
                  JsonSerializerSettings = jsonSettings
              });
        }

        /// <summary>
        /// 支持匿名请求的 Api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ForAnonymousApi<T>() {
            return RestService.For<T>(
                new HttpClient(new AnonymousRequestHttpClientHandler()) {
                    BaseAddress = new Uri(apiUrl),
                    Timeout = RequestTimeout
                },
                new RefitSettings() {
                    JsonSerializerSettings = jsonSettings
                });
        }

        ///<summary>
        /// 这里加了错误的消息提示等等全局处理
        ///</summary>
        /// <example>
        /// if(ForData<Api>(out var data)){
        ///  do something
        /// }
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool ForData<T>(Rest<T> rest, out T data) {
            if (rest.Code == HttpRestcode.Success) {
                data = rest.Data;
                return true;
            }
            data = default(T);
            return false;
        }

        /// <summary>
        /// 给一些非 200 的错误返回一些提示
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> HandleHttpIO(HttpRequestMessage request, HttpResponseMessage response) {
            Logger.Trace("request url: " + request.RequestUri.ToString());
            var res = await response.Content.ReadAsStringAsync();
            Logger.Trace("response data: " + res);
            if (response.StatusCode != HttpStatusCode.OK) {
                Logger.Error("响应错误", JsonConvert.SerializeObject(response.Content));
            }
            return response;
        }
    }

    /// <summary>
    /// 发出的 Post  请求支持 Authorization 
    /// <date>2019-1-25</date>
    /// </summary>
    class AuthenticatedHttpClientHandler : HttpClientHandler {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            // See if the request has an authorize header
            if (request.Headers.Authorization == null) {
                var token = App.GetHttpToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await base.SendAsync(request, cancellationToken);
            var message = await HttpRestService.HandleHttpIO(request, response);
            return message;
        }
    }

    /// <summary>
    /// 与 http 服务端约定的匿名请求
    /// </summary>
    class AnonymousRequestHttpClientHandler : HttpClientHandler {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            // 设置匿名请求的标记
            request.RequestUri = new Uri(request.RequestUri.ToString() + "&&_allow_anonymous=true");
            var response = await base.SendAsync(request, cancellationToken);
            var message = await HttpRestService.HandleHttpIO(request, response);
            return message;
        }
    }

    /// <summary>
    /// Http 匿名请求
    /// </summary>
    public class AnonymousRequestAttribute : Attribute {
    }
}
