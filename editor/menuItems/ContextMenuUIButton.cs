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
using System.Net.NetworkInformation;
using UniKh.comp.ui;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UniKh.editor {
    public class ContextMenuUIButton : ContextMenuUI {
        [MenuItem("GameObject/Kh UI (Molecules)/btn <Button>/Custom", false, 0)]
        internal static GameObject CreateUINodeButton(MenuCommand mc) {
            var go = CreateNewGameObject(mc.context as GameObject, "btn");
            var img = go.AddComponent<KhImage>();
            var btn = go.AddComponent<KhBtn>();
            btn.image = img;
            ((RectTransform) btn.transform).sizeDelta = new Vector2(160, 120);
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

        [MenuItem("CONTEXT/KhBtn/Use Bg Node",true)]
        static bool CheckIfKhBtnCanUseBgNode(MenuCommand command) {
            var btn = command.context as KhBtn;
            if(null == btn || null == btn.targetGraphic || btn.targetGraphic.gameObject != btn.gameObject) {
                return false;
            }
            return btn.targetGraphic is KhImage;
        }
        
        [MenuItem("CONTEXT/KhBtn/Use Bg Node")]
        static void UseBgNode(MenuCommand command) {
            var btn = command.context as KhBtn;
            if(null == btn) return;
            if (null == btn.targetGraphic) {
                Debug.LogError("KhBtn to convert bg model for must have targetGraphic been set.");
                return;
            }

            if (btn.targetGraphic.gameObject != btn.gameObject) { // attached on the same go, create a bg node
                Debug.LogError("when convert bg model, origin targetGraphic must be a khImage attached to the same gameObject of KhBtn.");
                return;
            }
            
            var originImage = btn.targetGraphic as KhImage;
            if (null == originImage) {
                Debug.LogError("when convert bg model, targetGraphic of the KhBtn must be khImage.");
                return;
            }
            
            if (!UnityEditorInternal.ComponentUtility.CopyComponent(originImage)) {
                Debug.LogError("KhBtn convert bg model failed: copy origin img failed.");
                return;
            }
                
            var goImg = CreateNewGameObject( btn.gameObject, "bg", "");
            var img = goImg.AddComponent<KhImage>();
            img.rectTransform.SetAnchorStretchAll();
            img.rectTransform.SetSiblingIndex(0);
            if (!UnityEditorInternal.ComponentUtility.PasteComponentValues(img)) {
                Debug.LogError("KhBtn convert bg model failed: paste img values failed.");
                Object.DestroyImmediate(goImg);
                return;
            } 
                
            Undo.RecordObject(btn, "Convert Bg Model");
            Undo.RegisterFullObjectHierarchyUndo(btn, "Convert Bg Model");
            btn.targetGraphic = img;
                
            Undo.DestroyObjectImmediate(originImage);
            Object.DestroyImmediate(originImage); 
            Undo.RegisterCreatedObjectUndo (goImg, "Convert Bg Model");
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