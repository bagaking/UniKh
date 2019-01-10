using System;
using UniKh.utils;
using UnityEngine;

namespace UniKh.core {
    public class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>, new() {
        public static T Inst { get; private set; }

        protected virtual void Awake() {
            var type = typeof(T);
            if (Inst && Inst != this) throw new Exception("The scene singleton of type {type} is already exist. Some release error may not exist.");
            Inst = this as T;
            Log.Info($"scene singleton set {Inst}\nof type {Inst.GetType()}");
        }

        protected virtual void OnDestroy() {
            if (!Inst || Inst != this) return;
            Log.Info($"scene singleton destroyed {Inst}\nof type {Inst.GetType()}");
            Inst = null;
        }
    }
}