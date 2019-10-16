/** EaseBezier.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 19:00:02
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;

namespace UniKh.core.tween {
    
    public class EaseBezier: Easing {

        public CubicBezier curve;
        
        public EaseBezier(Vector2 handle1, Vector2 handle2) {
            curve = new CubicBezier(handle1, handle2);
        }
        
        public EaseBezier(CubicBezier curve) {
            this.curve = curve;
        }
        
        public override float Convert(float convert) {
            return curve.Evaluate(convert);
        }
    }
}