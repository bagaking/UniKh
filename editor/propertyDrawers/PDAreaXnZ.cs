/** PDPlane3D.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2020/03/29 22:18:00
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Linq.Expressions;
using UniKh.coordinate;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    [CustomPropertyDrawer(typeof(AreaXnZ))]
    public class PDAreaXnZ : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return (base.GetPropertyHeight(property, label) + 2) * 5 + 4;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
          
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

           
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            position.position += new Vector2(0, 0);

            var part = new Vector3(position.size.x ,  position.size.y / 5 - 3);

            Rect GetRect(int i) {
                return new Rect(position.x, position.y + i * (part.y + 2) + (i >= 2 ? 3 : 0), part.x + (i >= 2 ? -2 : 0), part.y);
            } 
            
            EditorUtils.Render.BeginLabelWidth(16);
            EditorGUI.PropertyField(GetRect(0), property.FindPropertyRelative("center"), GUIContent.none);
            EditorGUI.PropertyField(GetRect(1), property.FindPropertyRelative("halfSize"), GUIContent.none);
            
            var target = property.serializedObject.targetObject;
            var targetType = target.GetType();
            var field = targetType.GetField(property.propertyPath); 
            if (field != null) {
                var area = field.GetValue(target) as AreaXnZ;
                if (area != null) {
                    var paintRect = GetRect(2);
                    paintRect = new Rect(paintRect.position.x - 34, paintRect.position.y - 2, paintRect.size.x + 36, (paintRect.size.y + 2) * 3 + 3); 
                    EditorGUI.DrawRect(paintRect, new Color(0.1f, 0.1f, 0.1f));
                    void labArea(int i, string labelOf){
                        var labRect =  GetRect(i);
                        labRect.x -= 30;
                        labRect.width = 32;
                        EditorGUI.LabelField(labRect, labelOf);
                    };
                    labArea(2, "size");
                    labArea(3, "lb");
                    labArea(4, "rt");
                    
                    EditorGUI.Vector2Field(GetRect(2), GUIContent.none, area.halfSize * 2);
                    EditorGUI.Vector3Field(GetRect(3), GUIContent.none, area.LB);
                    EditorGUI.Vector3Field(GetRect(4), GUIContent.none, area.RT);
                }
            }
             
            // EditorGUI.PropertyField(GetRect(2), property.FindPropertyRelative("rt"), GUIContent.none);
            // EditorGUI.PropertyField(GetRect(3), property.FindPropertyRelative("rb"), GUIContent.none);
            EditorUtils.Render.EndLabelWidth();

            EditorGUI.EndProperty();
        }
        
    }
}