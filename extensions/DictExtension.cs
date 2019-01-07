using System.Collections.Generic;
using System.Text;

namespace UniKh.extensions {
    public static class DictExtension {

        public static TV TryGet<TK, TV>(this Dictionary<TK, TV> dict, TK key, TV defaultValue) {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        public static int Inc<TK>(this Dictionary<TK, int> dict, TK key, int incValue) {
            return dict.ContainsKey(key) ? dict[key] += incValue : dict[key] = incValue;
        }
    }
}