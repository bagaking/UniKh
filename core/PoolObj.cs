using System.Collections.Generic;
using UnityEngine;

namespace UniKh.core {
    public abstract class PoolObj<T> : MonoBehaviour where T : PoolObj<T>, new() {
        public static List<T> Pool = new List<T>();

        public abstract void Initial();

        public abstract void Recycle();
    }
}