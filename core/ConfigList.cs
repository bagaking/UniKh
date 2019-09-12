using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniKh.core { 
    public class ConfigList : ScriptableObject {
        private static ConfigList _inst;

        public const string assetName = "_UniKh_ConfLst";

        public static ConfigList Inst {
            get {
                if (_inst) return _inst;
                var results = Resources.Load<ConfigList>(assetName);
                if (results == null)
                    throw new Exception("Cannot find UniKh/ConfigList. Create it from Menu : 'UniKh/Create/Config List'");
                return results;
            }
        }
        
        public List<ScriptableObject> Configs;

    }
}