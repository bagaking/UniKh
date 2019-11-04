/** == ContextMenuUIPanel.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/28 14:18:46
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.extensions;
using UniKh.comp.ui;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UniKh.editor {
    
    public class ContextMenuUIPanel : ContextMenuUI {
        
        [MenuItem("GameObject/Kh UI (Molecules)/p <Panel>/Ground", false, 0)]
        internal static void CreateUINodePanelGround(MenuCommand mc) {
            var go = CreateNewGameObject(mc.context as GameObject, "p");

            var panel = go.AddComponent<KhPanelGround>();
            panel.cg = go.AddComponent<CanvasGroup>();
            panel.startAlpha = 0.4f;
            go.AddComponent<KhImage>();

            var rect = go.GetComponent<RectTransform>();
            rect.SetAnchorStretchAll();
//            rect.anchorMin = Vector2.zero;
//            rect.anchorMax = Vector2.one;
//            rect.offsetMin = Vector2.zero;
//            rect.offsetMax = Vector2.zero;
        }
        
        [MenuItem("GameObject/Kh UI (Molecules)/p <Panel>/Window", false, 0)]
        internal static void CreateUINodePanelConst(MenuCommand mc) {
            var go = CreateNewGameObject(mc.context as GameObject, "p");
 
            var panel = go.AddComponent<KhPanelWindow>();
            panel.startOffset = Vector3.up * 100;
            panel.startScaleRate = 0.85f;
            go.AddComponent<KhImage>();
            var rectTransform = panel.transform as RectTransform;
            var rectParent = rectTransform.parent as RectTransform;

            if (rectParent.sizeDelta.x > 300 && rectParent.sizeDelta.y > 300) {
                rectTransform.sizeDelta = rectParent.sizeDelta - Vector2.one * 100;
            }
            else {
                rectTransform.sizeDelta =new Vector2(600,600);
            }

//            var rect = go.GetComponent<RectTransform>();

//            rect.anchorMin = Vector2.zero;
//            rect.anchorMax = Vector2.one;
//            rect.offsetMin = Vector2.zero;
//            rect.offsetMax = Vector2.zero;
        }

    }
}