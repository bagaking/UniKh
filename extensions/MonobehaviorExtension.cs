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
    }
}