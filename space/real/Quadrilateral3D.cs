using UniKh.utils;
using UnityEngine;

namespace UniKh.coordinate {
   
    [System.Serializable]
    public class Quadrilateral3D {
        public Vector3 lb;
        public Vector3 rt;
        public Vector3 lt;
        public Vector3 rb;

        public override string ToString() {
            return SGen.New["Quad3D("][lb][lt][rb][rt][')'].End;
        }

        public Vector3 Ct => (lt + rt) / 2;

        public Vector3 Cb => (lb + rb) / 2;

        public Vector3 Cl => (lb + lt) / 2;

        public Vector3 Cr => (rb + rt) / 2;
        
        public Vector3 Cc => (lb + rb + lt + rt) / 4;

        public LineSeg3D CulcSegmentRateL2R(float rateX) {
            return new LineSeg3D {origin = Vector3.Lerp(lb, rb, rateX), to = Vector3.Lerp(lt, rt, rateX)};
        }

        public LineSeg3D CulcSegmentRateB2T(float rateY) {
            return new LineSeg3D {origin = Vector3.Lerp(lb, lt, rateY), to = Vector3.Lerp(rb, rt, rateY)};
        }

        public Vector3 CulcRate(Vector2 rate) {
            return CulcRate(rate.x, rate.y);
        }

        public Vector3 CulcRate(float rateL2R, float rateB2T) {
            if (Mathf.Abs(rateL2R - 0.5f) < 0.0001f) { // 阈值还可以再大, 但在一些逻辑中可能导致突变的情况
                return Mathf.Abs(rateB2T - 0.5f) < 0.0001f ? Cc : Vector3.Lerp(Cb, Ct, rateB2T);
            }

            if (Mathf.Abs(rateB2T - 0.5f) < 0.0001f) { // 阈值还可以再大, 但在一些逻辑中可能导致突变的情况
                return Vector3.Lerp(Cb, Ct, rateB2T);
            }

            var l0 = CulcSegmentRateL2R(rateL2R);
            var l1 = CulcSegmentRateB2T(rateB2T);
            Vector3 a = l0.origin, b = l0.to, c = l1.origin, d = l1.to;
            var intersection = Vector3.zero; // consider 3 dir
            intersection.x = ((a.x - b.x) * (c.x * d.z - d.x * c.z) - (c.x - d.x) * (a.x * b.z - b.x * a.z)) /
                             ((c.x - d.x) * (a.z - b.z) - (a.x - b.x) * (c.z - d.z));
            intersection.z = ((a.z - b.z) * (c.x * d.z - d.x * c.z) - (c.z - d.z) * (a.x * b.z - b.x * a.z)) /
                             ((c.x - d.x) * (a.z - b.z) - (a.x - b.x) * (c.z - d.z));

            return intersection;
        }
    }
}