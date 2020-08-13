using System;
using UniKh.utils;
using UnityEngine;

namespace UniKh.space {
    [Serializable]
    public class LineSeg3D {
        public Vector3 origin;
        public Vector3 to;

        public Vector3 Direction => to - origin;

        public override string ToString() {
            return SGen.New["LineSeg3D("][origin][to][')'].End;
        }
    }
}