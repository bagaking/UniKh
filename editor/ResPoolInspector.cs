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
using UnityEngine;

namespace UniKh.editor {
    [CustomEditor(typeof(ResPool))]
    public class ResPoolInspector : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

//            var resPool = target as ResPool;

            var types = Assembly.GetAssembly(typeof(IdsAttribute)).GetTypes();
            var lstEnumType = new List<IdsAttribute>();
            IdsTree root = null;
            types.ForEach(
                type => {
                    if (!Attribute.IsDefined(type, typeof(IdsAttribute))) return;
                    var attr = type.GetCustomAttribute<IdsAttribute>();
                    var idSeg = new IdsTree(attr.SegStart, attr.SegEnd, type);
                    root = root == null ? idSeg : root.Insert(idSeg);
                });

            if (null != root)
            {
                root.ForEach(
                    (t, d) => EditorGUILayout.LabelField(" - ".Repeat(d) + t.ToString()));
                EditorGUILayout.Space();
            }

            
            if (!Application.isPlaying) {
                EditorGUILayout.LabelField("Monitor will show while playing");
                return;
            }

            EditorGUILayout.LabelField("|ResId\t|InPool\t|All\t|Desc\t|");
            if (ResPool.Inst.pool != null) {
                var lst = ResPool.Inst.pool.Keys.ToList();
                lst.Sort();
                foreach (var key in lst) {
                    var desc = null != root ? root.Find(key).ToString() : "NONE";
                    EditorGUILayout.LabelField(
                        $"|{key}\t|{ResPool.Inst.pool[key].Count}\t|{ResPool.Inst.totalCreated.TryGet(key, 0)}\t|{desc}\t|");
                }
            }

//            Log.Verbose(root.ToDir());
        }
    }
}