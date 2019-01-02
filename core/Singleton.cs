using System;
using UnityEngine;

namespace UniKh.core {
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>, new() {
        public static T Inst {
            get {
                if (_inst) return _inst;
                var go = GameObject.Find("__SYS__") ?? (new GameObject("__SYS__"));
                return go.AddComponent<T>();
            }
        }

        private static T _inst;

        public Singleton() : base() {
            if (_inst) { throw new Exception($"The singleton {_inst} of type {typeof(T)} is already exist"); }

            _inst = this as T;
        }

        private void Start() {
            DontDestroyOnLoad(this);
        }
    }
}