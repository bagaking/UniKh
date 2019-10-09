using System.Collections.Generic;
using System;
using System.Text;
using UniKh.extensions;

namespace UniKh.utils {
    public static class SGen {

        private static List<StringBuilder> builderPool = new List<StringBuilder>();

        private static Dictionary<int, StringBuilder> builderInUse = new Dictionary<int, StringBuilder>();
        private static int firstAvaliableInd = 0;

        public static int CountInPool { get { return builderPool.Count; } }
        public static int CountInUse { get { return builderInUse.Count; } }

        public static Builder GetBuilder() {
            if (builderPool.Count <= 0) {
                builderPool.QueuePush(new StringBuilder());
            }
            StringBuilder builder = builderPool.QueuePop();
            while (builderInUse.ContainsKey(firstAvaliableInd)) firstAvaliableInd++;
            builderInUse.Add(firstAvaliableInd, builder);
            return new Builder(firstAvaliableInd++);
        }

        public static Builder New { get { return GetBuilder(); } }

        public static Builder Start(string value, params object[] args) {
            if (args == null || args.Length <= 0) return GetBuilder().Append(value);
            else if (value.Exists()) return GetBuilder().AppendFormat(value, args);
            else return GetBuilder();
        }

        public static bool CollectBuilder(int id) {
            if (id < 0 || !builderInUse.ContainsKey(id)) return false;
            StringBuilder _sb = builderInUse[id];
            _sb.Remove(0, _sb.Length);
            builderInUse.Remove(id);
            if (!builderPool.Contains(_sb)) builderPool.QueuePush(_sb);
            if (id < firstAvaliableInd) firstAvaliableInd = id;
            return true;
        }

        public sealed class Builder : IDisposable {
            internal Builder(int _id) { ID = _id; }

            public delegate Setter Setter(object obj);

            ~Builder() { RecycleSelf(); }

            public void Dispose() { RecycleSelf(); }

            private void RecycleSelf() {
                if (ID < 0 || m_stringBuilder == null) return;
                CollectBuilder(ID);
                ID = -1;
            }

            public int ID { get; private set; }

            internal StringBuilder m_stringBuilder {
                get {
                    if (ID < 0) return null;
                    else return builderInUse.TryGet(ID, null);
                }
            }

            public string GetStringAndRelease() {
                string str = ToString();
                if (str != null) RecycleSelf();
                return str;
            }

            public int Length { get { return m_stringBuilder != null ? m_stringBuilder.Length : 0; } }

            public string End {
                get { return GetStringAndRelease(); }
            }

            public string Clear() {
                string ret = ToString();
                Remove(0, Length);
                return ret;
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
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(char[] value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(ulong value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(uint value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(ushort value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(decimal value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(double value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(long value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(int value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(short value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(char value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(float value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(sbyte value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(bool value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(byte value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(string value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value);
                return this;
            }
            public Builder Append(char value, int repeatCount) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value, repeatCount);
                return this;
            }
            public Builder Append(string value, int startIndex, int count) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value, startIndex, count);
                return this;
            }
            public Builder Append(char[] value, int startIndex, int charCount) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Append(value, startIndex, charCount);
                return this;
            }
            public Builder AppendFormat(string format, object arg0) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.AppendFormat(format, arg0);
                return this;
            }
            public Builder AppendFormat(string format, params object[] args) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.AppendFormat(format, args);
                return this;
            }
            public Builder AppendFormat(string format, object arg0, object arg1) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.AppendFormat(format, arg0, arg1);
                return this;
            }
            public Builder AppendFormat(IFormatProvider provider, string format, params object[] args) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.AppendFormat(provider, format, args);
                return this;
            }
            public Builder AppendFormat(string format, object arg0, object arg1, object arg2) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.AppendFormat(format, arg0, arg1, arg2);
                return this;
            }
            public Builder AppendLine() {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.AppendLine();
                return this;
            }
            public Builder AppendLine(string value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.AppendLine(value);
                return this;
            }
            public Builder CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.CopyTo(sourceIndex, destination, destinationIndex, count);
                return this;
            }

            public Builder Insert(int index, ushort value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, object value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, ulong value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, uint value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, decimal value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, sbyte value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, float value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, double value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, bool value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, byte value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, short value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, string value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, char[] value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, int value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, long value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, char value) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value);
                return this;
            }
            public Builder Insert(int index, string value, int count) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value, count);
                return this;
            }
            public Builder Insert(int index, char[] value, int startIndex, int charCount) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Insert(index, value, startIndex, charCount);
                return this;
            }
            public Builder Remove(int startIndex, int length) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Remove(startIndex, length);
                return this;
            }
            public Builder Replace(string oldValue, string newValue) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Replace(oldValue, newValue);
                return this;
            }
            public Builder Replace(char oldChar, char newChar) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Replace(oldChar, newChar);
                return this;
            }
            public Builder Replace(string oldValue, string newValue, int startIndex, int count) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Replace(oldValue, newValue, startIndex, count);
                return this;
            }
            public Builder Replace(char oldChar, char newChar, int startIndex, int count) {
                if (ID < 0 || m_stringBuilder == null) return this;
                m_stringBuilder.Replace(oldChar, newChar, startIndex, count);
                return this;
            }

            public string Get(int startIndex, int length) {
                if (ID < 0 || m_stringBuilder == null) return null;
                return m_stringBuilder.ToString(startIndex, length);
            }

            public override string ToString() {
                if (ID < 0 || m_stringBuilder == null) return null;
                return m_stringBuilder.ToString();
            }

            public int EnsureCapacity(int capacity) {
                if (ID < 0 || m_stringBuilder == null) return -1;
                return m_stringBuilder.EnsureCapacity(capacity);
            }
            public bool Equals(Builder sb) {
                if (ID < 0 || sb == null) return false;
                return sb.Equals(sb.m_stringBuilder);
            }

        }

    }
}
