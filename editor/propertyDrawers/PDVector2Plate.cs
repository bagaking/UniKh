/** PDVector2Plate.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/24 18:33:24
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.core.tween;
using UniKh.extensions;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UniKh.extensions;

namespace UniKh.editor {
    
    [CustomPropertyDrawer(typeof(Vector2PlateAttribute))]
    public class PDVector2Plate : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) * 4;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            var attr = attribute as Vector2PlateAttribute;
            
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (property.type != "Vector2") {
                EditorGUI.PropertyField(position, property, GUIContent.none);
                EditorGUI.EndProperty();
            }

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            {
                var slotHeight = position.height / 4;
                var slotWidth = position.width - position.height - 4;
                var rectX = new Rect(position.x + position.height + 4, position.y, slotWidth, slotHeight);
                var rectY = new Rect(position.x + position.height + 4, position.y + slotHeight, slotWidth, slotHeight);
                var rectRadar = new Rect(position.x + 1, position.y + 1, position.height - 2, position.height - 2);

                var oldLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 16;
                EditorGUI.PropertyField(rectX, property.FindPropertyRelative("x"), new GUIContent("X"));
                EditorGUI.PropertyField(rectY, property.FindPropertyRelative("y"));
                EditorGUIUtility.labelWidth = oldLabelWidth;
                
                
                EditorGUI.DrawRect(new Rect(rectRadar.min - Vector2.one * 1, rectRadar.size + Vector2.one * 2), Color.white.FromHex("#666666")); 
                EditorGUI.DrawRect(rectRadar, Color.white.FromHex("#444444"));

                var rate = (property.vector2Value - attr.minValue) / (attr.maxValue - attr.minValue);
                var inRectPos = rectRadar.min + rectRadar.size * rate;
                inRectPos.y = rectRadar.yMin + rectRadar.yMax - inRectPos.y;
                Handles.BeginGUI();
                {
                    Handles.color = new Color().FromHex("#60ACFC");
                    Handles.DrawSolidDisc(inRectPos, Vector3.back, 3);
                    
                }
                Handles.EndGUI();
            }
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}