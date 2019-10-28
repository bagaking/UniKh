/** == ContextMenuUIEmpty.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/28 12:12:11
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.extensions;
using UniKh.comp.ui;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    
    public class ContextMenuUIEmpty : ContextMenuUI { 
        
        [MenuItem("GameObject/Kh UI Components/c <Empty>/Custom", false, 0)]
        internal static GameObject CreateUINodeEmptyCustom(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            return CreateNewUIObject(null != goParent ? goParent.transform : null, "c");
        }
        
        [MenuItem("GameObject/Kh UI Components/c <Empty>/Fill", false, 0)]
        internal static void CreateUINodeEmptyFill(MenuCommand mc) {
            var go = CreateUINodeEmptyCustom(mc);
            var rectTransform = go.transform as RectTransform;
            rectTransform.SetAnchorStretchAll();
        }
        
        [MenuItem("GameObject/Kh UI Components/c <Empty>/Anchor To Left Top", false, 0)]
        internal static void CreateUINodeEmptyPingLeftTop(MenuCommand mc) {
            var go = CreateUINodeEmptyCustom(mc);
            var rectTransform = go.transform as RectTransform;
            rectTransform.SetAnchorPingLeftTop();
        }
        
        [MenuItem("GameObject/Kh UI Components/c <Empty>/Anchor To Left Bottom", false, 0)]
        internal static void CreateUINodeEmptyPingLeftBottom(MenuCommand mc) {
            var go = CreateUINodeEmptyCustom(mc);
            var rectTransform = go.transform as RectTransform;
            rectTransform.SetAnchorPingLeftBottom();
        }
        
        [MenuItem("GameObject/Kh UI Components/c <Empty>/Anchor To Right Top", false, 0)]
        internal static void CreateUINodeEmptyPingRightTop(MenuCommand mc) {
            var go = CreateUINodeEmptyCustom(mc);
            var rectTransform = go.transform as RectTransform;
            rectTransform.SetAnchorPingRightTop();
        }
        
        [MenuItem("GameObject/Kh UI Components/c <Empty>/Anchor To Right Bottom", false, 0)]
        internal static void CreateUINodeEmptyPingRightBottom(MenuCommand mc) {
            var go = CreateUINodeEmptyCustom(mc);
            var rectTransform = go.transform as RectTransform;
            rectTransform.SetAnchorPingRightBottom();
        }
 
    }
}