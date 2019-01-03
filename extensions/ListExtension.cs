using System.Collections.Generic;
using System.Text;

namespace UniKh.extensions {
    public static class ListExtension {
        public static TPrev Reduce<TPrev, TTerm>(
            this IEnumerable<TTerm> lst,
            System.Func<TPrev, TTerm, TPrev> reducer,
            TPrev initValue) {
            foreach (var term in lst) initValue = reducer(initValue, term);
            return initValue;
        }


        private static StringBuilder sbCombind = new StringBuilder();

        public static string ToItemsString<TTerm>(this IEnumerable<TTerm> lst, string prefix = "") => lst.Reduce(
                (prev, term) => prev.Append(' ').Append(term),
                sbCombind.Clear().Append(prefix))
            .ToString();

        public static List<TResult> Map<TTerm, TResult>(this List<TTerm> lst, System.Func<TTerm, TResult> mapper) {
            var ret = new List<TResult>(lst.Count);
            for (var i = 0; i < lst.Count; i++) {
                ret.Add(mapper(lst[i]));
            }

            return ret;
        }

        public static TTerm Last<TTerm>(this List<TTerm> lst) { return lst[lst.Count - 1]; }

        public static TTerm First<TTerm>(this List<TTerm> lst) { return lst[0]; }


        public static void ForEach<TTerm>(this IEnumerable<TTerm> lst, System.Action<TTerm> action) {
            foreach (var t in lst) {
                action(t);
            }
        }

        public static void ForEach<TTerm>(this List<TTerm> lst, System.Action<TTerm, int> action) {
            for (var i = 0; i < lst.Count; i++) {
                action(lst[i], i);
            }
        }
        
        public static void ForEach<TTerm>(this TTerm[] lst, System.Action<TTerm, int> action) {
            for (var i = 0; i < lst.Length; i++) {
                action(lst[i], i);
            }
        }

        public static void Push<TTerm>(this List<TTerm> lst, TTerm item) { lst.Add(item); }

        public static TTerm Pop<TTerm>(this List<TTerm> lst) {
            var item = lst[lst.Count - 1];
            lst.RemoveAt(lst.Count - 1);
            return item;
        }
    }
}