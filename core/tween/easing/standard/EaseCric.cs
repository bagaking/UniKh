/** EaseCrit.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/18 12:15:41
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UnityEngine; 

namespace UniKh.core.tween {
 
    public class EaseCric : StandardEasing<EaseCric> {
        public override float EaseIn(float x) {
            return 1f - Mathf.Sqrt(1 - x * x);
        }

        public override float EaseOut(float x) {
            return Mathf.Sqrt(1 - (x = 1 - x) * x);
        }
    }
}

