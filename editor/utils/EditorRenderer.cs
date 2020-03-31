using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    public class EditorRenderer {
        public class SandBoxBlob {
            public SandBoxBlob() {
                _contentColor = GUI.contentColor;
                _backgroundColor = GUI.backgroundColor;
                _labelWidth = EditorGUIUtility.labelWidth;
                _indentLevel = EditorGUI.indentLevel;
            }

            private readonly Color _contentColor;
            private readonly Color _backgroundColor;
            private readonly float _labelWidth;
            private readonly int _indentLevel;

            public void RecoverToInit() {
                GUI.contentColor = _contentColor;
                GUI.backgroundColor = _backgroundColor;
                EditorGUIUtility.labelWidth = _labelWidth;
                EditorGUI.indentLevel = _indentLevel;
            }
        }

        private readonly Stack<float> _labelWidthStack = new Stack<float>();
        private readonly Stack<Color> _contentBackStack = new Stack<Color>();
        private readonly Stack<Color> _contentColorStack = new Stack<Color>();
        private readonly Stack<SandBoxBlob> _sandBoxStack = new Stack<SandBoxBlob>();

        public EditorRenderer SetLabelWidth(float width) {
            EditorGUIUtility.labelWidth = width;
            return this;
        }

        public EditorRenderer BeginLabelWidth(float width) {
            _labelWidthStack.Push(EditorGUIUtility.labelWidth);
            return this.SetLabelWidth(width);
        }

        public EditorRenderer EndLabelWidth() {
            EditorGUIUtility.labelWidth = _labelWidthStack.Pop();
            return this;
        }

        public EditorRenderer SetIndentLevel(int indentLevel) {
            EditorGUI.indentLevel = indentLevel;
            return this;
        }

        public EditorRenderer IncIndentLevel(int indentLevelInc = 1) {
            EditorGUI.indentLevel += indentLevelInc;
            return this;
        }

        public EditorRenderer SetBgColor(Color backgroundColor) {
            GUI.backgroundColor = backgroundColor;
            return this;
        }

        public EditorRenderer BeginBgColor(Color backgroundColor) {
            _contentBackStack.Push(GUI.backgroundColor);
            return this.SetBgColor(backgroundColor);
        }

        public EditorRenderer EndBgColor() {
            GUI.backgroundColor = _contentBackStack.Pop();
            return this;
        }

        public EditorRenderer SetColor(Color contentColor) {
            GUI.contentColor = contentColor;
            return this;
        }

        public EditorRenderer BeginColor(Color contentColor) {
            _contentColorStack.Push(GUI.contentColor);
            return this.SetColor(contentColor);
        }

        public EditorRenderer EndColor() {
            GUI.contentColor = _contentColorStack.Pop();
            return this;
        }

        public EditorRenderer BeginSandBox() {
            _sandBoxStack.Push(new SandBoxBlob());
            return this;
        }

        public EditorRenderer EndSandBox() {
            _sandBoxStack.Pop().RecoverToInit();
            return this;
        }

        public EditorRenderer SandBox(Action<EditorRenderer> fnDraw) {
            if (fnDraw == null) return this;
            BeginSandBox();
            fnDraw(this);
            EndSandBox();
            return this;
        }

        public EditorRenderer Space() {
            EditorGUILayout.Space();
            return this;
        }

        public EditorRenderer Space(float pixels) {
            GUILayout.Space(pixels);
            return this;
        }

        private Vector2 contentStartPosition;

        public EditorRenderer BeginContents(GUIStyle style = null, params GUILayoutOption[] options) {
            EditorGUILayout.BeginHorizontal();
            Space(3f);
            EditorGUILayout.BeginHorizontal(style == null ? "AS TextArea" : style, options);
            EditorGUILayout.BeginVertical();
            Space(2f);
            if (Event.current.type != EventType.DragPerform) {
                contentStartPosition = GUILayoutUtility.GetLastRect().position;
            }

            return this;
        }

        public EditorRenderer EndContents(Color cFrame = default, Color cMask = default) {
            // default of color is equal to Clear
            Space(2f);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
            Space(2f);
            
            var right = GUILayoutUtility.GetLastRect().position;
            var pos = contentStartPosition - new Vector2(12, 0);
            
            EditorGUILayout.EndHorizontal();
            Space(3f);

            var bot = GUILayoutUtility.GetLastRect().position;
            var size = new Vector2(right.x - 4, bot.y - contentStartPosition.y);

            if (cMask != default) {
                EditorGUI.DrawRect(new Rect(pos, size), cMask);
            }

            if (cFrame != default) {
                DrawLine(new Vector3[] {
                    pos,
                    new Vector3(pos.x, pos.y + size.y),
                    (pos + size),
                    new Vector3(pos.x + size.x, pos.y)
                }, cFrame, true);
            }

            return this;
        }

        public EditorRenderer Contents(Action<EditorRenderer> fnDraw) {
            if (fnDraw == null) return this;
            BeginContents();
            fnDraw(this);
            EndContents();
            return this;
        }

        public EditorRenderer DrawLine(Vector3[] points, Color color, bool closure = false) {
            if (null == points || points.Length < 2) return this;
            Handles.BeginGUI();
            Handles.color = color;

            Handles.DrawAAPolyLine(points);
            if (closure) {
                points.Append(points[0]);
            }

            Handles.EndGUI();
            return this;
        }
    }
}