using System;
using System.Collections.Generic;
using UniKh.comp.ui;
using UnityEngine;
using UnityEngine.Serialization;

namespace UniKh.core { 
    public class KhPreferenceStatic : ScriptableObject {
        private static KhPreferenceStatic _inst;

        public const string assetName = "UniKh.Preference.Static";

        public static KhPreferenceStatic Inst {
            get {
                if (_inst) return _inst;
                var results = Resources.Load<KhPreferenceStatic>(assetName);
//                if (results == null)
//                    throw new Exception("Cannot find UniKh/ConfigList. Create it from Menu : 'UniKh/Create/Config List'");
                return results;
            }
        }
        
        public List<ScriptableObject> Configs;

        public Font fontCH;
        public Font fontEN;
        public Font fontNUM;

        public KhText PrefabText;
        public KhImage PrefabImage;
        public KhBtn PrefabBtn; 

        [FormerlySerializedAs("defaultFont")] public int defaultFontSize = 32;

    }
}