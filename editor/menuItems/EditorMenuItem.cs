using System;
using System.Collections.Generic;
using System.Diagnostics;

using UniKh.comp.ui;
using UniKh.core;
using UniKh.extensions;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    public class EditorMenuItem {

        [MenuItem("UniKh/Tools/Open Terminal")]
        public static void OpenTerminal() {
            var path = ToolPreference.GetTerminalPath();
            if (path.Exists()) {
                Process.Start(path);
            }
        }
        
        [MenuItem("UniKh/Create/Config List")]
        public static void CreateLst() {
            var so = ScriptableObject.CreateInstance<KhPreferenceStatic>();
            
            UnityEditor.AssetDatabase.CreateAsset(so, $"Assets/Resources/{KhPreferenceStatic.assetName}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.EditorUtility.FocusProjectWindow ();
            UnityEditor.Selection.activeObject = so;
        } 
    }
}