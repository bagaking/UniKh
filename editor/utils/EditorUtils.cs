/** EditorUtils.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/22 22:22:22
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;

namespace UniKh.editor {
    public static class EditorUtils {

        public static GUIStyle LabelCodeStyle {
            get {
                if (_labelCodeStyle == null) {
                    _labelCodeStyle = new GUIStyle(GUI.skin.label);
                    _labelCodeStyle.font = EditorFontCode;
                    return _labelCodeStyle;
                } 
                return _labelCodeStyle;
            }
        }
        private static GUIStyle _labelCodeStyle;
        
        public static GUIStyle LabelEditorStyle {
            get {
                if (_labelEditorStyle == null) {
                    _labelEditorStyle = new GUIStyle(GUI.skin.label);
                    _labelEditorStyle.font = EditorFontEditor;
                    return _labelEditorStyle;
                } 
                return _labelEditorStyle;
            }
        }
        private static GUIStyle _labelEditorStyle;
        
        public static Font EditorFontCode {
            get {
                if (_editorFontCode == null) {
                    // _editorFontCode = (Font) Resources.Load("font/editor_NotoMono");
                    _editorFontCode = (Font) Resources.Load("font/editor_SourceCodeVariable");
                }

                return _editorFontCode;
            }
        }

        private static Font _editorFontCode = null;
        
        public static Font EditorFontEditor {
            get {
                if (_editorFontCh == null) {
                    _editorFontCh = (Font) Resources.Load("font/editor_PangMenZhengDao");
                }

                return _editorFontCh;
            }
        }

        private static Font _editorFontCh = null;
            
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