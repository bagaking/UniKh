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
    
    [CustomEditor(typeof(KhBtn), true)]
    [CanEditMultipleObjects]
    public class InspectorKhBtn : ButtonEditor {

        public SerializedProperty m_Script;
        public SerializedProperty clickAnimation;
        public SerializedProperty tweenScale;
        public SerializedProperty text;
        public SerializedProperty audioName;
        
        protected override void OnEnable() {
            base.OnEnable();
            m_Script = serializedObject.FindProperty("m_Script");
            clickAnimation = serializedObject.FindProperty("clickAnimation");
            tweenScale = serializedObject.FindProperty("tweenScale");
            text = serializedObject.FindProperty("text");
            audioName = serializedObject.FindProperty("audioName");
        }
        
        public override void OnInspectorGUI() {
            
            base.OnInspectorGUI();
            
            var btn = target as KhBtn; 
            serializedObject.Update();
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(m_Script, true);
            EditorGUILayout.PropertyField(clickAnimation, true);
            EditorGUILayout.PropertyField(tweenScale, true);
            EditorGUILayout.PropertyField(text, true);
            EditorGUILayout.PropertyField(audioName, true);
            if (btn.audioName.Exists() && Application.isPlaying) {
                if (GUILayout.Button("Play")) {
                    btn.PlayClickAudio();
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}