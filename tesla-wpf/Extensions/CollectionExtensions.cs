using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tesla_wpf.Extensions {
    /// <summary>
    /// 一些集合的扩展方法
    /// <date>2019-1-25</date>
    /// </summary>
    public static class CollectionExtensions {
        /// <summary>
        ///  ObservableCollection 支持 Sort
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="comparison"></param>
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison) {
            var sortableList = new List<T>(collection);
            sortableList.Sort(comparison);

            for (int i = 0; i < sortableList.Count; i++) {
                collection.Move(collection.IndexOf(sortableList[i]), i);
            }
        }
    }
}
