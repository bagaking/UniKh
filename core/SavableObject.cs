using System;
using System.Collections.Generic;
using UniKh.extensions;
using UniKh.utils;
using UnityEngine;
using UnityEngine.UI;

namespace UniKh.core {
    [Serializable]
    public class SavableObj {
        public static string GetDataKey(string key) => $"save:{key}:data";
        public static string GetTypeKey(string key) => $"save:{key}:type";
        public string ToJson() => JsonUtility.ToJson(this);

        public void FromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);

        public override string ToString() { return ToJson(); }

        public void Save(string key = "") {
            var dataType = PlayerPrefs.GetString(GetTypeKey(key), null);
            var strMyType = GetType().FullName;
            if (!key.Exists()) {
                key = strMyType;
            }
            
            if (!dataType.Exists() || dataType == strMyType) {
                var serializedData = ToJson();
                Log.Info($"SavableObj.Save |\nkey:{key} \n=> {serializedData}");
                PlayerPrefs.SetString(GetDataKey(key), serializedData);
                PlayerPrefs.SetString(GetTypeKey(key), strMyType);
            } else {
                throw new Exception($"SavableObj.Save Error: data type not match {dataType} => {strMyType}");
            }
        }

        public SavableObj Load(string key) {
            FromJson(PlayerPrefs.GetString(GetDataKey(key)));
            return this;
        }

        public static T Get<T>(string key) where T : SavableObj, new() => (new T()).Load(key) as T;

        public static void Remove(string key) {
            PlayerPrefs.DeleteKey(GetDataKey(key));
            PlayerPrefs.DeleteKey(GetTypeKey(key));
        }

        public static bool RemoveInst<T>(string key = "") {
            var strMyType = typeof(T).FullName;
            if (!key.Exists()) key = strMyType;
            var dataType = PlayerPrefs.GetString(GetTypeKey(key), null);
            if (dataType != strMyType) return false;
            Remove(key);
            return true;
        }

        private static readonly Dictionary<string, SavableObj> _instances = new Dictionary<string, SavableObj>();

        public static T Inst<T>(string key = "") where T : SavableObj, new() {
            var strMyType = typeof(T).FullName;
            if (!key.Exists()) key = strMyType;
            if (_instances.ContainsKey(key)) return _instances[key] as T;
            var inst = Get<T>(key);
            _instances[key] = inst;
            return inst;
        }
        
        public static void SaveAll() {
            foreach (var pair in _instances) {
                pair.Value.Save(pair.Key);
            }
        }
    }
}