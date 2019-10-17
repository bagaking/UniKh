/** EaseSine.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/17 08:05:12
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UnityEngine;

namespace UniKh.core.tween {
    
    public class EaseSine: StandardEasing<EaseSine> {
         
        public override float EaseIn(float x) {
            return 1f - Mathf.Cos(x * Mathf.PI / 2f);
        }

        public override float EaseOut(float x) {
            return Mathf.Sin(x * Mathf.PI / 2f);
        }
    }
}