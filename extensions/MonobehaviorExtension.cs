using UnityEngine;

namespace UniKh.extensions {
    public static class MonobehaviorExtension {
        public static TComponent GetOrAdd<TComponent>(this MonoBehaviour mono) where TComponent : Component {
            var ret = mono.GetComponent<TComponent>();
            if (null == ret) {
                ret = mono.gameObject.AddComponent<TComponent>();
            }

            return ret;
        }


        public static TComponent AddChild<TComponent>(this MonoBehaviour mono, string name)
            where TComponent : Component {
            var aiNode = new GameObject {name = name};
            aiNode.transform.SetParent(mono.transform);
            return aiNode.AddComponent<TComponent>();
        }
        
        public static Transform GetChild(this MonoBehaviour mono, string name) {
            for (var i = 0; i < mono.transform.childCount; i++) {
                var childI = mono.transform.GetChild(i);
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
    }
}