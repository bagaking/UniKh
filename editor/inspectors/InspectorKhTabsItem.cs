/** InspectorKhBtn.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 15:21:03
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Collections;
using System.Collections.Generic;
using UniKh.comp.ui;
using UnityEngine;
using UniKh.core;
using UniKh.extensions;
using UnityEditor;
using UnityEditor.UI;

namespace UniKh.editor {
    
    [CustomEditor(typeof(KhTabsItem), true)]
    [CanEditMultipleObjects]
    public class InspectorKhTabsItem : InspectorKhBtn {

        public SerializedProperty pActive;
        public SerializedProperty pUnActive;
        
        protected override void OnEnable() {
            base.OnEnable(); 
            pActive = serializedObject.FindProperty("pActive");
            pUnActive = serializedObject.FindProperty("pUnActive");
        }
        
        public override void OnInspectorGUI() {
            
            base.OnInspectorGUI();
            
            serializedObject.Update();
            EditorGUILayout.Separator(); 
            EditorGUILayout.PropertyField(pActive, true);
            EditorGUILayout.PropertyField(pUnActive, true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}