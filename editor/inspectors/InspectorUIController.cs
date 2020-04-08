/** == InspectorKhImage.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/23 18:10:59
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.comp.ui;
using UniKh.mvc;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace UniKh.editor {
    [CustomEditor(typeof(UICenter.UIControllerBase), true)]
    [CanEditMultipleObjects]
    public class InspectorUIController : Editor {  
        public override void OnInspectorGUI() {
            
            var style = new GUIStyle(EditorUtils.LabelEditorStyle);
            style.alignment = TextAnchor.MiddleRight;
            style.padding = new RectOffset(2,4,2,2);

            var fontOrg = EditorStyles.label.font;
            EditorStyles.label.font = EditorUtils.EditorFontEditor;
            
            // EditorGUILayout.LabelField("[UniKh.UIController]", style);
            var id = EditorUtils.GetLastControlId();
            var rect = EditorGUILayout.GetControlRect(false, 16f);
            EditorGUI.DrawRect(rect, new Color(0.3f, 0.5f,0.7f, 0.2f));
            GUI.Label(rect, "[UniKh.UIController]", style);
            
            base.OnInspectorGUI();

            EditorStyles.label.font = fontOrg;

        }
    }
}