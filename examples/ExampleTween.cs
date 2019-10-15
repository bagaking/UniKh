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


        public Transform transMoveY;
        public float transMoveYVal;
        public float transMoveYDuration;
        public int transMoveYLoop = 1;
//        public float transMoveYDirection;
//        public float transMoveYPingPong;

        /// <summary>
        /// The initial method of ExampleTween.
        /// This method **CAN** be removed
        /// </summary>
        protected override void OnSetActive(bool active) {
            transMoveY.TweenMoveY(transMoveYVal, transMoveYDuration).SetLoop(transMoveYLoop);
        }

    }
}