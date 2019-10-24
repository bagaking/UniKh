/** == InspectorKhImage.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/23 18:10:59
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */
 
using UniKh.comp.ui; 
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace UniKh.editor {
    
    [CustomEditor(typeof(KhImage), true)]
    [CanEditMultipleObjects]
    public class InspectorKhImage : ImageEditor {

        public SerializedProperty m_Script;
        public SerializedProperty m_mirror;
        public SerializedProperty m_rotate;
        public SerializedProperty m_skew;

        public SerializedProperty gradientColor;
        public SerializedProperty gradientDirection;
        public SerializedProperty gradientEase;
        
        protected override void OnEnable() {
            base.OnEnable();
            m_Script = serializedObject.FindProperty("m_Script"); 
            m_mirror = serializedObject.FindProperty("m_mirror"); 
            m_rotate = serializedObject.FindProperty("m_rotate");
            m_skew = serializedObject.FindProperty("m_skew");
            
            gradientColor = serializedObject.FindProperty("gradientColor");
            gradientDirection = serializedObject.FindProperty("gradientDirection");
            gradientEase = serializedObject.FindProperty("gradientEase");
        }
        
        public override void OnInspectorGUI() {
            
            base.OnInspectorGUI();
            
            var img = target as KhImage; 
            serializedObject.Update();
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(m_Script, true);
            EditorGUILayout.PropertyField(m_mirror, true);
            EditorGUILayout.PropertyField(m_rotate, true);
            EditorGUILayout.PropertyField(m_skew, true);
            EditorGUILayout.PropertyField(gradientDirection, new GUIContent("Gradient"));
            if (img.gradientDirection != Vector2.zero) {
                gradientDirection.vector2Value = gradientDirection.vector2Value.normalized;
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(gradientColor, new GUIContent("Color"));
                EditorGUILayout.PropertyField(gradientEase, new GUIContent("Ease Type"));
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}