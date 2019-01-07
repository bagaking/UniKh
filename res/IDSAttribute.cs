using System;

namespace UniKh.utils {
    [AttributeUsage(System.AttributeTargets.Enum)]
    public class IdsAttribute : Attribute {
        public uint SegStart;
        public uint SegEnd;

        public IdsAttribute(uint segStart, uint segEnd) {
            SegStart = segStart;
            SegEnd = segEnd;
        }
    }
}