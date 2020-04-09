/** == InspectorKhImage.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/23 18:10:59
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.mvc;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    [CustomEditor(typeof(UICenter.UIControllerBase), true)]
    [CanEditMultipleObjects]
    public class InspectorUIController : Editor {
        private const string usage = @"[USAGE]:
- visit instance: {0}.Inst";

        

        public override void OnInspectorGUI() {

            var barColor = new Color(0.3f, 0.5f, 0.7f, 0.1f);
            var barShadowColor = new Color(0.1f, 0.3f, 0.4f, 0.05f);

            var rect = EditorUtils.DrawTag("[UniKh.UIController]", usage.Replace("{0}", target.GetType().Name));
            rect.xMin -= 14;
            rect.width += 14;
            var rectHeader = new Rect(rect.x, rect.y - 18, rect.width, rect.height + 4);
            EditorUtils.DrawOverlapHeaderRect(rectHeader, barColor, barShadowColor);

            var fontOrg = EditorStyles.label.font;
            EditorStyles.label.font = EditorUtils.EditorFontEditor;
            base.OnInspectorGUI();

            var endRect = EditorGUILayout.GetControlRect(false, 2f);
            endRect.yMin += 8;
            endRect.yMax += 7;
            endRect.xMin -= 14;
            endRect.width += 14;
            var rectAll = new Rect(rect.xMin, rectHeader.yMax, rect.width, endRect.yMax - rectHeader.yMax);
            EditorGUI.DrawRect(rectAll, new Color(0.1f, 0.1f, 0.1f, 0.2f));
            EditorGUI.DrawRect(endRect, new Color(0.05f, 0.05f, 0.1f, 0.2f));
            EditorStyles.label.font = fontOrg;
        }
    }
}