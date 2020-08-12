using System;
using UnityEngine;

namespace UniKh.coordinate {
    
    [Serializable]
    public class Plane3D {
        public Vector3 origin = Vector3.zero;
        public Vector3 normal = Vector3.up;

        public Vector3 IntersectWithRay(Ray ray) {
            var baseFactor = Vector3.Dot(normal, ray.direction);
            if (Mathf.Abs(baseFactor) <= float.Epsilon) {
                return Vector3.positiveInfinity;
            }

            var t = -Vector3.Dot(normal, (ray.origin - origin)) / baseFactor;
            return (ray.origin + t * ray.direction);
        }

        public Vector3 IntersectWithRay(Vector3 rayOrigin, Vector3 rayDirection) {
            var baseFactor = Vector3.Dot(normal, rayDirection);
            if (Mathf.Abs(baseFactor) <= float.Epsilon) {
                return Vector3.positiveInfinity;
            }

            var t = -Vector3.Dot(normal, (rayOrigin - origin)) / baseFactor;
            return (rayOrigin + t * rayDirection);
        }

        public Vector3 Projection(Vector3 p) {
            var n = normal;

            var xx = normal.x * normal.x;
            var xy = normal.x * normal.y;
            var xz = normal.x * normal.z;
            var yy = normal.y * normal.y;
            var zz = normal.z * normal.z;
            var yz = normal.y * normal.z;
            var xXyYzZ = xx + yy + zz;

            if (Mathf.Abs(xXyYzZ) <= float.Epsilon) {
                return Vector3.positiveInfinity;
            }

            return new Vector3(
                (xy * origin.y + yy * p.x - xy * p.y + xz * origin.z + zz * p.x - xz * p.z + xx * origin.x) / xXyYzZ,
                (yz * origin.z + zz * p.y - yz * p.z + xy * origin.x + xx * p.y - xy * p.x + yy * origin.y) / xXyYzZ,
                (xz * origin.x + xx * p.z - xz * p.x + yz * origin.y + yy * p.z - yz * p.y + zz * origin.z) / xXyYzZ
            );
        }
    }
}