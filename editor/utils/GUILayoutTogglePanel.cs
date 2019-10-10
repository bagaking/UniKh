/** GUILayoutTogglePanel.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/10 15:32:33
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;
using System;
using UniKh.utils;

namespace UniKh.editor {
    public class GUILayoutTogglePanel {

        public string title;
        public bool state;
        public GUILayoutTogglePanel(string title, bool initState = false) {
            this.title = title;
            this.state = initState;
        }

        public void Draw(string appendTitle, Action DeawContent) {
            GUILayout.Space(3f);

            var cBgOrigin = GUI.backgroundColor;
            GUI.backgroundColor = new Color(2f, 2f, 0.9f);
            state = GUILayout.Toggle(state, SGen.New[state ? "\u25B2" : "\u25BC"]["<b> <size=11>"][title][' '][appendTitle]["</size></b>"].End, "dragtab", GUILayout.MinWidth(20f));

            GUILayout.Space(2f);
            GUI.backgroundColor = cBgOrigin;

            if (state && null != DeawContent) {
                DeawContent();
            } else {
                GUILayout.Space(3f);
            }

        }

        public void Draw(Action DeawContent) {
            this.Draw("", DeawContent);
        }
    }
}