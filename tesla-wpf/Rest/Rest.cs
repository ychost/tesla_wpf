

namespace tesla_wpf.Rest {
    /// <summary>
    /// Rest模型
    /// </summary>
    public class Rest<T> {
        public T Data { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public object Ext { get; set; }
    }
}
