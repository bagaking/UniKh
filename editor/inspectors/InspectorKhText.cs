using UniKh.comp.ui;
using UnityEditor;
using UnityEditor.UI;

namespace UniKh.editor
{
    [CustomEditor(typeof(KhText), true)]
    [CanEditMultipleObjects]
    public class InspectorKhText:TextEditor
    {
        public SerializedProperty m_Script;
        public SerializedProperty unitLst;
        public SerializedProperty m_numberValue;
        public SerializedProperty m_rotateTarget;
        public SerializedProperty showSign;
        public SerializedProperty usingTextValue;
        protected override void OnEnable() {
            base.OnEnable();
            m_Script = serializedObject.FindProperty("m_Script");
            unitLst = serializedObject.FindProperty("unitLst");
            m_numberValue = serializedObject.FindProperty("m_numberValue");
            m_rotateTarget = serializedObject.FindProperty("m_rotateTarget");
            showSign = serializedObject.FindProperty("showSign");
            usingTextValue = serializedObject.FindProperty("usingTextValue");
        }
        
        public override void OnInspectorGUI() {
            
            base.OnInspectorGUI();
            
            var a = target as KhText; 
            serializedObject.Update();
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(m_Script, true);
            EditorGUILayout.PropertyField(unitLst, true);
            EditorGUILayout.PropertyField(m_numberValue, true);
            EditorGUILayout.PropertyField(m_rotateTarget, true);
            EditorGUILayout.PropertyField(showSign, true);
            EditorGUILayout.PropertyField(usingTextValue, true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}