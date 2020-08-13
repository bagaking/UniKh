/** PDQuadrilateral3D.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2020/03/29 00:00:00
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.space;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    [CustomPropertyDrawer(typeof(Quadrilateral3D))]
    public class PDQuadrilateral3D : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) * 3;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
          
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            position.position += new Vector2(16, 0);

            var part = new Vector3(position.size.x / 2 - 12,  position.size.y / 3);

            Rect GetRect(int i) {
                return new Rect(position.x + (i % 2) * (part.x + 8), position.y + part.y + (i / 2) * part.y, part.x, part.y);
            } 
            
            EditorUtils.Render.BeginLabelWidth(16);
            EditorGUI.PropertyField(GetRect(0), property.FindPropertyRelative("lb"), GUIContent.none);
            EditorGUI.PropertyField(GetRect(1), property.FindPropertyRelative("lt"), GUIContent.none);
             
            EditorGUI.PropertyField(GetRect(2), property.FindPropertyRelative("rt"), GUIContent.none);
            EditorGUI.PropertyField(GetRect(3), property.FindPropertyRelative("rb"), GUIContent.none);
            EditorUtils.Render.EndLabelWidth();

            EditorGUI.EndProperty();
        }
        
    }
}