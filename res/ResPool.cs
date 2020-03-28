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

        public readonly Dictionary<uint, GameObject> prefabCache = new Dictionary<uint, GameObject>();

        public GameObject Pop(uint tid, bool setActive = false) {
            if (pool.ContainsKey(tid) && pool[tid].Count > 0) {
                return pool[tid].Pop();
            }
            
            var go = prefabCache.ContainsKey(tid)
                ? Instantiate(prefabCache[tid])
                : ResMgr.LazyInst.Create<GameObject>($"prefab_r/{tid}");
            if (!pool.ContainsKey(tid)) {
                pool[tid] = new List<GameObject>();
            }

            var hash = go.GetHashCode();

            if (objIds.ContainsKey(hash)) {
                Debug.LogError("hash code " + hash + "are already exist");
            }
            
            objIds[hash] = tid;

            totalCreated.Inc(tid, 1);
            go.name = go.name.Replace("(Clone)", totalCreated[tid].ToString());
            
            if (setActive) {
                go.SetActive(true);
            }
            
            return go;
        }

        public void Bind(uint tid, GameObject prefab) {
            if (tid == 0) throw new Exception($"ResPool.Bind error: tid {tid} cannot be zero");
            if (pool.ContainsKey(tid)) throw new Exception($"ResPool.Bind error: tid {tid} is already exist");
            prefabCache[tid] = prefab;
            pool[tid] = new List<GameObject>();
        }

        public bool TidExist(uint tid) {
            return pool.ContainsKey(tid);
        }

        public void Push(GameObject gObj) {
            var objHash = gObj.GetHashCode(); 
            if (!objIds.ContainsKey(objHash))
                throw new Exception($"ResPool.Push error: cannot find id of the game object {gObj}");
            var tid = objIds[objHash];
            gObj.SetActive(false); 
            
            if (!pool.ContainsKey(tid)) pool[tid] = new List<GameObject>();
            pool[tid].Push(gObj);
            gObj.transform.SetParent(Inst.transform);
            gObj.transform.localPosition = Vector3.zero;
        }
    }
}