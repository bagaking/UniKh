/** EditorUtils.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/22 22:22:22
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.dataStructure;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    public static class EditorUtils {

        public static class UniKh {
            public static LazyVal<Texture> LogoWhite = new LazyVal<Texture>(()=>(Texture) Resources.Load("logo/logo-unikh-white"));
            public static LazyVal<Texture> LogoGray = new LazyVal<Texture>(()=>(Texture) Resources.Load("logo/logo-unikh-gray"));
            public static LazyVal<Texture> LogoLight = new LazyVal<Texture>(()=>(Texture) Resources.Load("logo/logo-unikh-light"));
        }
        
        public static LazyVal<GUIStyle> LabelCodeStyle =
            new LazyVal<GUIStyle>(() => new GUIStyle(GUI.skin.label) {font = EditorFontCode});

        public static LazyVal<GUIStyle> LabelEditorStyle =
            new LazyVal<GUIStyle>(() => new GUIStyle(GUI.skin.label) {font = EditorFontEditor});  
        
        
        public static LazyVal<GUIStyle> LabelEditorTagStyle =
            new LazyVal<GUIStyle>(() => new GUIStyle(LabelEditorStyle) {
                alignment = TextAnchor.LowerRight,
                padding = new RectOffset(2, 2, 2, 2)
            });  
            
        public static LazyVal<Font> EditorFontCode =
            new LazyVal<Font>(() => (Font) Resources.Load("font/editor_SourceCodeVariable"));
 
        public static LazyVal<Font> EditorFontEditor =
            new LazyVal<Font>(() => (Font) Resources.Load("font/editor_PangMenZhengDao"));
        
 
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