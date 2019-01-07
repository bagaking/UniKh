using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniKh.core;
using UniKh.extensions;
using UniKh.mgr;
using UniKh.res;
using UniKh.utils;
using UnityEditor;

namespace UniKh.editor {
    [CustomEditor(typeof(ResPool))]
    public class ResPoolInspector : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

//            var resPool = target as ResPool;

            var types = Assembly.GetAssembly(typeof(ResPool)).GetTypes();
            var lstEnumType = new List<IdsAttribute>();
            IdsTree root = null;
            types.ForEach(
                type => {
                    if (!Attribute.IsDefined(type, typeof(IdsAttribute))) return;
                    var attr = type.GetCustomAttribute<IdsAttribute>();
                    var idSeg = new IdsTree(attr.SegStart, attr.SegEnd, type);
                    root = root == null ? idSeg : root.Insert(idSeg);
                });
            
            
            root.ForEach(
                (t, d) => EditorGUILayout.LabelField(" - ".Repeat(d) + t.ToString()));
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("|ResId\t|InPool\t|All\t|Desc\t|");
            if (ResPool.Pool != null) {
                var lst = ResPool.Pool.Keys.ToList();
                lst.Sort();
                foreach (var key in lst) {
                    EditorGUILayout.LabelField(
                        $"|{key}\t|{ResPool.Pool[key].Count}\t|{ResPool.totalCreated.TryGet(key, 0)}\t|{root.Find(key)}\t|");
                }
            }

//            Log.Verbose(root.ToDir());
        }
    }
}