using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniKh.editor;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ProjectView {
    static ProjectView() {
        EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
    }

    private static GUIStyle TagStyle {
        get {
            if (_tagStyle == null) {
                _tagStyle = new GUIStyle(EditorUtils.LabelCodeStyle) {
                    alignment = TextAnchor.MiddleRight, fontSize = 8, padding = {right = 4, bottom = 2},
                };
                return _tagStyle;
            }

            return _tagStyle;
        }
    }

    private static GUIStyle _tagStyle;

    private const string Folder = "/Resources/cr/";
    private const string Suffix = ".prefab";

    private static void ProjectWindowItemOnGUI(string guid, Rect rect) {
        var path = AssetDatabase.GUIDToAssetPath(guid);
        if (!path.Contains(Folder) || !path.EndsWith(Suffix)) return;

        var fileName = Path.GetFileNameWithoutExtension(path);
        var originSize = GUI.skin.label.CalcSize(new GUIContent(fileName)) + new Vector2(14, 0);
        rect.x += originSize.x;
        rect.y += 2;
        rect.width -= originSize.x;

        var cRes = AssetDatabase.LoadAssetAtPath<CRes>(path);

        if (!cRes) return;

        var drawSize = TagStyle.CalcSize(new GUIContent(cRes.alias));
        var drawRect = new Rect(rect.xMax - drawSize.x - 3, rect.yMin + 2, drawSize.x + 2, rect.height - 3);
        var bgColor = new Color(0.18f, 0.13f, 0.12f, 0.7f);
        EditorGUI.DrawRect(drawRect, bgColor);
        GUI.Label(rect, cRes.alias, TagStyle); // display the number (string) on the right.
        
        // EditorApplication.RepaintProjectWindow();
    }
}