/** PositionProjector.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/15 13:57:54
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.core;
using UnityEngine;

namespace UniKh.utils {
    public class PositionProjector : BetterBehavior {

        
        public static Vector2 ScenePositionToUILocalPosition(
            Transform targetTransform, 
            RectTransform parentRectTransform, 
            Camera camScene, 
            Canvas canvas = null) {
            if (null == targetTransform) {
                throw new Exception("UniKh.utils.PositionProjector ScenePositionToUILocalPosition error: target are not exist");
            }
            return ScenePositionToUILocalPosition(targetTransform.position, parentRectTransform, camScene, canvas);
        }
        
        public static Vector2 ScenePositionToUILocalPosition(
            Vector3 position3D, 
            RectTransform parentRectTransform, 
            Camera camScene, 
            Canvas canvas = null) {
            if (null == parentRectTransform || null == camScene) {
                throw new Exception("UniKh.utils.PositionProjector ScenePositionToUILocalPosition error: input cannot be null");
            }
            var screenPos = camScene.WorldToScreenPoint(position3D);
            if (null == canvas) {
                canvas = parentRectTransform.GetComponentInParent<Canvas>();
            }
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPos, canvas.worldCamera, out var localPoint);
            return localPoint;
        }

    }
}
