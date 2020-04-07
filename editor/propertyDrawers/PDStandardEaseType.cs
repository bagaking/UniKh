/** PDStandardEaseType.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/16 18:52:00
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.core.tween;
using UniKh.extensions;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    [CustomPropertyDrawer(typeof(EaseDetailAttribute))]
    public class PDStandardEaseType : PropertyDrawer {

        public float ySlot = 1.9f;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) * (property.type == "Enum" ? ySlot : 1);
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
                var slotHeight = position.height / ySlot;
                var panelWidth = position.width / 3;
                var rEnum = new Rect(position.x, position.y, panelWidth * 2 - 3, slotHeight);
                var rPanelCurve = new Rect(position.x + panelWidth * 2, position.y, panelWidth - 3, position.height);
                var rPanelGradient = new Rect(position.x + 1, position.y + slotHeight + 3, panelWidth * 2 - 6, slotHeight * (ySlot - 1) - 3);

                EditorGUI.PropertyField(rEnum, property, GUIContent.none);
                var enumName = property.enumNames[property.enumValueIndex];
                var easeType = (StandardEase.Type) Enum.Parse(typeof(StandardEase.Type), enumName);
                var ease = StandardEase.Get(easeType);
//                Debug.Log(enumName + " " + ease);

                var sample = ease.GetSample(30);
               

                var bgColor = Color.white.FromHex("#444444"); 
                EditorGUI.DrawRect(rPanelCurve, bgColor);
                Handles.BeginGUI();
                {
                    var drawPoses = sample.Map(v2 => {
                        var offset = v2 * rPanelCurve.size;
                        return new Vector3(rPanelCurve.xMin + offset.x, rPanelCurve.yMax - offset.y);
                    });
                    var linePos = drawPoses.ToArray();

                    drawPoses.Insert(0, rPanelCurve.max);
                    Handles.color = new Color().FromHex("#60ACFC88");
                    Handles.DrawAAConvexPolygon(drawPoses.ToArray());
                    Handles.color = new Color().FromHex("#5bc49f");
                    Handles.DrawAAPolyLine(linePos);


//                   Handles.DrawPolyLine(drawPoses); 
                }
                Handles.EndGUI();

                var gradientStartColor = new Color().FromHex("#feb64d");
                var gradientEndColor = new Color().FromHex("#60ACFC");
                sample.ForEach((v2, ind) => {
                    if (ind == sample.Count - 1) return;
                    var offset = v2 * rPanelGradient.size;
                    var offset2 = sample[ind + 1] * rPanelGradient.size;
                    EditorGUI.DrawRect(new Rect(
                        rPanelGradient.xMin + offset.x,
                        rPanelGradient.yMin,
                        offset2.x - offset.x + 1,
                        rPanelGradient.height
                        ), Color.Lerp(gradientStartColor, gradientEndColor, v2.y));
                });
            }
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}