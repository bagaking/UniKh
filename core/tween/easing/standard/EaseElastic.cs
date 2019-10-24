/** == EaseElastic.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/24 15:58:44
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;

namespace UniKh.core.tween {
    public class EaseElastic : StandardEasing<EaseElastic> {
        private float n = 5f;

        public override float EaseIn(float x) {
            return Mathf.Pow(2, 10 * (x - 1)) * Mathf.Sin((2 * n + 0.5f) * Mathf.PI * x);
        }

        public override float EaseOut(float x) {
            return 1 - Mathf.Pow(2, 10 * ((x = 1 - x) - 1)) * Mathf.Sin((2 * n + 0.5f) * Mathf.PI * x);
        }
    }
}