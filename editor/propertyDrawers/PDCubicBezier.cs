/** PDCubicBezier.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/16 18:52:00
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.extensions;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniKh.editor {
    [CustomPropertyDrawer(typeof(CubicBezier))]
    public class PDCubicBezier : PropertyDrawer {
        
//        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
//            // Create property container element.
//            var container = new VisualElement();
//
//            // Create property fields.
//            var amountField = new PropertyField(property.FindPropertyRelative("h1Exp"), "Handle In");
//            var unitField = new PropertyField(property.FindPropertyRelative("h2Exp"), "Handle Out");
////   var nameField = new PropertyField(property.FindPropertyRelative("name"), "Fancy Name");
//
//            // Add fields to the container.
//            container.Add(amountField);
//            container.Add(unitField);
////   container.Add(nameField);
//
//            return container;
//        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var slotWidth = position.width - 62;
            var rIn = new Rect(position.x, position.y, slotWidth, position.height / 2);
            var rOut = new Rect(position.x, position.y + position.height / 2, slotWidth, position.height / 2);
            var rPanel = new Rect(position.x + slotWidth + 1, position.y, 60, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(rIn, property.FindPropertyRelative("m_hIn"), GUIContent.none);
            EditorGUI.PropertyField(rOut, property.FindPropertyRelative("m_hOut"), GUIContent.none);
//            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
            var target = property.serializedObject.targetObject;
            var targetType = target.GetType();
            var field = targetType.GetField(property.propertyPath);
            
            CubicBezier cb = null;
            Vector3[] drawPoses = null;
            if (field != null) {
                cb = field.GetValue(target) as CubicBezier;
            }

            if (cb != null) {
                cb.ReCalculate();

                var lst = cb.GetSample(20).Map(v => {
                    var offset = v * rPanel.size;
                    return new Vector3(rPanel.xMin + offset.x, rPanel.yMax - offset.y);
                });
                lst.Add(new Vector3(rPanel.xMax, rPanel.yMin));
                drawPoses = lst.ToArray(); 
                
//                Debug.Log(cb.h1Exp);
//                Debug.Log(cb.h2Exp);
//                Debug.Log(cb.GetSample(20).ToItemsString());
//                Debug.Log(drawPoses.ToItemsString());
            }
           

            EditorGUI.DrawRect(rPanel, Color.black);
            
            Handles.BeginGUI(); 
            Handles.color = Color.green;
            Handles.color = Color.Lerp(Color.green, Color.white, 0.2f);

            if (null != drawPoses) {
                Handles.DrawPolyLine(drawPoses);
            }

//            Handles.DrawLine(new Vector2(rPanel.xMin, rPanel.yMax), new Vector2(rPanel.xMax, rPanel.yMin));
//             Handles.DrawLines(new Vector2(rPanel.xMin + 1, rPanel.yMax), new Vector2(rPanel.xMax, rPanel.yMin+1));
//            Handles.DrawLine(new Vector2(rPanel.xMin, rPanel.yMax-1), new Vector2(rPanel.xMax-1, rPanel.yMin));
            Handles.EndGUI();
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}