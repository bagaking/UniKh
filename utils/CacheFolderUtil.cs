using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace UniKh.utils {
    
    public class CacheFolderUtil {
        public static string CachePath {
            get {
#if NETFX_CORE
            return Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#else
                return Application.persistentDataPath + "/";
#endif
            }
        }

        public static StreamWriter GetAppendSW(string path) {
            var filePath = Path.Combine(CachePath, path);
            var dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath ?? throw new InvalidOperationException());
            var sw = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write));
            sw.WriteLine("----");
            return sw;
        }
    }
}