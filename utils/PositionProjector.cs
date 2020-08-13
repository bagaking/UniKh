/** PositionProjector.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/15 13:57:54
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.core;
using UniKh.extensions;
using UniKh.space;
using UnityEngine;

namespace UniKh.utils {
     
    public class PositionProjector : BetterBehavior {
        public static Quadrilateral3D CameraViewportToSceneQuadrilateral(
            Camera camScene,
            Plane3D plane
        ) {
            var rayLb = camScene.ViewportPointToRay(Vector2.zero);
            var rayLt = camScene.ViewportPointToRay(Vector2.up);
            var rayRt = camScene.ViewportPointToRay(Vector2.one);
            var rayRb = camScene.ViewportPointToRay(Vector2.right);
            var origin = rayLb.origin;
            var factor = -Vector3.Dot(plane.normal, (origin - plane.origin));

            return new Quadrilateral3D {
                lb = origin + factor * rayLb.direction / Vector3.Dot(plane.normal, rayLb.direction),
                lt = origin + factor * rayLt.direction / Vector3.Dot(plane.normal, rayLt.direction),
                rt = origin + factor * rayRt.direction / Vector3.Dot(plane.normal, rayRt.direction),
                rb = origin + factor * rayRb.direction / Vector3.Dot(plane.normal, rayRb.direction),
            };
        }

        public static Vector3 CameraViewportPositionToScenePosition(
            Camera camScene,
            Vector2 viewportPos, // form 0 to 1
            Plane3D plane
        ) {
            var ray = camScene.ViewportPointToRay(viewportPos);
            return plane.IntersectWithRay(ray);
        }
        public static Vector2 ScenePositionToUILocalPositionRestricted(
            Transform targetTransform,
            RectTransform parentRectTransform,
            Camera camScene,
            Canvas canvas = null,
            Vector2 windowsShrink = default) {
            if (null == targetTransform) {
                throw new Exception(
                    "UniKh.utils.PositionProjector ScenePositionToUILocalPosition error: target are not exist");
            }

            var pos = ScenePositionToUILocalPosition(targetTransform.position, parentRectTransform, camScene, canvas);
            
            return parentRectTransform.rect.GetProjectionOf(pos, windowsShrink)  ; // pos
        }
        
        public static Vector2 ScenePositionToUILocalPosition(
            Transform targetTransform,
            RectTransform parentRectTransform,
            Camera camScene,
            Canvas canvas = null) {
            if (null == targetTransform) {
                throw new Exception(
                    "UniKh.utils.PositionProjector ScenePositionToUILocalPosition error: target are not exist");
            }

            return ScenePositionToUILocalPosition(targetTransform.position, parentRectTransform, camScene, canvas);
        }

        public static Vector2 ScenePositionToUILocalPosition(
            Vector3 position3D,
            RectTransform parentRectTransform,
            Camera camScene,
            Canvas canvas = null) {
            if (null == parentRectTransform || null == camScene) {
                throw new Exception(
                    "UniKh.utils.PositionProjector ScenePositionToUILocalPosition error: input cannot be null");
            }

            var screenPos = camScene.WorldToScreenPoint(position3D);
            if (null == canvas) {
                canvas = parentRectTransform.GetComponentInParent<Canvas>();
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPos, canvas.worldCamera,
                out var localPoint);
            return localPoint;
        }
    }
}