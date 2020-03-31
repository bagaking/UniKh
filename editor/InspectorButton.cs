using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniKh.utils.Inspector;
using UniKh.extensions;
using UniKh.mgr;
using UniKh.utils;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace UniKh.editor {
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class InspectorBtn : Editor {
        
        public static CacheLru<List<MemberInfo>> cachedMembers = new CacheLru<List<MemberInfo>>(); 
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            var mono = target as MonoBehaviour;

            if (mono == null) return;
            var type = mono.GetType();
            var methods = cachedMembers.Get(type.FullName);
            if (methods == null) {
                var allMethods = cachedMembers.Set(type.FullName, methods = type.GetMembers(
                    BindingFlags.Instance | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                    BindingFlags.NonPublic).ToList());
            }

 
            var btnLst = methods.Filter(o => Attribute.IsDefined(o, typeof(BtnAttribute)));
            foreach (var memberInfo in btnLst) { 
                var btnAttr = memberInfo.GetCustomAttribute<BtnAttribute>(); 
                if (!GUILayout.Button(btnAttr.Name.Exists() ? btnAttr.Name : memberInfo.Name)) continue;
                var method = memberInfo as MethodInfo;
                if (method != null) method.Invoke(mono, null);
            }
        }
    }
}
#endif