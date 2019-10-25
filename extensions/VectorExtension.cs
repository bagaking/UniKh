using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniKh.extensions {
    
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class Vector2PlateAttribute : PropertyAttribute {
        
        public readonly Vector2 minValue = Vector2.zero;
        public readonly Vector2 maxValue = Vector2.zero;
        public Vector2PlateAttribute() { }

        public Vector2PlateAttribute(float xMin, float yMin, float xMax, float yMax) {
            this.minValue = new Vector2(xMin, yMin);
            this.maxValue = new Vector2(xMax, yMax);
        }
    }
    
    public static class VectorExtension {
        public static Vector2 AddRandom(this Vector2 v, float xRange, float yRange) {
            return new Vector2(
                v.x + Random.Range(-xRange, xRange),
                v.y + Random.Range(-yRange, yRange)
            );
        }

        public static Vector3 AddRandom(this Vector3 v, float xRange, float yRange, float zRange) {
            return new Vector3(
                xRange > 0 ? v.x + Random.Range(-xRange, xRange) : v.x,
                yRange > 0 ? v.y + Random.Range(-yRange, yRange) : v.y,
                zRange > 0 ? v.z + Random.Range(-zRange, zRange) : v.z
            );
        }

        public static float Dot(this Vector2 v, Vector2 vt) { return Vector2.Dot(v, vt); }

        public static float Cross(this Vector2 v, Vector2 vt) { return v.x * vt.y - v.y * vt.x; }

        [System.Flags]
        public enum DistanceToSegOption : byte {
            Line = 0,
            IncludePointFrom = 0x01,
            IncludePointTo = 0x02,
        }

        public static float DistanceToSeg(
            this Vector2 point,
            Vector2 lineFrom,
            Vector2 lineTo,
            DistanceToSegOption seg = DistanceToSegOption.Line) {
            var v0 = lineTo - lineFrom;
            var vt = point - lineFrom;

            if (seg != DistanceToSegOption.Line) {
                var d = Vector2.Dot(v0, vt);
                if (d < 0)
                    return (seg | DistanceToSegOption.IncludePointFrom) > 0 ? vt.magnitude : float.PositiveInfinity;
                if (d > v0.magnitude)
                    return (seg | DistanceToSegOption.IncludePointTo) > 0
                        ? Vector2.Distance(point, lineTo)
                        : float.PositiveInfinity;
            }

            var dist = v0.Cross(vt) / v0.magnitude; // |vt||v0|sin / |v0|
            return UnityEngine.Mathf.Abs(dist);
        }

        public static bool IsZero(this Vector2 point) => point == Vector2.zero;
//  public static float FastLength(this Vector2 point)
//  {
//// this function computes the distance from 0,0 to x,y with 3.5% error
// 
//// fist compute the absolute value of x,y
//    var x = Mathf.Abs(point.x);
//    var y = Mathf.Abs(point.y);
// 
//// compute the minimum of x,y
//    float mn = Mathf.Min(x, y);
//    int 
// 
//// return the distance
//    return (x + y - (mn >> 1) - (mn >> 2) + (mn >> 4));
// 
//  }
    }
}