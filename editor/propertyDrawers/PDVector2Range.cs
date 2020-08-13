/** PDVector2Range.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2020/03/29 00:00:00
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.utils.Inspector;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    [CustomPropertyDrawer(typeof(Vector2RangeAttribute))]
    public class PDVector2Range : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var range = attribute as Vector2RangeAttribute;
            if (range == null || property.type != "Vector2") {
                EditorGUI.PropertyField(position, property, GUIContent.none);
            } else {
                var v = EditorGUI.Vector2Field(position, GUIContent.none, property.vector2Value);

                v = new Vector2(
                    v.x < range.XMin ? range.XMin :
                    v.x > range.XMax ? range.XMax :
                    v.x,
                    v.y < range.YMin ? range.YMin :
                    v.y > range.YMax ? range.YMax :
                    v.y);
                property.vector2Value = v;

                var pos = position.position;
                var size = position.size;
                var borderSize = new Vector2(2, size.y );

                var borderColor = new Color(1,0.5f,0f); 
                var progressColor = new Color(0.5f,1f,1f, 0.25f); 
                
                if (v.x <= range.XMin) {
                    EditorGUI.DrawRect(new Rect(pos + new Vector2(12, 0), borderSize), borderColor);
                }

                if (v.x >= range.XMax) {
                    EditorGUI.DrawRect(new Rect(pos + new Vector2(size.x / 2, 0), borderSize), borderColor);
                } 
                
                if (v.y <= range.YMin) {
                    EditorGUI.DrawRect(new Rect(pos + new Vector2(size.x / 2 + 12, 0), borderSize), borderColor); 
                }

                if (v.y >= range.YMax) {
                    EditorGUI.DrawRect(new Rect(pos + new Vector2(size.x, 0), borderSize), borderColor); 
                }
                EditorGUI.DrawRect(new Rect(pos + new Vector2(14, 1), new Vector2(2 + range.RateX(v.x) * (size.x / 2 - 16), size.y-2)), progressColor);  
                EditorGUI.DrawRect(new Rect(pos + new Vector2(size.x / 2 + 15, 1), new Vector2(1 + range.RateY(v.y) * (size.x / 2 - 16), size.y-2)), progressColor);  

            }

            EditorGUI.EndProperty();
        }
    }
}