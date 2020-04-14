using System;

namespace UniKh.dataStructure {
     
    public class LazyVal<T> where T: class{
        public T val;

        private Func<T> _fnInitialVal = null;

        public LazyVal(Func<T> fnInitialVal) {
            _fnInitialVal = fnInitialVal;
        }

        public T Val {
            get {
                if (val == null) {
                    val = _fnInitialVal();
                }

                return val;
            }
        }

        public static implicit operator T(LazyVal<T> lazyLoader) {
            return lazyLoader.Val;
        }
    }
}