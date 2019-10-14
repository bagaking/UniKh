/** EvaluateFloat.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 21:15:35
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */
 
using UnityEngine;

namespace UniKh.core.tween {
    public class EvaluateFloat : TweenEvaluator<float, EvaluateFloat> {
        public override float Evaluate(float from, float to, float evaluatePos) {
            return Mathf.Lerp(from, to, evaluatePos);
        }
    }
}