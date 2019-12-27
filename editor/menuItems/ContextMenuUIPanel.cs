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
            var go = CreateNewGameObject(mc.context as GameObject, "p-ground", "");

            var panel = go.AddComponent<KhPanelGround>();
            var motion = panel.CreateDefaultMotionComponents();
            
            var rectTransform = panel.transform as RectTransform;
            if (null == rectTransform) {
                Debug.LogError("Panel should be created in a canvas");
                return;
            }
            rectTransform.SetAnchorStretchAll();
            
            var goBg = CreateNewGameObject(go, "_bg", "");
            var iBg = goBg.AddComponent<KhImage>();
            iBg.rectTransform
                .SetAnchorStretchAll()
                .sizeDelta = new Vector2(0, 0);
            
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/Kh UI (Molecules)/p <Panel>/Window", false, 0)]
        internal static void CreateUINodePanelConst(MenuCommand mc) {
            var go = CreateNewGameObject(mc.context as GameObject, "p-window", "0");

            var panel = go.AddComponent<KhPanelWindow>();
            var motion = panel.CreateDefaultMotionComponents(); 

            var rectTransform = panel.transform as RectTransform;
            if (null == rectTransform) {
                Debug.LogError("Panel should be created in a canvas");
                return;
            }
            var rectParent = rectTransform.parent as RectTransform;

            if (rectParent != null && rectParent.sizeDelta.x > 300 && rectParent.sizeDelta.y > 300) {
                rectTransform.sizeDelta = rectParent.sizeDelta - Vector2.one * 100;
            }
            else {
                rectTransform.sizeDelta = new Vector2(600, 600);
            }

            var goMask = CreateNewGameObject(go, "_msk", "");
            var iMask = goMask.AddComponent<KhImage>();
            iMask.rectTransform
                .SetAnchorStretchAll()
                .sizeDelta = new Vector2(5000, 5000);
            iMask.color = new Color(0, 0, 0, 0.65f);
            panel.btnMask = goMask.AddComponent<KhBtn>();

            var goBg = CreateNewGameObject(go, "_bg", "");
            var iBg = goBg.AddComponent<KhImage>();
            iBg.rectTransform
                .SetAnchorStretchAll()
                .sizeDelta = new Vector2(0, 0);

            var goClose = CreateNewGameObject(goBg, "_btn", "close");
            var iClose = goClose.AddComponent<KhImage>();
            iClose.rectTransform
                .SetAnchorPingRightTop()
                .sizeDelta = new Vector2(128, 128);
            iClose.color = new Color(1, 0, 0, 0.65f);
            panel.btnClose = goClose.AddComponent<KhBtn>();
            
//            var clickEvent = new Button.ButtonClickedEvent();
//            var getMethodInfo = UnityEventBase.GetValidMethodInfo(go, "SetActive", new[] {typeof(bool)});
//            
//            clickEvent.AddListener(clickEvent.GetDelegate(go, getMethodInfo));
//            btnClose.onClick = clickEvent;

            Selection.activeObject = go;
        }
    }
}