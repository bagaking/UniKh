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
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (property.propertyType != SerializedPropertyType.Vector2) {
                EditorGUI.PropertyField(position, property, GUIContent.none);
                EditorGUI.EndProperty();
            }

            var attr = attribute as Vector2PlateAttribute;
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            {
                var slotHeight = position.height / 4;
                var slotWidth = position.width - position.height - 4;
                
                var rectLabelX = new Rect(position.x + position.height + 4, position.y, slotWidth, slotHeight);
                var rectLabelY = new Rect(position.x + position.height + 4, position.y + 2 * slotHeight, slotWidth, slotHeight);
                
                var rectX = new Rect(position.x + position.height + 4, position.y + slotHeight, slotWidth, slotHeight);
                var rectY = new Rect(position.x + position.height + 4, position.y + 3 * slotHeight, slotWidth, slotHeight);
                var rectRadar = new Rect(position.x + 1, position.y + 1, position.height - 2, position.height - 2);

                var oldLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 16;
                EditorGUI.LabelField(rectLabelX, new GUIContent(" [" + attr.minValue.x + ", " + attr.maxValue.x + "]"));
                EditorGUI.PropertyField(rectX, property.FindPropertyRelative("x"), new GUIContent("X"));
                EditorGUI.LabelField(rectLabelY, new GUIContent(" [" + attr.minValue.y + ", " + attr.maxValue.y + "]"));
                EditorGUI.PropertyField(rectY, property.FindPropertyRelative("y"));
                EditorGUIUtility.labelWidth = oldLabelWidth;

                EditorGUI.DrawRect(new Rect(rectRadar.min - Vector2.one * 1, rectRadar.size + Vector2.one * 2),
                    Color.white.FromHex("#666666"));
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
                GUI.backgroundColor = Color.clear;
                var pressed = GUI.Button(rectRadar, "");
                if (pressed) {
                    var ratioClick = (Event.current.mousePosition - rectRadar.min) / (rectRadar.max - rectRadar.min);
                    var valueSelected = attr.minValue + ratioClick * (attr.maxValue - attr.minValue);
                    valueSelected = new Vector2(valueSelected.x, -valueSelected.y);
                    property.vector2Value = valueSelected;
                    Debug.Log("POs BTN:" + Event.current.mousePosition + " in " + rectRadar + " ? " +
                              rectRadar.Contains(Event.current.mousePosition) + " _ " + Event.current.button);
                }
            }
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}