/** Easing.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 18:48:51
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniKh.core.tween {

    
    public abstract class Easing {

        public abstract float Convert(float x);
        
        public List<Vector2> GetSample(int split) {
            if (split <= 0) return null;
            var distance = 1f / split;
            var ret = new List<Vector2>();
            for (var i = 0; i < split; i++) {
                var xPos = i * distance;
                ret.Add(new Vector2(xPos, Convert(xPos)));
            }
            ret.Add(new Vector2(1, Convert(1)));
            return ret;
        }
    }

    public abstract class Easing<TEase> : Easing where TEase : Easing<TEase>, new() {
        
        public static readonly TEase Default = new TEase();
    }

    /// <summary>
    /// Easings Reference
    /// </summary>
    /// <see cref="https://easings.net/">Easings Reference</see>
    public abstract class StandardEasing<TEase> : Easing<TEase> where TEase : StandardEasing<TEase>, new() {

        
        public enum Method {
            In,
            Out,
            InOut
        }
        
        public static readonly TEase DefaultInOut = new TEase().SetMethod(Method.InOut);
        public static readonly TEase DefaultIn = new TEase().SetMethod(Method.In);
        public static readonly TEase DefaultOut = new TEase().SetMethod(Method.Out);
        
        public Method method = Method.InOut;

        public TEase SetMethod(Method m) {
            this.method = m;
            return this as TEase;
        }

        public override float Convert(float x) {
            switch (method) {
                case Method.In:
                    return EaseIn(x);
                case Method.Out:
                    return EaseOut(x);
                case Method.InOut:
                    return -0.5f * ((float)Math.Cos((float)Math.PI * x) - 1f);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public abstract float EaseIn(float x);
        
        public abstract float EaseOut(float x);

        public float EaseInOut(float x) {
            if (x < 0.5f) {
                return EaseIn(x * 2) / 2;
            }
            else {
                return 0.5f + EaseOut(x * 2 - 1) / 2;
            }
        } 
    }
}