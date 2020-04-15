using System.Diagnostics;
using UniKh.core;
using UniKh.extensions;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UniKh.editor {
    public class EditorMenuItem {
        [MenuItem("UniKh/Tools/Open Terminal %#`")]
        public static void OpenTerminal() {
            var path = ToolPreference.GetTerminalPath();
            if (path.Exists()) {
                Process.Start(path);
            } else {
                Debug.Log("Terminal path are not set, please set it in Edit/Preferences/UniKh/Tool");
            }
        }

        [MenuItem("UniKh/Create/Config List")]
        public static void CreateLst() {
            var so = ScriptableObject.CreateInstance<KhPreferenceStatic>();
            AssetDatabase.CreateAsset(so, $"Assets/Resources/{KhPreferenceStatic.assetName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = so;
        }
    }
}