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


        private static readonly StringBuilder SbCombine = new StringBuilder();

        public static string ToItemsString<TTerm>(this IEnumerable<TTerm> lst, string prefix = "", int maxCount = 0) {
            SbCombine.Clear().Append(prefix);
            if (maxCount <= 0) {
                foreach (var term in lst) {
                    SbCombine.Append(' ').Append(term);
                }
            } else {
                foreach (var term in lst) {
                    if (0 == maxCount--) break;
                    SbCombine.Append(' ').Append(term);
                }
            }

            return SbCombine.ToString();
        }


        public static List<TResult> Map<TTerm, TResult>(this List<TTerm> lst, System.Func<TTerm, TResult> mapper) {
            var ret = new List<TResult>(lst.Count);
            for (var i = 0; i < lst.Count; i++) {
                ret.Add(mapper(lst[i]));
            }
            return ret;
        }
        
        public static List<TResult> Map<TTerm, TResult>(this List<TTerm> lst, System.Func<TTerm, int, TResult> mapper) {
            var ret = new List<TResult>(lst.Count);
            for (var i = 0; i < lst.Count; i++) {
                ret.Add(mapper(lst[i], i));
            }
            return ret;
        }

        public static TTerm Last<TTerm>(this List<TTerm> lst) {
            return lst[lst.Count - 1];
        }

        public static TTerm First<TTerm>(this List<TTerm> lst) {
            return lst[0];
        }


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

        public static void Push<TTerm>(this List<TTerm> lst, TTerm item) {
            lst.Add(item);
        }

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

            lst.ForEach(
                t => {
                    if (condition(t)) {
                        ret.Add(t);
                    }
                }
            );
            return ret;
        }

        public static List<TTerm> ChangeLength<TTerm>(this List<TTerm> lst, int length) {
            if (length < 0) throw new ArgumentException("the new length must be >= 0.");

            if (lst.Count < length) {
                lst.Capacity = length;
                while (lst.Count < length) {
                    lst.Add(default);
                }
            }

            while (lst.Count > length) lst.RemoveRange(length, lst.Count - length);
            return lst;
        }

        public static List<TTerm> InPlaceFilter<TTerm>(this List<TTerm> lst, System.Predicate<TTerm> condition) {
            var shrinkOffset = 0;
            for (var i = 0; i < lst.Count; i++) {
                var item = lst[i];
                if (!condition(lst[i])) {
                    shrinkOffset++;
                    continue;
                }

                if (condition(item) && shrinkOffset > 0) {
                    lst[i - shrinkOffset] = item;
                }
            }

            lst.ChangeLength(lst.Count - shrinkOffset);
            return lst;
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


        /// <summary>
        /// Shuffle the list to random in place.
        /// using Fisher-Yates Shuffle Algorithm
        /// </summary>
        public static List<T> ShuffleInPlace<T>(this List<T> self) {
            var i = self.Count;
            while (i > 1) {
                var ind = random.Next(0, i);
                if (--i == ind) continue;
                var temp = self[ind];
                self[ind] = self[i];
                self[i] = temp;
            }

            return self;
        }

        /// <summary>
        /// clone a new list and shuffle it.
        /// using Inside-Out Shuffle Algorithm
        /// </summary>
        public static List<T> Shuffle<T>(this List<T> self) {
            var count = self.Count;
            var ret = new List<T>(count) { self[0] };
            
            for (var i = 1; i < count; i ++) {
                ret.Append(self[i]);
                var ind = random.Next(0, i + 1);
                if(ind == i) continue;
                ret[i] = ret[ind];
                ret[ind] = self[i];
            }

            return ret;
        }
    }
}