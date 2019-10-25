/** == EaseBack.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/24 16:00:03
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

namespace UniKh.core.tween {
    public class EaseBack : StandardEasing<EaseBack> {
        private float n = 2.5f;

        public override float EaseIn(float x) {
            return x * x * ((1 + n) * x - n);
        }

        public override float EaseOut(float x) {
            return 1 - (x = 1 - x) * x * ((1 + n) * x - n);
        }
    }
}