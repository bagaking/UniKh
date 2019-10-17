/** ExampleTween.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 22:43:25
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniKh.core;
using UniKh.extensions;
using UniKh.core.csp;
using UniKh.core.csp.waiting;
using UniKh.core.tween;

namespace UniKh.example {
    public class ExampleTween : BetterBehavior {

        [Header("Move Y")]
        public Transform transMoveY;
        public float transMoveYVal;
        public float transMoveYDuration = 1;
        public int transMoveYLoop = 1;
        
        [Header("Move Offset")]
        public Transform transMoveOffset;
        public Vector3 transMoveOffsetVal;
        public float transMoveOffsetDuration = 1;
        public int transMoveOffsetLoop = 1;
        [EaseDetail(true)]
        public StandardEase.Type transMoveOffsetEaseType = StandardEase.Type.Linear;
        public bool transMoveOffsetUsingCurve = false;
        public CubicBezier transMoveOffsetEaseCurve = new CubicBezier(new Vector2(0.645f, 0.045f), new Vector2(0.355f, 1f));
        
//        public float transMoveYDirection;
//        public float transMoveYPingPong;

        /// <summary>
        /// The initial method of ExampleTween.
        /// This method **CAN** be removed
        /// </summary>
        protected override void OnSetActive(bool active) {
            transMoveY.TweenMoveY(transMoveYVal, transMoveYDuration).SetLoop(transMoveYLoop);
            var tweenTransMoveOffset = transMoveOffset.TweenMoveOffset(transMoveOffsetVal, transMoveOffsetDuration).SetLoop(transMoveOffsetLoop);
            if (transMoveOffsetUsingCurve) {
                tweenTransMoveOffset.SetEase(new EaseBezier(transMoveOffsetEaseCurve));
            }
            else {
                tweenTransMoveOffset.SetEase(transMoveOffsetEaseType);
            }
                
        }
        
    }
}