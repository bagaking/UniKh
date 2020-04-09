using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniKh.core;
using UniKh.extensions;
using UniKh.res;
using UniKh.utils;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    public class WizardCreateLight : ScriptableWizard {
        public List<GameObject> gameObjects;

        public static void CreateWizard(string id, Func<List<GameObject>> getGos) {
            var wizard = DisplayWizard<WizardCreateLight>("Pool Items");
            wizard.getGos = getGos;
            wizard.createButtonName = "CLOSE";
            wizard.helpString = "res id : " + id;
        }
        
        void OnWizardCreate() {
            // do nothing
        }

        void OnWizardUpdate() {
            // helpString = "update time " + Time.time;
        }

        private EveryUpdate eU = new EveryUpdate(2f);
        private Func<List<GameObject>> getGos;

        void Update() {
            if(!Application.isPlaying) Close();
            if (!eU.Test(0.02f)) return;
            
            gameObjects = getGos();
            Repaint();
        } 
    }

    [CustomEditor(typeof(ResPool))]
    public class ResPoolInspector : Editor {
        [MenuItem("UniKh/Monitor/ResPool")]
        public static void ShowDialog() {
            Selection.activeObject = FindObjectOfType<ResPool>();
        }

        public override void OnInspectorGUI() {
            var r = EditorUtils.Render.BeginSandBox();

            var rect = EditorUtils.DrawTag("[UniKh.ResPool]", null);
            rect.xMin -= 14;
            rect.width += 14;
            var rectHeader = new Rect(rect.x, rect.y - 18, rect.width, rect.height + 4);
            EditorUtils.DrawOverlapHeaderRect(rectHeader, new Color(0.7f, 0.5f, 0.4f), Color.black);


            var types = Assembly.GetAssembly(typeof(IdsAttribute)).GetTypes();
            var lstEnumType = new List<IdsAttribute>();
            IdsTree root = null;
            types.ForEach(
                type => {
                    if (!Attribute.IsDefined(type, typeof(IdsAttribute))) return;
                    var attr = type.GetCustomAttribute<IdsAttribute>();
                    var idSeg = new IdsTree(attr.SegStart, attr.SegEnd, type);
                    root = root == null ? idSeg : root.Insert(idSeg);
                }
            );

            if (null != root) {
                root.ForEach((t, d) => EditorGUILayout.LabelField(" - ".Repeat(d) + t.ToString()));
                EditorGUILayout.Space();
            }


            if (!Application.isPlaying) {
                r.SetColor(new Color(1f, 0.3f, 0.2f));
                EditorGUILayout.LabelField("Monitor will show while playing", EditorUtils.LabelEditorTagStyle);
            } else if (ResPool.Inst.pool != null) {
                var lst = ResPool.Inst.pool.Keys.ToList();
                lst.Sort();
                var lightLine = true;
                var maxKey = ResPool.Inst.pool.Keys.Reduce((prev, key) => prev > key ? prev : key, 0u);
                var maxKeyLength = maxKey.ToString().Length;
                var keyResId = "ResId".PadRight(maxKeyLength, ' ');

                r.SetColor(new Color(0.9f, 0.6f, 0.3f));
                EditorGUILayout.LabelField($"|{keyResId}|InPool|All   |Desc\t|", EditorUtils.LabelCodeStyle);


                var cCreated = new Color(0.8f, 0.8f, 0.8f);
                var cInUse = new Color(0.6f, 1f, 0.9f);
                var cTooMuch = new Color(1f, 0.5f, 0.5f);

                foreach (var key in lst) {
                    var pool = ResPool.Inst.pool[key];
                    var poolCount = ResPool.Inst.pool[key].Count;
                    var totalCreated = ResPool.Inst.totalCreated.TryGet(key, 0);
                    var desc = null != root ? root.Find(key).ToString() : "NONE";

                    r.SetColor(
                        totalCreated == 0 ? Color.gray :
                        totalCreated == poolCount ? cCreated :
                        (totalCreated - poolCount) > 20 ? cTooMuch : cInUse
                    );
                    EditorGUILayout.LabelField(
                        $"|{key.ToString().PadRight(maxKeyLength, ' ')}|{poolCount.ToString().PadRight(6)}|{totalCreated.ToString().PadRight(6)}|{desc}\t|",
                        EditorUtils.LabelCodeStyle
                    );
                    var lineRect = GUILayoutUtility.GetLastRect();
                    if (lightLine) {
                        EditorGUI.DrawRect(lineRect, new Color(0.7f, 0.7f, 0.7f, 0.05f));
                    }

                    if (Event.current.type == EventType.MouseUp && lineRect.Contains(Event.current.mousePosition)) {
                        WizardCreateLight.CreateWizard(key.ToString(), () => pool);
                    }

                    lightLine = !lightLine;
                }
            } else {
                r.SetColor(new Color(0.8f, 0.5f, 0.2f));
                EditorGUILayout.LabelField("Empty pool", EditorUtils.LabelEditorTagStyle);
            }

            r.EndSandBox();
        }
    }
}