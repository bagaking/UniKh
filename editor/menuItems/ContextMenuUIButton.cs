/** == ContextMenuUIButton.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/01 23:18:16
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.core;
using UniKh.extensions;
using UniKh.utils;
using System.Collections;
using System.Collections.Generic;
using UniKh.comp.ui;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    public class ContextMenuUIButton : ContextMenuUI {
        [MenuItem("GameObject/Kh UI (Molecules)/btn <Button>/Custom", false, 0)]
        internal static GameObject CreateUINodeButton(MenuCommand mc) {
            var go = CreateNewGameObject(mc.context as GameObject, "btn");

            var btn = go.AddComponent<KhBtn>();
            btn.image = go.AddComponent<KhImage>();
            (btn.transform as RectTransform).sizeDelta = new Vector2(160, 120);
            return go;
        }

        [MenuItem("GameObject/Kh UI (Molecules)/btn <Button>/Anchor To Left Top", false, 0)]
        internal static void CreateUINodeButtonPingLeftTop(MenuCommand mc) {
            var go = CreateUINodeButton(mc);
            var rectTransform = go.transform as RectTransform;
            if (null == rectTransform) return;
            rectTransform.SetAnchorPingLeftTop();
        }

        [MenuItem("GameObject/Kh UI (Molecules)/btn <Button>/Anchor To Left Bottom", false, 0)]
        internal static void CreateUINodeButtonPingLeftBottom(MenuCommand mc) {
            var go = CreateUINodeButton(mc);
            var rectTransform = go.transform as RectTransform;
            if (null == rectTransform) return;
            rectTransform.SetAnchorPingLeftBottom();
        }

        [MenuItem("GameObject/Kh UI (Molecules)/btn <Button>/Anchor To Right Top", false, 0)]
        internal static void CreateUINodeButtonPingRightTop(MenuCommand mc) {
            var go = CreateUINodeButton(mc);
            var rectTransform = go.transform as RectTransform;
            if (null == rectTransform) return;
            rectTransform.SetAnchorPingRightTop();
        }

        [MenuItem("GameObject/Kh UI (Molecules)/btn <Button>/Anchor To Right Bottom", false, 0)]
        internal static void CreateUINodeButtonPingRightBottom(MenuCommand mc) {
            var go = CreateUINodeButton(mc);
            var rectTransform = go.transform as RectTransform;
            if (null == rectTransform) return;
            rectTransform.SetAnchorPingRightBottom();
        }


        [MenuItem("CONTEXT/KhBtn/Expand Click Area")]
        static void ExpandClickArea(MenuCommand command) {
            var btn = command.context as KhBtn;
            var go = CreateNewGameObject(btn.gameObject, "click_area", "");

            var image = go.AddComponent<KhImage>();
            image.color = new Color(0, 0, 0, 0.01f);
            image.rectTransform.SetAnchorStretchAll();
            image.rectTransform.sizeDelta =  new Vector2(80, 80);
        }

//        [MenuItem("GameObject/Kh UI (Molecules)/btn <Button>", true)]
//        internal static bool ValidateCreateUINodeButton(MenuCommand mc) {
//            var goParent = (mc.context as GameObject);
//            return goParent != null && goParent.GetComponent<RectTransform>() != null;
//
//        }
    }
}