using System;
using UniKh.extensions;
using UnityEngine;

namespace UniKh.utils {
    
    public static class Log {
        
        public static Action<string> Verbose => Debug.Log;

        public static Action<string> Warn => Debug.LogWarning;

        public static Action<string> Error => Debug.LogError;
    }
}