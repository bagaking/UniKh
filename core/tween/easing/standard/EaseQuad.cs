/** EaseQuad.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/17 13:23:09
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;

namespace UniKh.core.tween {
    public class EaseQuad : StandardEasing<EaseQuad> {
        public override float EaseIn(float x) {
            return x * x;
        }

        public override float EaseOut(float x) {
            return 1 - EaseIn(1 - x);
        }
    }
}