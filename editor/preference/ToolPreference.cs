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
            
            
            var fontOrg = EditorStyles.label.font;
            EditorStyles.label.font = EditorUtils.EditorFontEditor; 
            
            var terminalPath = GetTerminalPath();
            var terminalPathNew = EditorGUILayout.TextField("Terminal Path", terminalPath);
            if (terminalPath != terminalPathNew) EditorPrefs.SetString("terminalPath", terminalPathNew);
            
            var logo = EditorUtils.UniKh.LogoLight.Val;
            var size = new Vector2(logo.width, logo.height) / 4;
            var rect = GUILayoutUtility.GetRect(11, 100000, 11, 1000000);
            GUI.DrawTexture(new Rect(new Vector2(rect.width - size.x - 2, rect.position.y + rect.height - size.y - 2), size),  EditorUtils.UniKh.LogoLight);


            EditorStyles.label.font = fontOrg;
        } 
    }
}