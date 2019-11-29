using System;
using System.Collections.Generic;
using System.Text;

namespace UniKh.extensions {
    public static class DictExtension {
        public static TV TryGet<TK, TV>(this Dictionary<TK, TV> dict, TK key, TV defaultValue) {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }
        
        public static TV ReadCache<TK, TV>(this Dictionary<TK, TV> dict, TK key, Func<TK, TV> cbUpdateCache) {
            return dict.ContainsKey(key) ? dict[key] : (dict[key] = cbUpdateCache(key));
        }

        public static int Inc<TK>(this Dictionary<TK, int> dict, TK key, int incValue) {
            return dict.ContainsKey(key) ? dict[key] += incValue : dict[key] = incValue;
        }

        public static Dictionary<TKey, TValue> ForEachPairs<TKey, TValue>(
            this Dictionary<TKey, TValue> dict,
            Action<TKey, TValue> Func) {
            if (Func == null) return dict;
            var e = dict.GetEnumerator(); // todo: cache ? mem leak ?
            while (e.MoveNext()) {
                if (e.Current.Key != null)
                    Func(e.Current.Key, e.Current.Value);
            }
            e.Dispose();
            return dict;
        }

        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> dict) {
            var mapClone = new Dictionary<TKey, TValue>();
            dict.ForEachPairs((k, v) => mapClone[k] = v);
            return mapClone;
        }
    }
}