/** EditorUtils.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/22 22:22:22
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;

namespace UniKh.editor {
    public static class EditorUtils {
        public static EditorRenderer Render = new EditorRenderer();

        public static int GetLastControlId() {
            return GUIUtility.GetControlID(FocusType.Passive) - 1;
        }

        public static Vector2 CulcContentSize(string text) {
            return CulcContentSize(new GUIContent(text), GUI.skin.label);
        }

        public static Vector2 CulcContentSize(GUIContent content, GUIStyle style) {
            return style.CalcSize(content);
        }
    }
}