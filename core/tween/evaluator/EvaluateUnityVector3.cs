/** EvaluateUnityVector3.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/16 10:21:48
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */
 
using UnityEngine;

namespace UniKh.core.tween {
    
    public class EvaluateUnityVector3 : TweenEvaluator<Vector3, EvaluateUnityVector3> {
        
        public override Vector3 Evaluate(Vector3 from, Vector3 to, float evaluatePos) {
            var offset = to - from;
            return from + offset * evaluatePos;
        }
    }

    public class EvaluateUnityVector3RotateTowards : TweenEvaluator<Vector3, EvaluateUnityVector3RotateTowards> {

        public override Vector3 Evaluate(Vector3 from, Vector3 to, float evaluatePos) {
            var radius = Vector3.Angle(from, to) * Mathf.Deg2Rad;
            return Vector3.RotateTowards(from, to, evaluatePos * radius, 0f);
        }
    }
}