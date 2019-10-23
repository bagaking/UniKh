using System.Collections.Generic;
using UniKh.core;
using UniKh.extensions;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    public class EditorMenuItem {
        
        [UnityEditor.MenuItem("UniKh/Create/Config List")]
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
                
        [MenuItem("GameObject/Kh UI Component/c <Empty>", false, 0)]
        static void CreateUINodeEmpty(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            if (null == goParent) {
                // todo
                return;
            }
            var transParent = goParent.transform;
            if (transParent == null) return;
            
            var go = new GameObject(GetNewTransformName(transParent, "c"));
            go.transform.SetParent(transParent);
            go.transform.localPosition = Vector3.zero;
            go.transform.eulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;

            Selection.activeObject = go;
        }
        
    }
}