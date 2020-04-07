/** PDLiteral32.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2020/04/07 12:00:00
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.coordinate; 
using UnityEditor; 
using UnityEngine; 

namespace UniKh.editor {
    [CustomPropertyDrawer(typeof(Plane3D))]
    public class PDPlane3D : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) { 
            EditorGUI.BeginProperty(position, label, property);
 
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
 
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
 
            var slotWidth = position.width - position.height - 2;
            var rIn = new Rect(position.x, position.y, slotWidth, position.height / 2);
            var rOut = new Rect(position.x, position.y + position.height / 2, slotWidth, position.height / 2);
            var rPanel = new Rect(position.x + slotWidth + 1, position.y, position.height, position.height);
 
            EditorGUI.PropertyField(rIn, property.FindPropertyRelative("origin"), GUIContent.none);
            EditorGUI.PropertyField(rOut, property.FindPropertyRelative("normal"), GUIContent.none); 
            var target = property.serializedObject.targetObject;
            var targetType = target.GetType();
            var field = targetType.GetField(property.propertyPath);

            Plane3D plane = null; 
            if (field != null) {
                plane = field.GetValue(target) as Plane3D;
            }
 
            var normalized = plane.normal.normalized;
            EditorGUI.DrawRect(rPanel, new Color(0, 0, 0, 0.2f));

            Handles.BeginGUI();
            Handles.color = Color.green;
            Handles.color = Color.Lerp(Color.green, Color.white, 0.2f);
 
            Handles.DrawAAPolyLine((Vector3) rPanel.center, (Vector3) rPanel.center + normalized * rPanel.height / 2);
 
            var x = normalized.y == 0
                ? 1
                : 1 / Mathf.Sqrt(1 + (normalized.x * normalized.x) / (normalized.y * normalized.y));
            var length = (rPanel.height / 2);
            var v1 = length * new Vector2(x, normalized.y == 0 ? 0 : x * normalized.x / normalized.y);
            var v2 = -v1;
                
            Handles.color = Color.Lerp(Color.blue, Color.white, 0.2f);
            Handles.DrawAAPolyLine(rPanel.center + v1, rPanel.center + v2);
 
            Handles.EndGUI();
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}