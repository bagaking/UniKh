using System;
using UnityEngine;

namespace UniKh.utils {
    [AttributeUsage(System.AttributeTargets.Enum)]
    public class IdsAttribute : PropertyAttribute {
        public uint SegStart;
        public uint SegEnd;

        public IdsAttribute(uint segStart, uint segEnd) {
            SegStart = segStart;
            SegEnd = segEnd;
        }
    }
    
    [AttributeUsage(System.AttributeTargets.Field)]
    public class CrIdAttribute : PropertyAttribute {
        public uint SegStart; 

        public CrIdAttribute(uint segStart) {
            SegStart = segStart; 
        }
    }
}