/** == InspectorKhImage.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/23 18:10:59
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */
 
using UniKh.comp.ui; 
using UnityEditor;
using UnityEditor.UI;

namespace UniKh.editor {
    
    [CustomEditor(typeof(KhImage), true)]
    [CanEditMultipleObjects]
    public class InspectorKhImage : ImageEditor {

        public SerializedProperty m_Script;
        public SerializedProperty m_mirror;
        public SerializedProperty m_rotate;
        public SerializedProperty m_skew;
        
        protected override void OnEnable() {
            base.OnEnable();
            m_Script = serializedObject.FindProperty("m_Script"); 
            m_mirror = serializedObject.FindProperty("m_mirror"); 
            m_rotate = serializedObject.FindProperty("m_rotate");
            m_skew = serializedObject.FindProperty("m_skew");
        }
        
        public override void OnInspectorGUI() {
            
            base.OnInspectorGUI();
            
            var btn = target as KhBtn; 
            serializedObject.Update();
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(m_Script, true);
            EditorGUILayout.PropertyField(m_mirror, true);
            EditorGUILayout.PropertyField(m_rotate, true);
            EditorGUILayout.PropertyField(m_skew, true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}