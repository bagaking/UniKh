/** EaseExpo.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/18 12:15:22
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine; 

namespace UniKh.core.tween {
 
    public class EaseExpo : StandardEasing<EaseExpo> {
        public override float EaseIn(float x) {
            return x <= 0f ? 0f : Mathf.Pow(2, 10 * (x - 1));
        }

        public override float EaseOut(float x) {
            return x >= 1f ? 1f : 1 - Mathf.Pow(2, - 10 * x);
        }
    }
}
