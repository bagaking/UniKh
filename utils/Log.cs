using System.Linq;
using System.Text;
using UniKh.extensions;
using UnityEngine;

namespace UniKh.utils {
    
    public static class Log {
        
        public static void Verbose(params string[] messages) {
            Debug.Log(messages.ToItemsString());
        }
        
        public static void Warn(params string[] messages) {
            Debug.LogWarning(messages.ToItemsString());
        }
        
        public static void Error(params string[] messages) {
            Debug.LogError(messages.ToItemsString());
        }
        
    }
}