/** PDPlane3D.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2020/03/29 22:18:00
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Linq.Expressions;
using UniKh.coordinate;
using UniKh.dataStructure;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace UniKh.editor {
    [CustomPropertyDrawer(typeof(Literal32))]
    public class PDLiteral32 : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return (base.GetPropertyHeight(property, label) + 10);
        }

        public static LazyVal<GUIStyle> TagStyle = new LazyVal<GUIStyle>(
            () => new GUIStyle(EditorUtils.LabelCodeStyle) {alignment = TextAnchor.MiddleRight, fontSize = 9}
        );
        
        public static LazyVal<GUIStyle> FloatStyle = new LazyVal<GUIStyle>(
            () => {
                var floatStyle = new GUIStyle(GUI.skin.label) {
                    alignment = TextAnchor.UpperRight,
                    fontSize = 10,
                    fontStyle = FontStyle.Italic,
                    font = EditorUtils.EditorFontEditor
                };
                return floatStyle;
            }
        );
 

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            // position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var render = EditorUtils.Render.BeginSandBox();

            // EditorGUI.indentLevel = 0;
            position.position += new Vector2(0, 0);


            var r = new Rect(position.position, position.size - new Vector2(0, 9));
            var rTag = new Rect(
                position.position + new Vector2(2, position.size.y - 10),
                new Vector2(position.size.x, 10)
            );

            unchecked {
                var h = (uint) property.FindPropertyRelative("h").longValue;
                var l = (uint) property.FindPropertyRelative("l").longValue;
                var rawL = l & Literal32.MaskL;
                var rawH = h & Literal32.MaskH;

                var val = (uint) EditorGUI.IntField(r, label, (int) (rawL | rawH));
                property.FindPropertyRelative("h").longValue = (val & Literal32.MaskH | Literal32.RanL);
                property.FindPropertyRelative("l").longValue = (val & Literal32.MaskL | Literal32.RanH);


                EditorGUI.LabelField(
                    rTag,
                    $"{Convert.ToString(rawH, 16)}|{Convert.ToString(rawL, 16)} <{Convert.ToString(h, 16)}|{Convert.ToString(l, 16)}>",
                    TagStyle
                );

                render.SetColor(new Color(1, 1, 1, 0.3f));
                EditorGUI.LabelField(r, $"UniKh.Literal32", FloatStyle);
            }


            // EditorGUI.PropertyField(GetRect(0), );
            // EditorGUI.PropertyField(GetRect(1), property.FindPropertyRelative("l"));

            // var target = property.serializedObject.targetObject;
            // var targetType = target.GetType();
            // var field = targetType.GetField(property.propertyPath);
            // if (field != null) {
            //     var literal = (Literal32) field.GetValue(target);
            //
            //     var paintRect = GetRect(2);
            //     paintRect = new Rect(
            //         paintRect.position.x - 34,
            //         paintRect.position.y - 2,
            //         paintRect.size.x + 36,
            //         (paintRect.size.y + 2) * 3 + 3
            //     );
            //     EditorGUI.DrawRect(paintRect, new Color(0.1f, 0.1f, 0.1f));
            //
            //     void labArea(int i, string labelOf) {
            //         var labRect = GetRect(i);
            //         labRect.x -= 30;
            //         labRect.width = 32;
            //         EditorGUI.LabelField(labRect, labelOf);
            //     }
            //
            //     labArea(1, "rawVal");
            //     labArea(2, "masked"); 
            //
            //     EditorGUI.Vector2Field(GetRect(1), GUIContent.none, new Vector2(literal.RawL, literal.RawH));
            //     EditorGUI.Vector2Field(GetRect(2), GUIContent.none, new Vector2(literal.l, literal.h));
            //     
            // }

            // EditorGUI.PropertyField(GetRect(2), property.FindPropertyRelative("rt"), GUIContent.none);
            // EditorGUI.PropertyField(GetRect(3), property.FindPropertyRelative("rb"), GUIContent.none);
            // EditorUtils.Render.EndLabelWidth();

            EditorUtils.Render.EndSandBox();

            EditorGUI.EndProperty();
        }
    }
}