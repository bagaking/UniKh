/** == EaseBounce.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/24 15:58:19
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;

namespace UniKh.core.tween {
    public class EaseBounce : StandardEasing<EaseBounce> {
        private float d1 = 2.75f;
        private float n1 = 7.5625f;

        public override float EaseIn(float x) {
            return 1 - BounceOut(1 - x);
        }

        public override float EaseOut(float x) {
            return BounceOut(x);
        }

        float BounceOut(float x) {
            if (x < 1 / d1) {
                return n1 * x * x;
            }

            if (x < 2 / d1) {
                return n1 * (x -= (1.5f / d1)) * x + .75f;
            }

            if (x < 2.5 / d1) {
                return n1 * (x -= (2.25f / d1)) * x + .9375f;
            }

            return n1 * (x -= (2.625f / d1)) * x + .984375f;
        }
    }
}