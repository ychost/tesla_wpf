

using System;

namespace tesla_wpf.Rest {
    /// <summary>
    /// 引用类型的 Rest
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Rest<T> {
        public T Data { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public object Ext { get; set; }
    }

    /// <summary>
    /// Http 请求返回的一些错误码
    /// </summary>
    public static class HttpRestcode {
        // 成功
        public static int Success = 0;
        // 未知错误
        public static int UnknownError = 1000;
        // 请求参数错误
        public static int ReqParamError = 1001;
        // 请求结果为空
        public static int ResIsNull = 1002;
        // 处理过程中发生了异常
        public static int Exeception = 1003;
        // 权限禁止
        public static int PermissionForbid = 1004;
        // 操作失败
        public static int Failed = 1005;
        // 某些批量操作失败了一部分
        public static int SomeFailed = 1006;
    }
}
