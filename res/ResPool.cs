using System;
using System.Collections.Generic;
using UniKh.core;
using UniKh.extensions;
using UniKh.mgr;
using UnityEngine;

namespace UniKh.res {
    public class ResPool : Singleton<ResPool> {
        public static readonly Dictionary<uint, List<GameObject>> Pool = new Dictionary<uint, List<GameObject>>();
        
        public static readonly Dictionary<uint, int> totalCreated = new Dictionary<uint, int>();

        public static readonly Dictionary<GameObject, uint> ObjIds = new Dictionary<GameObject, uint>();

        public static GameObject Pop(uint tid) {
            if (Pool.ContainsKey(tid) && Pool[tid].Count > 0) return Pool[tid].Pop();
            var go = ResMgr.Inst.Create<GameObject>($"prefab_r/{tid}");
            ObjIds[go] = tid;
            go.name = go.name.Replace("(Clone)", "");
            totalCreated.Inc(tid, 1); 
            return go;
        }

        public static void Push(GameObject obj) {
            if (!ObjIds.ContainsKey(obj)) throw new Exception($"ResPool.Push error: cannot find id of the game object");
            var tid = ObjIds[obj];
            if (!Pool.ContainsKey(tid)) Pool[tid] = new List<GameObject>();
            Pool[tid].Push(obj);
            obj.transform.SetParent(Inst.transform);
            obj.transform.localPosition = Vector3.zero;
        }

    }
}