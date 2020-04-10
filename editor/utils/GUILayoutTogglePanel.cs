/** GUILayoutTogglePanel.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/10 15:32:33
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;
using System;
using UniKh.utils;
using UnityEditor;

namespace UniKh.editor {
    public class GUILayoutTogglePanel {
        public string Title;
        public bool State;

        public GUIStyle TabStyle {
            get {
                if (_tabStyle == null) {
                    // GUI.skin.GetStyle("dragtab")
                    _tabStyle = new GUIStyle(GUI.skin.button) {
                        font = EditorUtils.EditorFontEditor,
                        fontSize = 12,
                        fontStyle = FontStyle.Normal,
                        alignment = TextAnchor.MiddleLeft,
                        padding = {
                            left = 8,
                        },
                        margin = new RectOffset(1, 1, 0, 0)
                    };
                }

                return _tabStyle;
            }
        }
        
        public Color BgColor;

        public GUILayoutTogglePanel(string title, bool initState = false, bool usingScroll = true) {
            this.Title = title;
            this.State = initState;
            BgColor = new Color(0.9f, 0.95f, 0.96f);
            if (usingScroll) {
                ScrollPanel = new GUILayoutScrollPanel();
            }
        }

        public GUILayoutScrollPanel ScrollPanel;
        private GUIStyle _tabStyle;

        public void Draw(string appendTitle, Action drawContent) {
            GUILayout.Space(3f);

            var cBgOrigin = GUI.backgroundColor;
            GUI.backgroundColor = BgColor;
            var text = SGen.New[State ? "\u25B2 " : "\u25BC "][Title][' '][appendTitle].End;
            //["<b> <size=11>"]["</size></b>"]

            State = GUILayout.Toggle(State, text, TabStyle, GUILayout.MinWidth(20f));
            var rectA = GUILayoutUtility.GetLastRect();
            // EditorGUI.DrawRect(rectA,  GUI.backgroundColor);

            GUILayout.Space(2f);
            GUI.backgroundColor = cBgOrigin;

            if (State && null != drawContent) {
                if (null != ScrollPanel) {
                    ScrollPanel.Draw(drawContent);
                } else {
                    drawContent();
                }
            } else {
                GUILayout.Space(3f);
            }
        }

        public void Draw(Action drawContent) {
            this.Draw("", drawContent);
        }
    }
}