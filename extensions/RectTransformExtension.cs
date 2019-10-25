/** == RectTransformExtension.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/26 01:40:14
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;

namespace UniKh.extensions {
    
    public static class RectTransformExtension {

        public static RectTransform SetAnchorStretchTop(this RectTransform rectTransform) {
            rectTransform.anchorMin = Vector2.up;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = rectTransform.sizeDelta.MappingX(x => 0);
            rectTransform.pivot = new Vector2(0.5f, 1);
            return rectTransform;
        }
        
        public static RectTransform SetAnchorStretchBottom(this RectTransform rectTransform) {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.right;
            rectTransform.sizeDelta = rectTransform.sizeDelta.MappingX(x => 0);
            rectTransform.pivot = new Vector2(0.5f, 0);
            return rectTransform;
        }
        
        public static RectTransform SetAnchorStretchLeft(this RectTransform rectTransform) {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.up;
            rectTransform.sizeDelta = rectTransform.sizeDelta.MappingY(y => 0);
            rectTransform.pivot = new Vector2(0, 0.5f);
            return rectTransform;
        }
        
        public static RectTransform SetAnchorStretchRight(this RectTransform rectTransform) {
            rectTransform.anchorMin = Vector2.right;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = rectTransform.sizeDelta.MappingY(y => 0);
            rectTransform.pivot = new Vector2(1, 0.5f);
            return rectTransform;
        }
    }
}