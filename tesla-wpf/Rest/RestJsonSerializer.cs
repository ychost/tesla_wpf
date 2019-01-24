using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serialization;

namespace tesla_wpf.Rest {
    /// <summary>
    /// Rest 请求 Json 序列化，导入 NewtonJson 
    /// <date>2019-1-24</date>
    /// </summary>
    public class RestJsonSerializer : IRestSerializer {
        /// <summary>
        /// 后端 Api 是小写的 Java 风格，这里是 C# 的大写风格，所以要加转换器
        /// </summary>
        JsonSerializerSettings JsonSetting = new JsonSerializerSettings();
        public RestJsonSerializer() {
            JsonSetting.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public string Serialize(object obj) => JsonConvert.SerializeObject(obj);

        public string Serialize(BodyParameter bodyParameter) {
            return JsonConvert.SerializeObject(bodyParameter.Value, JsonSetting);
        }

        public T Deserialize<T>(IRestResponse response) {
            return JsonConvert.DeserializeObject<T>(response.Content, JsonSetting);
        }


        public string Serialize(Parameter parameter) { throw new NotImplementedException(); }

        public string[] SupportedContentTypes { get; } = {
                "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
            };

        public string ContentType { get; set; } = "application/json";

        public RestSharp.DataFormat DataFormat { get; } = RestSharp.DataFormat.Json;

        RestSharp.DataFormat IRestSerializer.DataFormat => throw new NotImplementedException();
    }
}
