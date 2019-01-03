using System.Collections.Generic;
using UniKh.core;
using UniKh.extensions;
using UniKh.mgr;
using UnityEngine;

namespace UniKh.res {
    public class ResPool : Singleton<ResPool> {
        private static readonly Dictionary<uint, List<GameObject>> Pool = new Dictionary<uint, List<GameObject>>();

        public static GameObject Pop(uint tid) {
            if (Pool.ContainsKey(tid) && Pool[tid].Count > 0) return Pool[tid].Pop();
            var go = Instantiate(ResMgr.Inst.LoadR(tid));
            go.name = go.name.Replace("(Clone)", "");
            return go;
        }

        public static void Push(uint tid, GameObject obj) {
            if (!Pool.ContainsKey(tid)) Pool[tid] = new List<GameObject>();
            Pool[tid].Push(obj);
            obj.transform.SetParent(Inst.transform);
            obj.transform.localPosition = Vector3.zero;
        }
    }
}