/** PDCubicBezier.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/16 18:52:00
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.core.tween;
using UniKh.extensions;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniKh.editor {
    [CustomPropertyDrawer(typeof(EaseDetailAttribute))]
    public class PDStandardEaseType : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) * (property.type == "Enum" ? 3 : 1);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
 
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (property.type != "Enum") {
                EditorGUI.PropertyField(position, property, GUIContent.none);
                EditorGUI.EndProperty();
            }
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            {
                var slotWidth = position.width - 62;
                var rEnum = new Rect(position.x, position.y, slotWidth, position.height / 3);
                var rPanel = new Rect(position.x + slotWidth + 1, position.y, 60, position.height);

                EditorGUI.PropertyField(rEnum, property, GUIContent.none);
                EditorGUI.DrawRect(rPanel, Color.black);
                var enumName = property.enumNames[property.enumValueIndex];
                var easeType = (StandardEase.Type) Enum.Parse(typeof(StandardEase.Type),enumName);
                var ease = StandardEase.Get(easeType);
                Debug.Log(enumName + " " + ease);

                var drawPoses = ease.GetSample(20).Map(v2 => {
                        var offset = v2 * rPanel.size;
                        return new Vector3(rPanel.xMin + offset.x, rPanel.yMax - offset.y);
                    }).ToArray();

                Handles.BeginGUI();
                {
                    Handles.color = Color.green;
                    Handles.color = Color.Lerp(Color.green, Color.white, 0.2f);
                    Handles.DrawAAPolyLine(drawPoses);
//                    Handles.DrawPolyLine(drawPoses);
                }
                Handles.EndGUI();
            }
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
    
    
}