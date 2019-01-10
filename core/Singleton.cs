using System;
using UniKh.utils;
using UnityEngine;

namespace UniKh.core {
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>, new() {
        public static T Inst { get; private set; }

        public static T LazyInst {
            get {
                if (Inst) return Inst;
                var type = typeof(T);
                
                var inUniKh = type.Namespace != null && type.Namespace.StartsWith("UniKh");
                var go = new GameObject(inUniKh ? $"[S]UniKh/{type.Name}" : $"[S]{type.FullName}");
                return _SetInst(go.AddComponent<T>());
            }
        }

        private static T _SetInst(T mono) {
            Inst = mono;
            DontDestroyOnLoad(mono.gameObject);
            Log.Info($"create singleton node of type {typeof(T)} \n{mono}");
            return mono;
        }

        protected virtual void Awake() {
            var type = typeof(T);
            if (Inst && Inst != this) {
                Log.Info(
                    $"The singleton of type {type} is already exist :{Inst}. \nThis node {name} will be destroy immediately");
                if (Inst.gameObject == gameObject) { // same gameobject
                    DestroyImmediate(this);
                } else {
                    DestroyImmediate(gameObject);
                }
                return;
            }

            if (Inst) return;
            _SetInst(this as T);
        }

        protected virtual void OnDestroy() {
            if (!Inst || Inst != this) return;
            Log.Verbose($"Singleton destroyed {Inst} of type {Inst.GetType()}");
            Inst = null;
        }
    }
}