/** == ContextMenuUIPanel.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/28 14:18:46
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Reflection;
using UniKh.extensions;
using UniKh.comp.ui;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
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
            var go = CreateNewGameObject(mc.context as GameObject, "p", "0");

            var panel = go.AddComponent<KhPanelWindow>();
            panel.startOffset = Vector3.up * 100;
            panel.startScaleRate = 0.85f;

            var rectTransform = panel.transform as RectTransform;
            var rectParent = rectTransform.parent as RectTransform;

            if (rectParent.sizeDelta.x > 300 && rectParent.sizeDelta.y > 300) {
                rectTransform.sizeDelta = rectParent.sizeDelta - Vector2.one * 100;
            }
            else {
                rectTransform.sizeDelta = new Vector2(600, 600);
            }

            var goMask = CreateNewGameObject(go, "mask", "");
            var iMask = goMask.AddComponent<KhImage>();
            iMask.rectTransform.SetAnchorStretchAll();
            iMask.rectTransform.sizeDelta = new Vector2(5000, 5000);
            iMask.color = new Color(0, 0, 0, 0.65f);
            var btnMask = goMask.AddComponent<KhBtn>();

            var goBg = CreateNewGameObject(go, "bg", "");
            var iBg = goBg.AddComponent<KhImage>();
            iBg.rectTransform.SetAnchorStretchAll();
            iBg.rectTransform.sizeDelta = new Vector2(0, 0);

            var goClose = CreateNewGameObject(go, "btn", "close");
            var iClose = goClose.AddComponent<KhImage>();
            iClose.rectTransform.SetAnchorPingRightTop();
            iClose.rectTransform.sizeDelta = new Vector2(128, 128);
            iClose.color = new Color(1, 0, 0, 0.65f);
            var btnClose = goClose.AddComponent<KhBtn>();
//            var clickEvent = new Button.ButtonClickedEvent();
//            var getMethodInfo = UnityEventBase.GetValidMethodInfo(go, "SetActive", new[] {typeof(bool)});
//            
//            clickEvent.AddListener(clickEvent.GetDelegate(go, getMethodInfo));
//            btnClose.onClick = clickEvent;
        }
    }
}