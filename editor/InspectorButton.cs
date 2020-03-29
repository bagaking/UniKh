using System;
using System.Linq;
using System.Reflection;
using UniKh.utils.Inspector;
using UniKh.extensions;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace UniKh.editor {
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class InspectorBtn : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            var mono = target as MonoBehaviour;

            if (mono == null) return;
            var methods = mono.GetType()
                .GetMembers(
                    BindingFlags.Instance | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                    BindingFlags.NonPublic)
                .Where(o => Attribute.IsDefined(o, typeof(BtnAttribute)));

            foreach (var memberInfo in methods) {

                var btnAttr = memberInfo.GetCustomAttribute<BtnAttribute>();
                
                if (!GUILayout.Button(btnAttr.Name.Exists() ? btnAttr.Name : memberInfo.Name)) continue;
                var method = memberInfo as MethodInfo;
                if (method != null) method.Invoke(mono, null);
            }
        }
    }
}
#endif