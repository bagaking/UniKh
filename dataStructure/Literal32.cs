/** Literal32.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2020/04/07 12:00:00
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.extensions;

namespace UniKh.dataStructure {
    
    [Serializable]
    public struct Literal32 : IFormattable, IComparable, IComparable<Literal32>, IEquatable<Literal32> {
        public const uint MaskL = 0xFFFFFFFF / (0x2 + 1);
        public const uint MaskH = MaskL << 1;
        public static readonly uint RanL;
        public static readonly uint RanH;

        static Literal32() {
            var r = new Random((int) DateTime.Now.GetTimeStamp());
            var salt = r.Next(0, int.MaxValue);
            RanL = (uint) (MaskL & salt);
            RanH = (uint) (MaskH & salt);
        }

        public uint l;
        public uint h;

        public uint Raw {
            set {
                l = value & MaskL | RanH;
                h = value & MaskH | RanL;
            }
            get => (l & MaskL) | (h & MaskH);
        }

        public int Val {
            set => Raw = unchecked((uint) value);
            get => unchecked((int) Raw);
        }

        public uint RawL => (l & MaskL);
        public uint RawH => (h & MaskH);

        public bool Equals(Literal32 other) {
            return RawL == other.RawL && RawH == other.RawH;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj.GetType() == GetType() && Equals((Literal32) obj);
        }

        public override int GetHashCode() {
            return unchecked((int) ((RawL * 397) ^ RawH));
        }

        public int CompareTo(Literal32 other) {
            var ret = unchecked(((int) RawH).CompareTo((int) other.RawH));
            return ret == 0 ? RawL.CompareTo(other.RawL) : ret;
        }

        public int CompareTo(object obj) {
            if (ReferenceEquals(null, obj)) return 1;
            return obj.GetType() == GetType() ? CompareTo((Literal32) obj) : Val.CompareTo(obj);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            return Val.ToString(format, formatProvider);
        }

        public override string ToString() {
            return Val.ToString();
        }

        public static implicit operator int(Literal32 literal) {
            return literal.Val;
        }

        public static implicit operator Literal32(int val) { 
            return new Literal32 { Val = val }; 
        }

        // only for storage usage
        //
        // public static Literal32 operator *(Literal32 literal, int val) {
        //     return literal.Val *= val;
        // } 
        //
        // public static Literal32 operator +(Literal32 literal, int val) {
        //     return literal.Val += val;
        // }
        //
        // public static Literal32 operator -(Literal32 literal, int val) {
        //     return literal.Val -= val;
        // }
    }
}