using System;
using UnityEngine;

namespace UniKh.utils.Inspector {
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class BtnAttribute : PropertyAttribute {
        public string Name;
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class CompAttribute : PropertyAttribute { }

    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class Vector2RangeAttribute : PropertyAttribute {
        public float XMin = 0;
        public float XMax = 1;
        public float YMin = 0;
        public float YMax = 1;

        public Vector2RangeAttribute(): base() {    
        }
        
        public Vector2RangeAttribute(float xMax, float yMax): this() {
            XMax = xMax;
            YMax = yMax;
        }
        
        public Vector2RangeAttribute(float xMin, float yMin, float xMax, float yMax) : this(xMax, yMax) {
            XMin = xMin;
            YMin = yMin;
        }
        
        public float RateX(float x) {
            if (float.IsInfinity(XMax)) return 0;
            var distance = XMax - XMin;
            if (Math.Abs(distance) < float.Epsilon) return 1;
            return (x - XMin) / distance;
        }
        
        public float RateY(float y) {
            if (float.IsInfinity(YMax)) return 0;
            var distance = YMax - YMin;
            if (Math.Abs(distance) < float.Epsilon) return 1;
            return (y - YMin) / distance;
        }
        
    }
}