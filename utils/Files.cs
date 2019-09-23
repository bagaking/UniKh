using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniKh.extensions;

namespace UniKh.utils{

    public static class Files {

        public static char PathSeparator {
            get {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_XBOXONE || UNITY_WINRT_8_1
                return '\\';
#else
                return '/';
#endif
            }
        }


        public static string GetDirName(string path) { return Path.GetDirectoryName(path); }

        public static string GetExtention(string path) { return Path.GetExtension(path); }

        public static string GetFileNameWithoutExtension(string path) { return Path.GetFileNameWithoutExtension(path); }

        public static DirectoryInfo EnsureFolder(string path) {
            string dirPath = GetDirName(path);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            return new DirectoryInfo(dirPath);
        }

        public static string[] GetFilenamesAtDir(string path) {
            var files = EnsureFolder(path).GetFiles();
            return files.Map(f => f.Name);
        }

        public static void ForEachFile(string dirPath, Action<FileInfo> func) {
            if (func == null) return;
            DirectoryInfo di = EnsureFolder(dirPath);
            var files = di.GetFiles();
            var directories = di.GetDirectories();
            files.ForEach(f => func(f));
            directories.ForEach(d => ForEachFile(d.FullName + PathSeparator, func));
        }

        public static List<string> GetFilenamesAtDirOfExtention(string path, string extention) {
            var files = EnsureFolder(path).GetFiles();
            return files.Filter(f => f.Extension == extention).Map(f => f.Name);
        }

        public static string[] GetFilePathesAtDir(string path) {
            var files = EnsureFolder(path).GetFiles();
            return files.Map(f => f.FullName);
        }


        public static void WriteStringIntoFile(this string code, string path) {
            EnsureFolder(path);
            File.WriteAllText(path, code, Encoding.UTF8);
        }

        public static void WriteBytesIntoFile(this byte[] data, string path) {
            EnsureFolder(path);
            File.WriteAllBytes(path, data);
        }

        public static string ReadStringIfFileExist(string path, Encoding encoding = null) {
            if (!File.Exists(path)) return "";
            string text = "";
            if (encoding == null) using (StreamReader reader = File.OpenText(path)) text = reader.ReadToEnd();
            else using (StreamReader reader = new StreamReader(path, encoding)) text = reader.ReadToEnd();
            return text;
        }

        public static byte[] ReadAllBytesIfFileExist(string path) {
            if (!File.Exists(path)) return null;
            return File.ReadAllBytes(path);
        }

        public static bool Exist(string filePath) { return File.Exists(filePath); }

    }

}
