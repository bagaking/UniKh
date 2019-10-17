using System;
using System.Collections.Generic;
using System.Text;

namespace UniKh.extensions {
    public static class ListExtension {

        static Random random = new Random();

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

        public static bool Append<TTerm>(this List<TTerm> lst, TTerm item) {
            if (lst.Contains(item)) {
                return false;
            }
            lst.Add(item);
            return false;
        }

        public static TTerm Pop<TTerm>(this List<TTerm> lst) {
            var item = lst[lst.Count - 1];
            lst.RemoveAt(lst.Count - 1);
            return item;
        }

        public static List<TTerm> Filter<TTerm>(this List<TTerm> lst, System.Predicate<TTerm> condition) {
            var ret = new List<TTerm>(lst.Count / 4 + 1);

            lst.ForEach(t => {
                if (condition(t)) {
                    ret.Add(t);
                }
            });
            return ret;
        }

        public static T RandomElem<T>(this List<T> list) {
            if (list.Count <= 0) {
                throw new Exception("cannot call RandomElem for a empty list");
            }
            return list[random.Next(list.Count)];
        }


        /** queue and stack */
        /// <summary>
        /// Pop the head item of the seq
        /// </summary>
        /// <param name="item">the out value</param>
        /// <returns>return self for pipeline</returns>
        public static List<T> PopFirst<T>(this List<T> self, out T elem) {
            if (self.Count <= 0) {
                throw new Exception("List pop first elem error: empty list");
            }
            elem = self[0];
            self.RemoveAt(0);
            return self;
        }

        public static T PopFirst<T>(this List<T> self) {
            self.PopFirst(out T ret);
            return ret;
        }

        /// <summary>
        /// Pop the tile item of the seq
        /// </summary>
        /// <param name="item">the out value</param>
        /// <returns>return self for pipeline</returns>
        public static List<T> PopLast<T>(this List<T> self, out T elem) {
            if (self.Count <= 0) {
                throw new Exception("List pop last elem error: empty list");
            }
            elem = self[self.Count - 1];
            self.RemoveAt(self.Count - 1);
            return self;
        }


        public static T PopLast<T>(this List<T> self) {
            self.PopLast(out T elem);
            return elem;
        }

        /// <summary>
        /// push an item at the head of the seq
        /// </summary>
        /// <param name="item">the item</param>
        /// <returns>return self for pipeline</returns>
        public static List<T> PushFirst<T>(this List<T> self, T t) {
            self.Insert(0, t);
            return self;
        }

        /// <summary>
        /// push an item at the tail of the seq
        /// </summary>
        /// <param name="item">the item</param>
        /// <returns>return self for pipeline</returns>
        public static List<T> PushLast<T>(this List<T> self, T t) {
            self.Add(t);
            return self;
        }

        /// <summary>
        /// push an item to the stack(seq)
        /// </summary>
        /// <param name="item">the item</param>
        /// <returns>the item self</returns>
        public static T StackPush<T>(this List<T> self, T t) {
            self.PushLast(t);
            return t;
        }

        /// <summary>
        /// pop an item from the stack(seq)
        /// </summary> 
        /// <returns>the item</returns>
        public static T StackPop<T>(this List<T> self) {
            self.PopLast(out T t);
            return t;
        }

        /// <summary>
        /// push an item to the queue(seq)
        /// </summary>
        /// <param name="item">the item</param>
        /// <returns>the item self</returns>
        public static T QueuePush<T>(this List<T> self, T t) {
            self.PushFirst(t);
            return t;
        }

        /// <summary>
        /// pop an item from the queue(seq)
        /// </summary> 
        /// <returns>the item</returns>
        public static T QueuePop<T>(this List<T> self) {
            self.PopLast(out T t);
            return t;
        }
        
        
        public static List<T> Shuffle<T>(this List<T> self) {
            var lst = new List<T>(self.Count);
            for (int i = 0; i < self.Count; i++) {
                int pos = random.Next(0, lst.Count);
                if (pos != i) {
                    T tmp = lst[pos]; 
                    lst[pos] = self[i]; 
                    lst.Add(tmp);
                }
            }  
            return lst;
        } 

    }
}