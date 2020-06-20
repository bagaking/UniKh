using UniKh.extensions;

namespace UniKh.dataStructure {
    public class V2I {
        public static readonly V2I zero = new V2I(0, 0);
        public static readonly V2I one = new V2I(1, 1);
        public static readonly V2I up = new V2I(0, 1);
        public static readonly V2I down = new V2I(0, -1);
        public static readonly V2I left = new V2I(-1, 0);
        public static readonly V2I right = new V2I(1, 0);

        public V2I(int x_ = 0, int y_ = 0) {
            x = x_;
            y = y_;
        }
        
        public V2I(uint x_ = 0, uint y_ = 0) {
            x = (int)x_;
            y = (int)y_;
        }

        public int x = 0;
        public int y = 0;

        public int magnitude {
            get { return UnityEngine.Mathf.FloorToInt(UnityEngine.Mathf.Sqrt(sqrMagnitude)); }
        }

        public int sqrMagnitude {
            get { return x * x + y * y; }
        }

        public int Dot(V2I v) {
            return x * v.x + y * v.y;
        }

        public int Cross(V2I v) {
            return x * v.y - y * v.x;
        }

        public V2I TurnLeft() {
            return new V2I(-y, x);
        }

        public V2I TurnRight() {
            return new V2I(y, -x);
        }

        double DistanceFromPointToLine(V2I point, V2I pointA, V2I pointB) {
            var v1 = pointB - pointA;
            var v2 = point - pointA;
            int dist = v1.Cross(v2) / v1.magnitude;
            return UnityEngine.Mathf.Abs(dist);
        }

        public UnityEngine.Vector2 ToVector2() {
            return new UnityEngine.Vector2(x, y);
        }

        public UnityEngine.Vector2 ToVector2(uint digit) {
            return new UnityEngine.Vector2(x / 10.Pow(digit), y / 10.Pow(digit));
        }

        public UnityEngine.Vector3 ToVector3() {
            return new UnityEngine.Vector3(x, y);
        }

        public UnityEngine.Vector3 ToVector3(uint digit) {
            return new UnityEngine.Vector3(x / 10.Pow(digit), y / 10.Pow(digit));
        }

        public static implicit operator UnityEngine.Vector2(V2I v) {
            return v.ToVector2();
        }

        public static implicit operator UnityEngine.Vector3(V2I v) {
            return v.ToVector2();
        }

        public static V2I operator *(V2I v, float scale) {
            return new V2I(
                UnityEngine.Mathf.FloorToInt(v.x * scale),
                UnityEngine.Mathf.FloorToInt(v.y * scale)
            );
        }

        public static V2I operator /(V2I v, float scale) {
            return new V2I(
                UnityEngine.Mathf.FloorToInt(v.x / scale),
                UnityEngine.Mathf.FloorToInt(v.y / scale)
            );
        }

        public static V2I operator +(V2I v1, V2I v2) {
            return new V2I(v1.x + v2.x, v1.y + v2.y);
        }

        public static V2I operator -(V2I v1, V2I v2) {
            return new V2I(v1.x - v2.x, v1.y - v2.y);
        }

        public static float operator *(V2I v1, V2I v2) {
            return v1.x * v2.x + v1.y * v2.y;
        }
    }
}