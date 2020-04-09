/** EditorUtils.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/22 22:22:22
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    public static class EditorUtils {

        public static GUIStyle LabelCodeStyle {
            get {
                if (_labelCodeStyle == null) {
                    _labelCodeStyle = new GUIStyle(GUI.skin.label) {font = EditorFontCode}; 
                } 
                return _labelCodeStyle;
            }
        }
        private static GUIStyle _labelCodeStyle;
        
        public static GUIStyle LabelEditorStyle {
            get {
                if (_labelEditorStyle == null) {
                    _labelEditorStyle = new GUIStyle(GUI.skin.label) {font = EditorFontEditor}; 
                } 
                return _labelEditorStyle;
            }
        }
        private static GUIStyle _labelEditorStyle;
        
        public static GUIStyle LabelEditorTagStyle {
            get {
                if (_labelEditorTagStyle == null) {
                    _labelEditorTagStyle = new GUIStyle(LabelEditorStyle) { 
                        alignment = TextAnchor.LowerRight,
                        padding = new RectOffset(2, 2, 2, 2)
                    }; 
                } 
                return _labelEditorTagStyle;
            }
        }
        private static GUIStyle _labelEditorTagStyle;
        
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
        
        public static Rect DrawTag(string tag, string hint) {
            var rect = EditorGUILayout.GetControlRect(false, 16);
            Render.BeginColor(Color.gray);
            GUI.Label(
                rect,
                new GUIContent(tag, hint),
                LabelEditorTagStyle
            );
            Render.EndColor();
            return rect;
        }
        
        public static void DrawOverlapHeaderRect(Rect r, Color cLight, Color cShadow) { 
            cLight.a = 0.1f;
            cShadow.a = 0.05f;
            EditorGUI.DrawRect(r, cLight);
            var lightLine = new Rect(r.xMin, r.yMax - 2, r.width, 2);
            EditorGUI.DrawRect(lightLine, cLight);
            lightLine.y += 1;
            lightLine.height = 1;
            EditorGUI.DrawRect(lightLine, cLight);
            lightLine.y += 1;
            EditorGUI.DrawRect(lightLine, cShadow);
            lightLine.height += 1;
            EditorGUI.DrawRect(lightLine, cShadow);
        }

    }
}