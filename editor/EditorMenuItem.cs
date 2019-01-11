using UniKh.core;
using UnityEngine;

namespace UniKh.editor {
    public class EditorMenuItem {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("UniKh/Create/Config List")]
        public static void CreateLst() {
            var so = ScriptableObject.CreateInstance<ConfigList>();
            
            UnityEditor.AssetDatabase.CreateAsset(so, $"Assets/Resources/{ConfigList.assetName}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.EditorUtility.FocusProjectWindow ();
            UnityEditor.Selection.activeObject = so;
        }
#endif
    }
}