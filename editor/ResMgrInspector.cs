using UniKh.mgr;
using UnityEditor;

namespace UniKh.editor {
    [CustomEditor(typeof(ResMgr))]
    public class ResMgrInspector : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            var resMgr = target as ResMgr;

            EditorGUILayout.LabelField("Cache1");
            if (resMgr.cache.Cache1 != null) {
                foreach (var pair in resMgr.cache.Cache1) {
                    EditorGUILayout.LabelField(pair.Key);
                }
            }

            EditorGUILayout.LabelField("Cache2");
            if (resMgr.cache.Cache2 != null) {
                foreach (var pair in resMgr.cache.Cache2) {
                    EditorGUILayout.LabelField(pair.Key);
                }
            }
        }
    }
}