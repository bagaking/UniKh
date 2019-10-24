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
            var path = EditorPreference.GetTerminalPath();
            if (path.Exists()) {
                Process.Start(path);
            }
        }
        
        [MenuItem("UniKh/Create/Config List")]
        public static void CreateLst() {
            var so = ScriptableObject.CreateInstance<ConfigList>();
            
            UnityEditor.AssetDatabase.CreateAsset(so, $"Assets/Resources/{ConfigList.assetName}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.EditorUtility.FocusProjectWindow ();
            UnityEditor.Selection.activeObject = so;
        }

        private static string GetNewTransformName(Transform parent, string title = "c") {
            if (parent == null) {
                return title;
            }
            var children = parent.GetDirectChildren(n => n.name.StartsWith(title + ":"));
            if (children.Count <= 0) return title + ":00";
            var t = children[children.Count - 1];
            return t.name.Replace(' ', '_').IncNumberTail(2);
        }

        private static GameObject CreateNewObject(Transform transParent, string title) { 
            if (null == transParent) { return null; }
            var go = new GameObject(GetNewTransformName(transParent, title));
            go.transform.SetParent(transParent);
            go.transform.localPosition = Vector3.zero;
            go.transform.eulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;

            Selection.activeObject = go; 
            return go;
        }
                
        [MenuItem("GameObject/Kh UI Component/c <Empty>", false, 0)]
        static void CreateUINodeEmpty(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            CreateNewObject(null != goParent ? goParent.transform : null, "c");
        }
        
        [MenuItem("GameObject/Kh UI Component/i <Image>", false, 0)]
        static void CreateUINodeImage(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            var go = CreateNewObject(null != goParent ? goParent.transform : null, "i");
            go.AddComponent<KhImage>();
        }
        
    }
}