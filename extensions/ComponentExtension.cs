using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Object;

namespace UniKh.extensions {
    public static class ComponentExtension {
        public static TComponent GetOrAdd<TComponent>(this Component comp) where TComponent : Component {
            var ret = comp.GetComponent<TComponent>();
            if (null == ret) {
                ret = comp.gameObject.AddComponent<TComponent>();
            }

            return ret;
        }
        
        public static T SetObjectActive<T>(this T comp, bool active = true) where T : Component {
            var go = comp.gameObject;
            if (active != go.activeSelf) {
                go.SetActive(active);
            };
            return comp;
        }


        public static TComponent AddChild<TComponent>(this Component comp, string name)
            where TComponent : Component {
            var aiNode = new GameObject {name = name};
            aiNode.transform.SetParent(comp.transform);
            return aiNode.AddComponent<TComponent>();
        }

        public static void RemoveIfExist<T>(this Component comp) where T : Component {
            var t = comp.GetComponent<T>();
            if (t != null) DestroyImmediate(t);
        }

        public static Transform GetChild(this Component comp, string name) {
            for (var i = 0; i < comp.transform.childCount; i++) {
                var childI = comp.transform.GetChild(i);
                if (childI.name == name) {
                    return childI;
                }
            }

            return null;
        }

        public static Transform GetChild(this GameObject go, string name) {
            for (var i = 0; i < go.transform.childCount; i++) {
                var childI = go.transform.GetChild(i);
                if (childI.name == name) {
                    return childI;
                }
            }

            return null;
        }
        
        public static List<Transform> GetChildren(this Transform trans, bool includeInactive = true) {
            var children = new List<Transform>();
            trans.ForEachChild(children.Add, includeInactive);
            return children;
        }

        public static void ForEachChild(this Transform trans, Action<Transform> func, bool includeInactive = true) {
            if (func == null) throw new Exception("func must exist when traversing child nodes");
            for (var i = 0; i < trans.childCount; i++) {
                if (!includeInactive && !trans.gameObject.activeInHierarchy) return;
                func(trans.GetChild(i));
            }
        }
        
        public static void ForEachChild<T>(this Transform trans, Action<T> func) where T : Component{
            if (func == null) throw new Exception("func must exist when traversing child nodes");
            for (var i = 0; i < trans.childCount; i++) {
                var childTrans = trans.GetChild(i);
                var comp = childTrans.GetComponent<T>();
                func(comp);
            }
        }
    }
}