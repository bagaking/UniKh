/** EaseMultipleEquation.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/18 11:50:53
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
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
    public class EaseBack : StandardEasing<EaseBack>
    {
        private float n = 2.5f;
        public override float EaseIn(float x)
        {
            return x * x * ((1 + n) * x - n);
        }

        public override float EaseOut(float x)
        {
            return 1 - (x = 1 - x) * x * ((1 + n) * x - n);
        }
    } 
    public class EaseElastic : StandardEasing<EaseElastic>
    {
        private float n = 5f;
        public override float EaseIn(float x)
        {
            return Mathf.Pow(2, 10 * (x - 1)) * Mathf.Sin((2 * n + 0.5f) * Mathf.PI * x);
        }

        public override float EaseOut(float x)
        {
            return 1 - Mathf.Pow(2, 10 * ((x = 1 - x) - 1)) * Mathf.Sin((2 * n + 0.5f) * Mathf.PI * x);
        }
    } 
    public class EaseBounce : StandardEasing<EaseBounce>
    {
        private float d1 = 2.75f;
        private float n1 = 7.5625f;
        public override float EaseIn(float x)
        {
            return 1 - BounceOut(1 - x);
        }

        public override float EaseOut(float x)
        {
            return BounceOut(x);
        }

        float BounceOut(float x)
        {
            if(x < 1/d1){

                return n1*x*x;

            } else if(x < 2/d1){

                return n1*(x-=(1.5f/d1))*x + .75f;

            } else if(x < 2.5/d1){

                return n1*(x-=(2.25f/d1))*x + .9375f;

            } else {

                return n1*(x-=(2.625f/d1))*x + .984375f;

            }
        }
    } 
}
