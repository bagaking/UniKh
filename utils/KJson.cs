/** == KJson.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/29 01:20:41
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.core;
using UniKh.extensions;
using UniKh.utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.Utilities;
using UnityEngine;

namespace UniKh.utils {
    public class KJson {
        public static readonly KJson Empty = new KJson();

        public virtual bool isList { get; protected set; }

        public Dictionary<string, object> Data {
            get {
                if (data == null) data = new Dictionary<string, object>();
                return data;
            }
        }

        public List<object> ListData {
            get {
                if (seqData == null) {
                    seqData = new List<object>();
                }

                return seqData;
            }
        }

        protected Dictionary<string, object> data = null;
        protected List<object> seqData = null;

        public int Count {
            get { return isList ? ListData.Count : Data.Count; }
        }

        public KJson Add(string name, object value) {
            if (HasChild(name)) throw new KJsonException("[Json Add] the Name (" + name + ") is already exist. ");
            Data[name] = value;
            return this;
        }

        public KJson Add(object value) {
            isList = true;
            ListData.Add(value);
            return this;
        }

        public bool Del(string key) {
            if (isList) return false;
            if (data.ContainsKey(key)) {
                data.Remove(key);
                return true;
            }

            return false;
        }

        public bool Del(int ind) {
            if (!isList) return false;
            if (isList && seqData.Count > ind) {
                ListData.RemoveAt(ind);
                return true;
            }

            return false;
        }

        public virtual object GetByKey(string key) {
            if (!Data.ContainsKey(key)) throw new KJsonException("[Json GetByKey] key(" + key + ") is out of range. ");
            return Data[key];
        }

        public virtual object this[string key] {
            get { return GetByKey(key); }
            protected set { Data[key] = value; }
        }


        public virtual object GetByIndex(int index) {
            //if(index < 0 || index > Count) throw new JsonException("[Json GetByIndex] index(" + index + ") is out of range. "); //这种判断其实很影响性能
            return ListData[index];
        }

        public virtual object this[int index] {
            get { return GetByIndex(index); }
            protected set { ListData[index] = value; }
        }

        public KJson CreateChild(string key, bool _isList = false) {
            if (HasChild(key)) throw new KJsonException("[Json CreateChild] the key (" + key + ") is already exist. ");
            KJson j = new KJson();
            Add(key, j);
            j.isList = _isList;
            return j;
        }

        public KJson CreateListChild() {
            KJson j = new KJson();
            Add(j);
            return j;
        }

        protected KJson() {
            isList = false;
        }

        public static KJson Create() {
            return new KJson();
        }

        public static KJson Create(string code) {
            return Deserializer.Deserialize(code) as KJson;
        }

        public static KJson Create(Dictionary<string, object> _data) {
            return new KJson() {data = _data};
        }

        public static KJson Create(List<object> _seqData) {
            return new KJson() {seqData = _seqData, isList = true};
        }

        public static implicit operator KJson(Dictionary<string, object> _data) {
            return Create(_data);
        }

        public static implicit operator KJson(List<object> _data) {
            return Create(new List<object>(_data));
        }

        public KJson DoMap(Action<string, object> Executor) {
            if (Executor == null) throw new KJsonException("[Json DoMap] empty executor");
            if (isList) throw new KJsonException("[Json DoMap] cannot apply DoMap to a seq json");
            else
                foreach (var pair in Data)
                    Executor(pair.Key, pair.Value);
            return this;
        }

        public KJson DoList(Action<int, object> Executor) {
            if (Executor == null) throw new KJsonException("[Json DoValue] empty executor");
            if (isList)
                for (int i = 0; i < ListData.Count; i++) {
                    Executor(i, ListData[i]);
                }
            else throw new KJsonException("[Json DoMap] cannot apply DoList to a map json");

            return this;
        }

        public virtual bool HasChild(string key) {
            return Data.ContainsKey(key);
        }

//        public object[] Keys {
//            get {
//                if(isList) {
//                    var _keys = new List<object>().ChangeLength(ListData.Count);
//                    for(var i = 0; i < ListData.Count; i++) _keys[i] = i;
//                    return _keys.ToArray();
//                } else {
//                    return Data.Keys.ToArray();
//                }
//            }
//        }


        public virtual object Get(params object[] keys) {
            object m = this;
            for (int i = 0; i < keys.Length; i++) {
                if (keys[i] is string) {
                    m = (m as KJson)[keys[i] as string];
                }
                else if (keys[i] is int) {
                    m = (m as KJson)[(int) keys[i]];
                }
                else throw new KJsonException("[Json Get] wrong input for key " + i + " " + keys[i]);
            }

            return m;
        }

        public virtual bool GetBool(params object[] keys) {
            return KJsonUtility.ObjectToBool(Get(keys));
        }

        public virtual int GetInt(params object[] keys) {
            return KJsonUtility.ObjectToInt(Get(keys));
        }

        public virtual int GetByte(params object[] keys) {
            return (byte) KJsonUtility.ObjectToInt(Get(keys));
        }

        public virtual long GetLong(params object[] keys) {
            return KJsonUtility.ObjectToLong(Get(keys));
        }

        public virtual float GetFloat(params object[] keys) {
            return KJsonUtility.ObjectToFloat(Get(keys));
        }

        public virtual double GetDouble(params object[] keys) {
            return KJsonUtility.ObjectToDouble(Get(keys));
        }

        public virtual string GetString(params object[] keys) {
            return KJsonUtility.ObjectToString(Get(keys));
        }

        public KJson ChildByIndex(int index) {
            return ListData.Json(index);
        }

        public virtual bool BoolByIndex(int index) {
            return ListData.Bool(index);
        }

        public virtual int IntByIndex(int index) {
            return ListData.Int(index);
        }

        public virtual byte ByteByIndex(int index) {
            return (byte) ListData.Int(index);
        }

        public virtual long LongByIndex(int index) {
            return ListData.Long(index);
        }

        public virtual float FloatByIndex(int index) {
            return ListData.Float(index);
        }

        public virtual double DoubleByIndex(int index) {
            return ListData.Double(index);
        }

        public virtual string StringByIndex(int index, string format) {
            return ListData.String(index, format);
        }

        public virtual string StringByIndex(int index) {
            return ListData.String(index, null);
        }

        public KJson Child(string key) {
            return Data.Json(key);
        }

        public virtual bool Bool(string key) {
            return Data.Bool(key);
        }

        public virtual int Int(string key) {
            return Data.Int(key);
        }

        public virtual byte Byte(string key) {
            return (byte) Data.Int(key);
        }

        public virtual long Long(string key) {
            return Data.Long(key);
        }

        public virtual float Float(string key) {
            return Data.Float(key);
        }

        public virtual double Double(string key) {
            return Data.Double(key);
        }

        public virtual string String(string key, string format) {
            return Data.String(key, format);
        }

        public virtual string String(string key) {
            return Data.String(key, null);
        }

        public virtual uint Uint(string key) {
            return Data.Uint(key);
        }
        //public Color ColorFormInt(string key) { return NGUIMath.IntToColor(data.Int(key)); }

        public virtual bool Null(string key) {
            return !Data.ContainsKey(key) || Data[key] == null;
        }

        public override string ToString() {
            return Serialize();
        }

        public virtual string Serialize() {
            return Serializer.Serialize(this);
        }

        public virtual KJson SetValue(string key, object value) {
            if (HasChild(key)) this[key] = value;
            else Add(key, value);
            return this;
        }

        public virtual KJson SetValue(int index, object value) {
            if (index > Count) throw new KJsonException("SetValue Index " + index + " is Larger than Count " + Count);
            else this[index] = value;
            return this;
        }

        public virtual KJson Clone {
            get {
                var d = isList ? Create(ListData) : Create(Data.Clone());
                return d;
            }
        }

        //G:感觉可能还是需要一个地方设置一个空的seqJson
        public virtual void SetisList(bool isList) {
            this.isList = isList;
        }


        public class Deserializer : IDisposable {
            const string WORD_BREAK = "{}[],:\"";

            public static bool IsWordBreak(char c) {
                return char.IsWhiteSpace(c) || WORD_BREAK.IndexOf(c) != -1;
            }

            enum TOKEN {
                NONE,
                CURLY_OPEN,
                CURLY_CLOSE,
                SQUARED_OPEN,
                SQUARED_CLOSE,
                COLON,
                COMMA,
                STRING,
                NUMBER,
                TRUE,
                FALSE,
                NULL
            };

            StringReader json;

            Deserializer(string jsonString) {
                json = new StringReader(jsonString);
            }

            public static object Deserialize(string jsonString) {
                try {
                    using (var instance = new Deserializer(jsonString)) {
                        return instance.ParseValue();
                    }
                }
                catch (Exception ex) {
                    throw new Exception("Json Deserialize Error : [" + jsonString.Length + "| " + jsonString + "] " +
                                        ex.Message);
                }
            }

            public void Dispose() {
                json.Dispose();
                json = null;
            }

            KJson ParseObject() {
                KJson kJsonObj = new KJson();

                // ditch opening brace
                json.Read();

                while (true) {
                    TOKEN nextToken = NextToken();
                    switch (nextToken) {
                        case TOKEN.NONE: return null;
                        case TOKEN.COMMA: continue;
                        case TOKEN.CURLY_CLOSE: return kJsonObj;
                        default:
                            // name
                            string name = ParseString();
                            if (name == null) {
                                return null;
                            }

                            // :
                            nextToken = NextToken();
                            if (nextToken != TOKEN.COLON) {
                                return null;
                            }

                            // ditch the colon
                            json.Read();

                            // value
                            kJsonObj[name] = ParseValue();
                            break;
                    }
                }
            }

            KJson ParseArray() {
                KJson kJsonArray = new KJson();
                kJsonArray.isList = true;
                // ditch opening bracket
                json.Read();
                // [
                var parsing = true;
                while (parsing) {
                    TOKEN nextToken = NextToken();

                    switch (nextToken) {
                        case TOKEN.NONE: return null;
                        case TOKEN.COMMA: continue;
                        case TOKEN.SQUARED_CLOSE:
                            parsing = false;
                            break;
                        default:
                            object value = ParseByToken(nextToken);
                            kJsonArray.Add(value);
                            break;
                    }
                }

                return kJsonArray;
            }

            object ParseValue() {
                TOKEN nextToken = NextToken();
                return ParseByToken(nextToken);
            }

            object ParseByToken(TOKEN token) {
                switch (token) {
                    case TOKEN.STRING: return ParseString();
                    case TOKEN.NUMBER: return ParseNumber();
                    case TOKEN.CURLY_OPEN: return ParseObject();
                    case TOKEN.SQUARED_OPEN: return ParseArray();
                    case TOKEN.TRUE: return true;
                    case TOKEN.FALSE: return false;
                    case TOKEN.NULL: return null;
                    default: return null;
                }
            }

            string ParseString() {
                StringBuilder s = new StringBuilder();
                char c;

                // ditch opening quote
                json.Read();
                bool parsing = true;
                while (parsing) {
                    if (json.Peek() == -1) {
                        parsing = false;
                        break;
                    }

                    c = NextChar;
                    switch (c) {
                        case '"':
                            parsing = false;
                            break;
                        case '\\':
                            if (json.Peek() == -1) {
                                parsing = false;
                                break;
                            }

                            c = NextChar;
                            switch (c) {
                                case '"':
                                case '\\':
                                case '/':
                                    s.Append(c);
                                    break;
                                case 'b':
                                    s.Append('\b');
                                    break;
                                case 'f':
                                    s.Append('\f');
                                    break;
                                case 'n':
                                    s.Append('\n');
                                    break;
                                case 'r':
                                    s.Append('\r');
                                    break;
                                case 't':
                                    s.Append('\t');
                                    break;
                                case 'u':
                                    var hex = new char[4];
                                    for (int i = 0; i < 4; i++) {
                                        hex[i] = NextChar;
                                    }

                                    s.Append((char) Convert.ToInt32(new string(hex), 16));
                                    break;
                            }

                            break;
                        default:
                            s.Append(c);
                            break;
                    }
                }

                return s.ToString();
            }

            object ParseNumber() {
                string number = NextWord;

                if (number.IndexOf('.') == -1) {
                    long parsedInt;
                    long.TryParse(number, out parsedInt);
                    if (parsedInt > int.MinValue && parsedInt < int.MaxValue) return (int) parsedInt;
                    else return parsedInt;
                }

                double parsedDouble;
                double.TryParse(number, out parsedDouble);
                if (parsedDouble > float.MinValue && parsedDouble < float.MaxValue) return (float) parsedDouble;
                return parsedDouble;
            }

            void EatWhitespace() {
                while (char.IsWhiteSpace(PeekChar)) {
                    json.Read();
                    if (json.Peek() == -1) break;
                }
            }

            char PeekChar {
                get { return Convert.ToChar(json.Peek()); }
            }

            char NextChar {
                get { return Convert.ToChar(json.Read()); }
            }

            string NextWord {
                get {
                    StringBuilder word = new StringBuilder();
                    while (!IsWordBreak(PeekChar)) {
                        word.Append(NextChar);
                        if (json.Peek() == -1) break;
                    }

                    return word.ToString();
                }
            }

            TOKEN NextToken() {
                EatWhitespace();
                if (json.Peek() == -1) return TOKEN.NONE;
                switch (PeekChar) {
                    case '{': return TOKEN.CURLY_OPEN;
                    case '}':
                        json.Read();
                        return TOKEN.CURLY_CLOSE;
                    case '[': return TOKEN.SQUARED_OPEN;
                    case ']':
                        json.Read();
                        return TOKEN.SQUARED_CLOSE;
                    case ',':
                        json.Read();
                        return TOKEN.COMMA;
                    case '"': return TOKEN.STRING;
                    case ':': return TOKEN.COLON;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-': return TOKEN.NUMBER;
                }

                switch (NextWord) {
                    case "false": return TOKEN.FALSE;
                    case "true": return TOKEN.TRUE;
                    case "null": return TOKEN.NULL;
                }

                return TOKEN.NONE;
            }
        }

        public class Serializer {
            StringBuilder builder = new StringBuilder();

            public static Serializer PublicInstance {
                get {
                    if (_instance == null) _instance = new Serializer();
                    return _instance;
                }
            }

            private static Serializer _instance;

            public const string def_null = "null";
            public const string def_true = "true";
            public const string def_false = "false";

            protected Serializer() { }

            public static string Serialize(object obj) {
                Serializer instance = new Serializer();
                instance.builder.Remove(0, instance.builder.Length);
                instance.SerializeValue(obj);
                return instance.builder.ToString();
            }

            void SerializeValue(object value) {
                if (value == null) {
                    builder.Append(def_null);
                }
                else if (value is string) {
                    SerializeString(value as string);
                }
                else if (value is bool) {
                    builder.Append((bool) value ? def_true : def_false);
                }
                else if (value is KJson) {
                    KJson kJsonObject = value as KJson;
                    if (kJsonObject.isList) SerializeArray(kJsonObject);
                    else SerializeObject(kJsonObject);
                }
                else if (value is char) {
                    SerializeStringObj(value);
                }
                else {
                    SerializeOther(value);
                }
            }

            void SerializeObject(KJson obj) {
                bool first = true;

                builder.Append('{');

                //foreach(string e in obj.Data.Keys) {
                //    if(!first) { builder.Append(','); } 
                //    SerializeString(e);
                //    builder.Append(':'); 
                //    SerializeValue(obj[e]); 
                //    first = false;
                //}

                //obj.Data.ForEachPairs((k, v))

                foreach (var p in obj.Data) {
                    if (!first) {
                        builder.Append(',');
                    }

                    SerializeString(p.Key);
                    builder.Append(':');
                    SerializeValue(p.Value);
                    first = false;
                }

                builder.Append('}');
            }

            void SerializeArray(KJson anArray) {
                builder.Append('[');
                bool first = true;
                anArray.ListData.ForEach(obj => {
                    if (!first) {
                        builder.Append(',');
                    }

                    SerializeValue(obj);
                    first = false;
                });
                builder.Append(']');
            }

            void SerializeStringObj(object obj) {
                builder.Append('\"');
                if (obj is string) {
                    string str = obj as string;
                    for (int i = 0; i < str.Length; i++) {
                        switch (str[i]) {
                            case '"':
                                builder.Append("\\\"");
                                break;
                            case '\\':
                                builder.Append("\\\\");
                                break;
                            case '\b':
                                builder.Append("\\b");
                                break;
                            case '\f':
                                builder.Append("\\f");
                                break;
                            case '\n':
                                builder.Append("\\n");
                                break;
                            case '\r':
                                builder.Append("\\r");
                                break;
                            case '\t':
                                builder.Append("\\t");
                                break;
                            default:
                                int codepoint = Convert.ToInt32(str[i]);
                                if ((codepoint >= 32) && (codepoint <= 126)) {
                                    builder.Append(str[i]);
                                }
                                else {
                                    builder.Append("\\u");
                                    builder.Append(codepoint.ToString("x4"));
                                }

                                break;
                        }
                    }
                }
                else {
                    builder.Append(obj);
                }

                builder.Append('\"');
            }

            void SerializeString(string str) {
                builder.Append('\"');
                for (int i = 0; i < str.Length; i++) {
                    switch (str[i]) {
                        case '"':
                            builder.Append("\\\"");
                            break;
                        case '\\':
                            builder.Append("\\\\");
                            break;
                        case '\b':
                            builder.Append("\\b");
                            break;
                        case '\f':
                            builder.Append("\\f");
                            break;
                        case '\n':
                            builder.Append("\\n");
                            break;
                        case '\r':
                            builder.Append("\\r");
                            break;
                        case '\t':
                            builder.Append("\\t");
                            break;
                        default:
                            int codepoint = Convert.ToInt32(str[i]);
                            if ((str[i] >= 32) && (str[i] <= 126)) {
                                builder.Append(str[i]);
                            }
                            else {
                                builder.Append("\\u");
                                builder.Append(codepoint.ToString("x4"));
                            }

                            break;
                    }
                }

                builder.Append('\"');
            }

            private void SerializeOther(object value) {
                // NOTE: decimals lose precision during serialization.
                // They always have, I'm just letting you know.
                // Previously floats and doubles lost precision too.
                if (value is int
                    || value is uint
                    || value is long
                    || value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is ulong) {
                    builder.Append(value);
                }
                else if (value is float) {
                    builder.Append(((float) value).ToString("R"));
                }
                else if (value is double
                         || value is decimal) {
                    builder.Append(Convert.ToDouble(value).ToString("R"));
                }
                else {
                    SerializeStringObj(value);
                }
            }
        }
    }

    internal static class KJsonUtility {
        public static object Check(this Dictionary<string, object> map, string key) {
            if (!map.ContainsKey(key)) throw new KJsonException("[KJsonUtility (Parse)Check] key " + key + " not exist ");
            return map[key];
        }

        public static object Check(this List<object> seq, int index) {
            if (index < 0 || index >= seq.Count)
                throw new KJsonException("[KJsonUtility (Parse)Check] index " + index + " is out of range ");
            return seq[index];
        }

        public static KJson Json(this Dictionary<string, object> map, string key) {
            return map.Check(key) as KJson;
        }

        public static string String(this Dictionary<string, object> map, string key, string format = null) {
            return ObjectToString(map.Check(key), format);
        }

        public static bool Bool(this Dictionary<string, object> map, string key) {
            return ObjectToBool(map.Check(key));
        }

        public static int Int(this Dictionary<string, object> map, string key) {
            return ObjectToInt(map.Check(key));
        }

        public static long Long(this Dictionary<string, object> map, string key) {
            return ObjectToLong(map.Check(key));
        }

        public static float Float(this Dictionary<string, object> map, string key) {
            return ObjectToFloat(map.Check(key));
        }

        public static double Double(this Dictionary<string, object> map, string key) {
            return ObjectToDouble(map.Check(key));
        }

        public static uint Uint(this Dictionary<string, object> map, string key) {
            return ObjectToUint(map.Check(key));
        }

        public static KJson Json(this List<object> seq, int index) {
            return seq.Check(index) as KJson;
        }

        public static string String(this List<object> seq, int index, string format = null) {
            return ObjectToString(seq.Check(index), format);
        }

        public static bool Bool(this List<object> seq, int index) {
            return ObjectToBool(seq.Check(index));
        }

        public static int Int(this List<object> seq, int index) {
            return ObjectToInt(seq.Check(index));
        }

        public static long Long(this List<object> seq, int index) {
            return ObjectToLong(seq.Check(index));
        }

        public static float Float(this List<object> seq, int index) {
            return ObjectToFloat(seq.Check(index));
        }

        public static double Double(this List<object> seq, int index) {
            return ObjectToDouble(seq.Check(index));
        }

        public static uint Uint(this List<object> seq, int index) {
            return ObjectToUint(seq.Check(index));
        }

        public static string ObjectToString(object value, string format = null) {
            if (format.Exists()) {
                return string.Format(format, value);
            }
            else {
                if (value is string) return value as string;
                return value.ToString();
            }
        }

        public static bool ObjectToBool(object value) {
            if (value is bool) return (bool) value;
            else
                throw new KJsonException("[KJsonUtility (Parse)Bool] type of mapitem is dismatch  ," + value + " (" +
                                        value.GetType() + ")");
        }

        public static int ObjectToInt(object value) {
            if (value is int) return (int) value;
            else if (value is long) return (int) ((long) value);

            else if (value is float) return (int) ((float) value);
            else if (value is double) return (int) ((double) value);
            else
                throw new KJsonException("[KJsonUtility (Parse)Int] type of mapitem is dismatch  ," + value + " (" +
                                        value.GetType() + ")");
        }

        public static uint ObjectToUint(object value) {
            if (value is int) return (uint) ((int) value);
            if (value is uint) return (uint) value;
            else if (value is long) return (uint) ((long) value);

            else if (value is float) return (uint) ((float) value);
            else if (value is double) return (uint) ((double) value);
            else
                throw new KJsonException("[KJsonUtility (Parse)Int] type of mapitem is dismatch  ," + value + " (" +
                                        value.GetType() + ")");
        }

        public static long ObjectToLong(object value) {
            if (value is long) return ((long) value);
            else if (value is int) return ((int) value);

            else if (value is float) return (long) ((float) value);
            else if (value is double) return (long) ((double) value);
            else
                throw new KJsonException("[KJsonUtility (Parse)Long] type of mapitem is dismatch  ," + value + " (" +
                                        value.GetType() + ")");
        }

        public static float ObjectToFloat(object value) {
            if (value is float) return ((float) value);
            else if (value is double) return (float) ((double) value);

            else if (value is int) return ((int) value);
            else if (value is long) return ((long) value);
            else
                throw new KJsonException("[KJsonUtility (Parse)Float] type of mapitem is dismatch  ," + value + " (" +
                                        value.GetType() + ")");
        }

        public static double ObjectToDouble(object value) {
            if (value is double) return ((double) value);
            else if (value is float) return ((float) value);

            else if (value is int) return ((int) value);
            else if (value is long) return ((long) value);
            else
                throw new KJsonException("[KJsonUtility (Parse)Double] type of mapitem is dismatch  ," + value + " (" +
                                        value.GetType() + ")");
        }
    }

    public class KJsonException : Exception {
        public KJsonException(string message) : base(message) { }
    }
}