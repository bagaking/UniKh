using System.Collections.Generic;
using UniKh.utils;

namespace UniKh.res {
    
    public class CacheLru<T> where T : class {
        public Dictionary<string, T> Cache1 { get; private set; }
        public Dictionary<string, T> Cache2 { get; private set; }
   
        public int PageSize { get; private set; }

        public CacheLru(int pageSize = 64) {
            Cache1 = new Dictionary<string, T>(PageSize = pageSize); 
        }

        public T Get(string key) {
            if (Cache1 != null && Cache1.ContainsKey(key)) {
                return Cache1[key];
            }

            if (Cache2 != null && Cache2.ContainsKey(key)) {
                return Cache2[key];
            }

            return null;
        }

        public TVal Set<TVal>(string key, TVal obj) where TVal : T {
            if (Cache1 == null) {
                Cache1 = new Dictionary<string, T>(PageSize);
            }

            if (!Cache1.ContainsKey(key) && Cache1.Count >= PageSize) { // todo: 如果是动态的 monoBehavior 退出缓存时要手动删除一下
                var temp = Cache2 ?? new Dictionary<string, T>(PageSize);
                Cache2 = Cache1;
                Cache1 = temp;
                
                Cache1.Clear();
            }

            Cache1[key] = obj;
            return obj;
        }


        public override string ToString() {
            var builder = SGen.New;
            builder.AppendLine().Append("cache 1 :");
            if (Cache1 != null)
                foreach (var key in Cache1.Keys) {
                    builder.Append(key).Append(' ');
                }

            builder.AppendLine().Append("cache 2 :");
            if (Cache2 != null)
                foreach (var key in Cache2.Keys) {
                    builder.Append(key).Append(' ');
                }

            return builder.End;
        }
    }
}