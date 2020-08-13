using UniKh.extensions;

namespace UniKh.dataStructure {
    public class V2I {
        public static V2I zero => new V2I(0, 0);
        public static V2I one => new V2I(1, 1);
        public static V2I up => new V2I(0, 1);
        public static V2I down => new V2I(0, -1);
        public static V2I left => new V2I(-1, 0);
        public static V2I right => new V2I(1, 0);

        public V2I(int x = 0, int y = 0) {
            this.x = x;
            this.y = y;
        }

        public V2I(uint col = 0, uint row = 0) : this((int) col, (int) row) { }


        public int Col => x;

        public int Row => y;
        
        public int x = 0;
        public int y = 0;

        public int Magnitude => UnityEngine.Mathf.FloorToInt(UnityEngine.Mathf.Sqrt(SqrMagnitude));

        public int SqrMagnitude => Col * Col + Row * Row;
        
        public int Area00 => Col * Row;

        public int Dot(V2I v) {
            return Col * v.Col + Row * v.Row;
        }

        public int Cross(V2I v) {
            return Col * v.Row - Row * v.Col;
        }
        
        public int Manhattan(V2I v) {
            var disX = v.Col - Col;
            var disY = v.Row - Row;
            return (disX < 0 ? -disX : disX) + (disY < 0 ? -disY : disY);
        }

        public V2I TurnLeft() {
            return new V2I(-Row, Col);
        }

        public V2I TurnRight() {
            return new V2I(Row, -Col);
        }
 
        public double DistanceFromPointToLine(V2I point, V2I pointA, V2I pointB) {
            var v1 = pointB - pointA;
            var v2 = point - pointA;
            var dist = v1.Cross(v2) / v1.Magnitude;
            return UnityEngine.Mathf.Abs(dist);
        }

        public UnityEngine.Vector2 ToVector2() {
            return new UnityEngine.Vector2(Col, Row);
        }

        public UnityEngine.Vector2 ToVector2(uint digit) {
            return new UnityEngine.Vector2(Col / 10.Pow(digit), Row / 10.Pow(digit));
        }

        public UnityEngine.Vector3 ToVector3() {
            return new UnityEngine.Vector3(Col, Row);
        }

        public UnityEngine.Vector3 ToVector3(uint digit) {
            return new UnityEngine.Vector3(Col / 10.Pow(digit), Row / 10.Pow(digit));
        }

        public static implicit operator UnityEngine.Vector2(V2I v) {
            return v.ToVector2();
        }

        public static implicit operator UnityEngine.Vector3(V2I v) {
            return v.ToVector2();
        }

        public static V2I operator *(V2I v, float scale) {
            return new V2I(
                UnityEngine.Mathf.FloorToInt(v.Col * scale),
                UnityEngine.Mathf.FloorToInt(v.Row * scale)
            );
        }

        public static V2I operator /(V2I v, float scale) {
            return new V2I(
                UnityEngine.Mathf.FloorToInt(v.Col / scale),
                UnityEngine.Mathf.FloorToInt(v.Row / scale)
            );
        }

        public static V2I operator +(V2I v1, V2I v2) {
            return new V2I(v1.Col + v2.Col, v1.Row + v2.Row);
        }

        public static V2I operator -(V2I v1, V2I v2) {
            return new V2I(v1.Col - v2.Col, v1.Row - v2.Row);
        }

        public static float operator *(V2I v1, V2I v2) {
            return v1.Col * v2.Col + v1.Row * v2.Row;
        }
    }
}