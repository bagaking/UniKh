using System;
using UniKh.utils;
using UnityEngine;

namespace UniKh.core {
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>, new() {
        public static T Inst {
            get {
                if(!Application.isPlaying) throw new Exception($"The singleton can only be called at runtime");
                if (_inst) return _inst;
                var type = typeof(T);
                Log.Verbose($"create singleton node of type {type} ");
                var inUniKh = type.Namespace != null && type.Namespace.StartsWith("UniKh");
                var go = new GameObject(inUniKh ? $"[S]UniKh/{type.Name}" : $"[S]{type.FullName}");
                return go.AddComponent<T>();
            }
        }

        private static T _inst;

        public Singleton() : base() {
            var type = typeof(T);
            if (_inst && _inst != this) { Log.Error($"The singleton of type is already exist"); }

            _inst = this as T;
        }

        private void Start() {
            DontDestroyOnLoad(this);
        }
    }
}