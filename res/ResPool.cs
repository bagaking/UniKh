using System;
using System.Collections.Generic;
using UniKh.core;
using UniKh.extensions;
using UniKh.mgr;
using UnityEngine;

namespace UniKh.res {
    public class ResPool : Singleton<ResPool> {
        public readonly Dictionary<uint, List<GameObject>> pool = new Dictionary<uint, List<GameObject>>();

        public readonly Dictionary<uint, int> totalCreated = new Dictionary<uint, int>();

        public readonly Dictionary<int, uint> objIds = new Dictionary<int, uint>();

        public GameObject Pop(uint tid) {
            if (pool.ContainsKey(tid) && pool[tid].Count > 0) return pool[tid].Pop();
            var go = ResMgr.LazyInst.Create<GameObject>($"prefab_r/{tid}");
            objIds[go.GetHashCode()] = tid;
            go.name = go.name.Replace("(Clone)", "");
            totalCreated.Inc(tid, 1);
            return go;
        }

        public void Push(GameObject obj) {
            var objHash = obj.GetHashCode();
            if (!objIds.ContainsKey(objHash)) throw new Exception($"ResPool.Push error: cannot find id of the game object {obj}");
            var tid = objIds[objHash];
            if (!pool.ContainsKey(tid)) pool[tid] = new List<GameObject>();
            pool[tid].Push(obj);
            obj.transform.SetParent(Inst.transform);
            obj.transform.localPosition = Vector3.zero;
        }
    }
}