using System;
using model;
using UnityEngine;

namespace UniKh.core {
    
    public class Conf<T> : ScriptableObject where T : Conf<T>{
        private static T _inst;
        public static T Inst {
            get {
                if (_inst) return _inst;
                var results = Resources.FindObjectsOfTypeAll<T>();
                if(results.Length != 1) throw new Exception($"One and only one configuration. find : {results.Length}");
                _inst = results[0];
                return _inst;
            }
        }
    }
}