using System;
using System.Collections.Generic;

namespace UniKh.extensions {
    public static class ArrayExtension {

        static Random random = new Random();

        public static TResult[] Map<T, TResult>(this T[] arr, Converter<T, TResult> converter) {
            var ret = new TResult[arr.Length];
            for (uint i = 0; i < arr.Length; i++) ret[i] = converter(arr[i]);

            return ret;
        }
        
        public static TResult[] Map<T, TResult>(this T[] arr, Func<T, int, TResult> converter) {
            var ret = new TResult[arr.Length];
            for (int i = 0; i < arr.Length; i++) ret[i] = converter(arr[i], i);
            return ret;
        }

        public static T RandomElem<T>(this T[] arr)
        {
            return arr[random.Next(arr.Length)];
        }

        public static TResult Reduce<T, TResult>(this T[] lst, System.Func<TResult, T, TResult> reducer, TResult startVal) {
            var ret = startVal;
            for (var i = 0; i < lst.Length; i++) {
                startVal = reducer(startVal, lst[i]);
            }
            return ret;
        }

        public static void ForEach<T>(this T[] lst, System.Action<T, int, T[]> action) {
            for (var i = 0; i < lst.Length; i++) {
                action(lst[i], i, lst);
            }
        }

        public static List<T> Filter<T>(this T[] arr, Predicate<T> predicate) {
            var ret = new List<T>(arr.Length);

            for (uint i = 0; i < arr.Length; i++) {
                var obj = arr[i];
                if (!predicate(obj)) {
                    continue;
                }
                ret.Add(obj);
            }

            return ret;
        }

        public static T Find<T>(this T[] arr, Predicate<T> predicate) where T : class {
            for (uint i = 0; i < arr.Length; i++) {
                var obj = arr[i];
                if (predicate(obj)) {
                    return obj;
                }
            }

            return null;
        }

    }
}