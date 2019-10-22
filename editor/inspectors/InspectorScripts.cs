/** InspectorScripts.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/22 14:00:13
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */
  
using UniKh.extensions;
using UniKh.utils;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    [CustomEditor(typeof(MonoScript))]
    public class InspectorScripts : Editor {
        public GUILayoutScrollPanel pScript = new GUILayoutScrollPanel();
        public GUILayoutTogglePanel pToggleObjects = new GUILayoutTogglePanel("All Objects");
//        public GUILayoutTextEditor textEditor = new GUILayoutTextEditor(new []{ "public", "namespace"});
        
        public override void OnInspectorGUI() {
            var scriptObj = target as MonoScript;
            var classObj = scriptObj.GetClass();
            if (null == classObj) {
                base.OnInspectorGUI();
            }
            else {
                if (classObj.Namespace.Exists()) {
                    EditorGUILayout.LabelField("namespace " + classObj.Namespace );
                }

                // draw class
                var strClass = SGen.New["class "];
                var classTemp = classObj;
                while (classTemp != null) {
                    if (classTemp == typeof(object)) {
                        strClass.Append("object");
                    }
                    else {
                        strClass.Append(classTemp.Name).Append(" <");
                    }
                    classTemp = classTemp.BaseType;
                } 
                EditorGUILayout.LabelField(strClass.End);
                
                // draw objects
                if (classObj.IsInherits(typeof(Object))) {
                    var objects = Resources.FindObjectsOfTypeAll(classObj);
                    pToggleObjects.Draw("[" + objects.Length + "]", () => {
                        var inAssets = objects.Filter(obj => EditorUtility.IsPersistent(obj));
                        var inScene = objects.Filter(obj => !EditorUtility.IsPersistent(obj));
                        EditorGUI.indentLevel += 1;
                        if (inScene.Count > 0) {
                            EditorGUILayout.LabelField("In Scene");
                            EditorGUI.indentLevel += 1;
                            inScene.ForEach(obj => { EditorGUILayout.ObjectField(obj, obj.GetType(), true); });
                            EditorGUI.indentLevel -= 1;
                        }

                        if (inAssets.Count > 0) {
                            EditorGUILayout.LabelField("In Assets");
                            EditorGUI.indentLevel += 1;
                            inAssets.ForEach(obj => { EditorGUILayout.ObjectField(obj, obj.GetType(), false); });
                            EditorGUI.indentLevel -= 1;
                        }

                        EditorGUILayout.Space();
                        EditorGUI.indentLevel -= 1;
                    });
                }    
            }
            
            pScript.Draw(() => {
                var style = new GUIStyle(GUI.skin.textArea);
                GUILayout.TextArea(
                    scriptObj.text.Replace("\r\n", "\n"),
                    style
                );
//                var kbc = GUIUtility.keyboardControl;
//                textEditor.DrawColoredTextArea(EditorUtils.GetLastControlID(), style);
            });
        }

        
    }
}