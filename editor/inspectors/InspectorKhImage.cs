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
        public SerializedProperty m_gray;
        public SerializedProperty m_mirror;
        public SerializedProperty m_rotate;
        public SerializedProperty m_skew;

        public SerializedProperty gradientColor;
        public SerializedProperty gradientDirection;
        public SerializedProperty gradientEase;

        protected override void OnEnable() {
            base.OnEnable();
            m_Script = serializedObject.FindProperty("m_Script");
            m_gray = serializedObject.FindProperty("m_gray");
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
            EditorGUILayout.PropertyField(m_gray, true);
            EditorGUILayout.PropertyField(m_mirror, true);
            EditorGUILayout.PropertyField(m_rotate, true);
            EditorGUILayout.PropertyField(m_skew, true);
            EditorGUILayout.PropertyField(gradientDirection, new GUIContent("Gradient"));
            if (img.gradientDirection != Vector2.zero) {
                var normalizedValue = gradientDirection.vector2Value.normalized;
                gradientDirection.vector2Value = new Vector2(
                    Mathf.Abs(normalizedValue.x) < 0.0001f ? 0 : normalizedValue.x,
                    Mathf.Abs(normalizedValue.y) < 0.0001f ? 0 : normalizedValue.y
                );
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(gradientColor, new GUIContent("Color"));
                EditorGUILayout.PropertyField(gradientEase, new GUIContent("Ease Type"));
                var c = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Remove Gradient")) {
                    gradientDirection.vector2Value = Vector2.zero;;
                }
                GUI.backgroundColor = c;
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}