using System;
using UniKh.comp.ui;
using UniKh.extensions;
using UnityEditor;
using UnityEngine;
using TextEditor = UnityEditor.UI.TextEditor;

namespace UniKh.editor {
    [CustomEditor(typeof(KhText), true)]
    [CanEditMultipleObjects]
    public class InspectorKhText : TextEditor {
        public SerializedProperty m_Script;
        public SerializedProperty m_Text;
        public SerializedProperty m_FontData;
        public SerializedProperty m_type;
        public SerializedProperty m_prefix;
        public SerializedProperty m_subfix;
        public SerializedProperty m_outline;
        public SerializedProperty m_outlineOffset;
        public SerializedProperty m_outlineColor;
        public SerializedProperty m_shadowOffset;
        public SerializedProperty m_shadowColor;
        public SerializedProperty numberTextSetting;
        public SerializedProperty numberTextSetting_value;
        public SerializedProperty numberTextSetting_rotateTo;
        public SerializedProperty numberTextSetting_showSign;
        public SerializedProperty numberTextSetting_unitLst;
        public SerializedProperty numberTextSetting_digit;
        public SerializedProperty numberTextSetting_shrink;
        public SerializedProperty numberTextSetting_format;

        protected override void OnEnable() {
            base.OnEnable();
            m_Script = serializedObject.FindProperty("m_Script");

            m_Text = serializedObject.FindProperty("m_Text");
            m_FontData = serializedObject.FindProperty("m_FontData");
            m_outline = serializedObject.FindProperty("m_outline");
            m_outlineOffset = serializedObject.FindProperty("m_outlineOffset");
            m_outlineColor = serializedObject.FindProperty("m_outlineColor");
            m_shadowColor = serializedObject.FindProperty("m_shadowColor");
            m_shadowOffset = serializedObject.FindProperty("m_shadowOffset");
            m_type = serializedObject.FindProperty("m_type");
            m_prefix = serializedObject.FindProperty("m_prefix");
            m_subfix = serializedObject.FindProperty("m_subfix");
            numberTextSetting = serializedObject.FindProperty("numberTextSetting");
            numberTextSetting_format = numberTextSetting.FindPropertyRelative("format");
            numberTextSetting_value = numberTextSetting.FindPropertyRelative("value");
            numberTextSetting_rotateTo = numberTextSetting.FindPropertyRelative("rotateTo");
            numberTextSetting_showSign = numberTextSetting.FindPropertyRelative("showSign");
            numberTextSetting_unitLst = numberTextSetting.FindPropertyRelative("unitLst");
            numberTextSetting_digit = numberTextSetting.FindPropertyRelative("digit");
            numberTextSetting_shrink = numberTextSetting.FindPropertyRelative("shrink");
        }

        public override void OnInspectorGUI() {
//            base.OnInspectorGUI();

            var a = target as KhText;
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(m_Script, true);
            
//            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Content", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_type, true);
            EditorGUILayout.PropertyField(m_prefix, true);
            switch (a.m_type) {
                case KhText.Type.Text:
                    EditorGUILayout.PropertyField(m_Text);
                    break;
                case KhText.Type.NumberText:
                    EditorGUILayout.PropertyField(numberTextSetting_value);
                    
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(numberTextSetting_showSign);
                    EditorGUILayout.PropertyField(numberTextSetting_digit);
                    EditorGUILayout.PropertyField(numberTextSetting_format);
                    if (!a.NumberFormat.Exists()) {
                        EditorGUILayout.PropertyField(numberTextSetting_shrink);
                        if (a.NumberShrink) { 
                            EditorGUILayout.PropertyField(numberTextSetting_unitLst, true);
                        }
                    }
                    EditorGUILayout.PropertyField(numberTextSetting_rotateTo);

                    EditorGUI.indentLevel--;
                    break;
                default:
                    break;
            }

            EditorGUILayout.PropertyField(m_subfix, true);
            EditorGUI.indentLevel--;

//            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(m_FontData);
            
            EditorGUILayout.LabelField("Appearance Setting", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            AppearanceControlsGUI();
            EditorGUILayout.PropertyField(m_outline);
            if (a.m_outline != KhText.OutlineType.None) { 
                EditorGUILayout.PropertyField(m_outlineOffset, true);
                EditorGUILayout.PropertyField(m_outlineColor, true);    
            }
            
            EditorGUILayout.PropertyField(m_shadowOffset);
            if (a.m_shadowOffset != Vector2.zero) {
                EditorGUILayout.PropertyField(m_shadowColor);    
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.LabelField("Events Setting", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            RaycastControlsGUI();
            EditorGUI.indentLevel--; 
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}