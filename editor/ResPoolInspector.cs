using System.Linq;
using UniKh.extensions;
using UniKh.mgr;
using UniKh.res;
using UnityEditor;

namespace UniKh.editor {
    [CustomEditor(typeof(ResPool))]
    public class ResPoolInspector : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

//            var resPool = target as ResPool;

            EditorGUILayout.LabelField("|ResId\t|InPool\t|All\t|");
            if (ResPool.Pool != null) {
                var lst = ResPool.Pool.Keys.ToList();
                lst.Sort();
                foreach (var key in lst) {
                    EditorGUILayout.LabelField($"|{key}\t|{ResPool.Pool[key].Count}\t|{ResPool.totalCreated.TryGet(key, 0)}\t|");
                }
            }
        }
    }
}