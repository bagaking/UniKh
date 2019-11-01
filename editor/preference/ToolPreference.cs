/** == EditorPreference.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/24 11:53:37
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */
 
using UnityEngine;
using UnityEditor;

namespace UniKh.editor {
    
    public class ToolPreference : MonoBehaviour {
        public static string GetTerminalPath() {
            return EditorPrefs.GetString("terminalPath", "");
        }

//        [SettingsProvider]
        [PreferenceItem("UniKh/Tool")]
        public static void DrawUniKhTool() {
            var terminalPath = GetTerminalPath();
            var terminalPathNew = EditorGUILayout.TextField("Terminal Path", terminalPath);
            if (terminalPath != terminalPathNew) EditorPrefs.SetString("terminalPath", terminalPathNew);
        } 
    }
}