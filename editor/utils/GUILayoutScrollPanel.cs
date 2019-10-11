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
    public class GUILayoutScrollPanel {

        public Vector2 position;

        public GUILayoutScrollPanel() { 
        }

        public void Draw(Action DeawContent, params GUILayoutOption[] options) {
            position = EditorGUILayout.BeginScrollView(position, options);
            DeawContent();
            EditorGUILayout.EndScrollView();
        }

    }
}