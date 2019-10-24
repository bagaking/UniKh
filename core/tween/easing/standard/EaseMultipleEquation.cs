/** EaseMultipleEquation.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/18 11:50:53
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */
 
using UnityEngine; 

namespace UniKh.core.tween {

    // Cuz Math.pow is significantly slower than direct multiplication, so it is generally not recommended
    public abstract class EaseMultipleEquation<T> : StandardEasing<T> where T : EaseMultipleEquation<T>, new() { 

        public abstract float Power { get; }

        public override float EaseIn(float x) {
            return Mathf.Pow(x, Power); // equal to return (float) Math.Pow((double) f, (double) p);   
        }

        public override float EaseOut(float x) {
            return 1 - Mathf.Pow(1 - x, Power); 
        }
    }
    
    
    public class EaseQuad : StandardEasing<EaseQuad> {
        public override float EaseIn(float x) {
            return x * x;
        }

        public override float EaseOut(float x) {
            return 1 - (x = 1 - x) * x;
        }
    }
    
    public class EaseCubic : StandardEasing<EaseCubic> {
        public override float EaseIn(float x) {
            return x * x * x;
        }

        public override float EaseOut(float x) {
            return 1 - (x = 1 - x) * x * x;
        }
    }
    
    public class EaseQuart : StandardEasing<EaseQuart> {
        public override float EaseIn(float x) {
            return x * x * x * x;
        }

        public override float EaseOut(float x) {
            return 1 - (x = 1 - x) * x * x * x;
        }
    }
    
    public class EaseQuint : StandardEasing<EaseQuint> {
        public override float EaseIn(float x) {
            return x * x * x * x * x;
        }

        public override float EaseOut(float x) {
            return 1 - (x = 1 - x) * x * x * x * x;
        }
    } 
    
}
