using UniKh.utils;
using UniKh.utils.Inspector;
using UnityEngine;

namespace UniKh.coordinate {
    
    [System.Serializable]
    public class AreaXnZ {
        
        public Vector3 center;

        [Vector2Range(XMax = float.MaxValue, YMax = float.MaxValue)]
        public Vector2 halfSize;
        
        public Vector3 LB => new Vector3(center.x - halfSize.x, 0, center.z - halfSize.y);
        public Vector3 RT => new Vector3(center.x + halfSize.x, 0, center.z + halfSize.y);
        public Vector3 LT => new Vector3(center.x - halfSize.x, 0, center.z + halfSize.y);
        public Vector3 RB => new Vector3(center.x + halfSize.x, 0, center.z - halfSize.y);
        public Vector3 CB => new Vector3(center.x, 0, center.z - halfSize.y);
        public Vector3 CT => new Vector3(center.x, 0, center.z + halfSize.y);
        
        public Vector3 RandomPos3D() {
            return new Vector3(
                center.x + Random.Range(-halfSize.x, halfSize.x),
                center.y,
                center.z + Random.Range(-halfSize.y, halfSize.y)
            );
        }
        
        public override string ToString() {
            return SGen.New["Area("][center][halfSize][")"].End;
        }
    }
}