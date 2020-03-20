using System.Collections.Generic;
using System;
using System.Text;
using UniKh.extensions;

namespace UniKh.utils {
    public static class SGen {

        private static readonly List<StringBuilder> BuilderPool = new List<StringBuilder>(); // maybe priority queue can be used here to avoid allocation of memory

        private static readonly Dictionary<int, StringBuilder> BuilderInUse = new Dictionary<int, StringBuilder>();
        private static int _firstAvailableInd = 0;
        
        private static readonly object MutexInd = new object(); 

        public static int CountInPool => BuilderPool.Count;
        public static int CountInUse => BuilderInUse.Count;

        public static Builder GetBuilder() {
            if (BuilderPool.Count <= 0) {
                BuilderPool.QueuePush(new StringBuilder());
            }
            int preferIndex;
            StringBuilder builder;
            lock(MutexInd) {
                if (_firstAvailableInd == int.MaxValue) {
                    _firstAvailableInd = 0;
                }
                while (BuilderInUse.ContainsKey(_firstAvailableInd)) _firstAvailableInd++;
                preferIndex = _firstAvailableInd++;
                builder = BuilderPool.QueuePop();
            } 
            BuilderInUse.Add(preferIndex, builder);
            return new Builder(preferIndex);
        }

        /// <summary>
        /// Get new anonymous builder
        /// </summary>
        public static Builder New => GetBuilder();

        public static Builder Start(string value, params object[] args) {
            if (args == null || args.Length <= 0) return GetBuilder().Append(value);
            else if (value.Exists()) return GetBuilder().AppendFormat(value, args);
            else return GetBuilder();
        }

        internal static bool Collect(int id) {
            if (id < 0 || !BuilderInUse.ContainsKey(id)) return false; // has already been collected 
            var sb = BuilderInUse[id];
            sb.Remove(0, sb.Length); 
            BuilderInUse.Remove(id);
            if (!BuilderPool.Contains(sb)) BuilderPool.QueuePush(sb);
            if (id < _firstAvailableInd) _firstAvailableInd = id;
            return true;
        }

        /// <summary>
        /// SGen.Builder is a reusable, leak-proof string concatenation tool
        /// </summary>
        public sealed class Builder : IDisposable {
            internal Builder(int id) { Id = id; }

            public delegate Setter Setter(object obj);

            ~Builder() { RecycleSelf(); }

            public void Dispose() { RecycleSelf(); }

            private void RecycleSelf() {
                if (Id < 0 || MStringBuilder == null) return;
                Collect(Id);
                Id = -1;
            }

            public int Id { get; private set; }

            internal StringBuilder MStringBuilder => Id < 0 ? null : BuilderInUse.TryGet(Id, null);

            public string GetStringAndRelease() {
                var str = ToString();
                RecycleSelf();
                return str;
            }

            public int Length => MStringBuilder?.Length ?? 0;

            /// <summary>
            /// Returns the concatenation results and release the related string builder
            /// </summary>
            public string End => GetStringAndRelease();

            public Builder Clear() { 
                Remove(0, Length);
                return this;
            }

            public Builder this[object value] { get { return this.Append(value); } }
            public Builder this[char[] value] { get { return this.Append(value); } }
            public Builder this[ulong value] { get { return this.Append(value); } }
            public Builder this[uint value] { get { return this.Append(value); } }
            public Builder this[ushort value] { get { return this.Append(value); } }
            public Builder this[decimal value] { get { return this.Append(value); } }
            public Builder this[double value] { get { return this.Append(value); } }
            public Builder this[long value] { get { return this.Append(value); } }
            public Builder this[int value] { get { return this.Append(value); } }
            public Builder this[short value] { get { return this.Append(value); } }
            public Builder this[char value] { get { return this.Append(value); } }
            public Builder this[float value] { get { return this.Append(value); } }
            public Builder this[sbyte value] { get { return this.Append(value); } }
            public Builder this[bool value] { get { return this.Append(value); } }
            public Builder this[byte value] { get { return this.Append(value); } }
            public Builder this[string value] { get { return this.Append(value); } }
            //=====================

            public Builder Append(object value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(char[] value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(ulong value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(uint value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(ushort value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(decimal value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(double value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(long value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(int value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(short value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(char value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(float value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(sbyte value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(bool value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(byte value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(string value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value);
                return this;
            }
            public Builder Append(char value, int repeatCount) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value, repeatCount);
                return this;
            }
            public Builder Append(string value, int startIndex, int count) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value, startIndex, count);
                return this;
            }
            public Builder Append(char[] value, int startIndex, int charCount) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Append(value, startIndex, charCount);
                return this;
            }
            public Builder AppendFormat(string format, object arg0) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.AppendFormat(format, arg0);
                return this;
            }
            public Builder AppendFormat(string format, params object[] args) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.AppendFormat(format, args);
                return this;
            }
            public Builder AppendFormat(string format, object arg0, object arg1) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.AppendFormat(format, arg0, arg1);
                return this;
            }
            public Builder AppendFormat(IFormatProvider provider, string format, params object[] args) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.AppendFormat(provider, format, args);
                return this;
            }
            public Builder AppendFormat(string format, object arg0, object arg1, object arg2) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.AppendFormat(format, arg0, arg1, arg2);
                return this;
            }
            public Builder AppendLine() {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.AppendLine();
                return this;
            }
            public Builder AppendLine(string value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.AppendLine(value);
                return this;
            }
            public Builder CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.CopyTo(sourceIndex, destination, destinationIndex, count);
                return this;
            }

            public Builder Insert(int index, ushort value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, object value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, ulong value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, uint value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, decimal value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, sbyte value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, float value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, double value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, bool value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, byte value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, short value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, string value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, char[] value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, int value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, long value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, char value) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, string value, int count) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value, count);
                return this;
            }
            public Builder Insert(int index, char[] value, int startIndex, int charCount) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Insert(index, value, startIndex, charCount);
                return this;
            }
            public Builder Remove(int startIndex, int length) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Remove(startIndex, length);
                return this;
            }
            public Builder Replace(string oldValue, string newValue) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Replace(oldValue, newValue);
                return this;
            }
            public Builder Replace(char oldChar, char newChar) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Replace(oldChar, newChar);
                return this;
            }
            public Builder Replace(string oldValue, string newValue, int startIndex, int count) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Replace(oldValue, newValue, startIndex, count);
                return this;
            }
            public Builder Replace(char oldChar, char newChar, int startIndex, int count) {
                if (Id < 0 || MStringBuilder == null) return this;
                MStringBuilder.Replace(oldChar, newChar, startIndex, count);
                return this;
            }

            public string Get(int startIndex, int length) {
                if (Id < 0 || MStringBuilder == null) return null;
                return MStringBuilder.ToString(startIndex, length);
            }

            public override string ToString() {
                if (Id < 0 || MStringBuilder == null) return "";
                return MStringBuilder.ToString();
            }

            public int EnsureCapacity(int capacity) {
                if (Id < 0 || MStringBuilder == null) return -1;
                return MStringBuilder.EnsureCapacity(capacity);
            }
            public bool Equals(Builder builder) {
                if (Id < 0 || builder == null) return false;
                return builder.MStringBuilder.Equals(builder.MStringBuilder);
            }

        }

    }
}
