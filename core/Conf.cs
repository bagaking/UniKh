using System;
using UnityEngine;

namespace UniKh.core {
    public class Conf<T> : ScriptableObject where T : Conf<T> {
        private static T _inst;

        public static T Inst {
            get {
                if (_inst) return _inst;
                var type = typeof(T);
                var results = KhPreferenceStatic.Inst.Configs.Find(c => c.GetType() == type);
                if (results == null) throw new Exception("Cannot find configuration of type " + type);
                _inst = results as T;
                return _inst;
            }
        }
    } 
}