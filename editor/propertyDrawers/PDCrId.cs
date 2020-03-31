/** PDCrId.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2020/03/30 21:28:00
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.editor;
using UnityEditor;
using UnityEngine;
using UniKh.utils;

[CustomPropertyDrawer(typeof(CrIdAttribute))]
public class PDCrId : PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        var crId = attribute as CrIdAttribute;

        var rect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        if (property.type != "uint") {
            EditorGUI.PropertyField(position, property, GUIContent.none);
            EditorGUI.EndProperty();
            return;
        }

        var length = 0;
        var segStart = crId.SegStart;
        do {
            length += 1;
        } while ((segStart /= 10) > 0);

        var partHeight = rect.size.y;

        var strPrefix = crId.SegStart.ToString();
        var strLabel = SGen.New[strPrefix].Append('*', 8 - length)[':'].End; 
        var strValue = property.intValue.ToString();

        var power = Mathf.FloorToInt(Mathf.Pow(10, 8 - length));
        var culc = EditorUtils.CulcContentSize(strLabel);
        EditorUtils.Render.SandBox(r => {
            r.SetLabelWidth(culc.x + 5).SetColor(strValue.Length == 8
                    ? (strValue.StartsWith(strPrefix) ? Color.green : Color.yellow)
                    : Color.red); // todo: using config
            EditorGUI.PropertyField(rect, property, new GUIContent(strLabel));
            // if (property.intValue % power == 0) {
            //     var culc = EditorUtils.CulcContentSize(strLabel);
            //     r.SetLabelWidth(culc.x + 5);
            //     property.intValue = 0;
            //     EditorGUI.PropertyField(rect, property, new GUIContent(strLabel));
            // } else {
            //     
            //     
            //     var culc = EditorUtils.CulcContentSize(strPrefix);
            //     r.SetLabelWidth(culc.x + 2);
            //
            //     property.intValue = (int) (crId.SegStart * power +
            //                            EditorGUI.IntField(rect, new GUIContent(strPrefix),
            //                                property.intValue % power) % power);
            // }
        });

        EditorGUI.EndProperty();
    }
}