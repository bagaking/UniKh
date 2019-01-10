using System;
using UniKh.extensions;
using UnityEngine;

namespace UniKh.utils {
    
    public static class Log {
        
        public static Action<string> Verbose => Debug.Log;

        public static Action<string> Warn => Debug.LogWarning;

        public static Action<string> Info {
            get {
                
                return s => {
                    var ind = s.IndexOf('\n');
                    if (ind < 0 || ind == s.Length - 1) {
                        Debug.Log($"<color=#00EEEE>{s}</color>");
                    } else {
                        Debug.Log($"<color=#00EEEE>{s.Substring(0, ind)}</color>{s.Substring(ind)}");
                    }
                };
            }
        }

        public static Action<string> Error => Debug.LogError;
    }
}