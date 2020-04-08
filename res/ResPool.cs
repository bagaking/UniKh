using System;
using System.Collections.Generic;
using UniKh.core;
using UniKh.extensions; 
using UnityEngine;

namespace UniKh.res {
    public class ResPool : Singleton<ResPool> {
        public readonly Dictionary<uint, List<GameObject>> pool = new Dictionary<uint, List<GameObject>>();

        public readonly Dictionary<uint, int> totalCreated = new Dictionary<uint, int>();

        public readonly Dictionary<int, uint> objIds = new Dictionary<int, uint>();

        public readonly Dictionary<uint, string> pathCache = new Dictionary<uint, string>();
 
        public GameObject Bind(uint resId, string path) {
            if (resId == 0) throw new Exception($"ResPool.Bind error: resId {resId} cannot be zero (ban flag)");
            if (pool.ContainsKey(resId)) throw new Exception($"ResPool.Bind error: resId {resId} is already exist");
            var gObj = ResMgr.LazyInst.Load<GameObject>(path); 
            if (!gObj) {
                Debug.LogError($"ResPool.Bind error: resource <resId:{resId}> cannot be found as GameObject");
            }
            pathCache[resId] = path;
            pool[resId] = new List<GameObject>();
            return gObj;
        }

        public bool TidExist(uint resId) {
            return pool.ContainsKey(resId);
        }

        public GameObject Pop(uint resId, bool setActive = false)  {
            if (!pathCache.ContainsKey(resId)) throw new Exception($"the resource of id {resId} are not bind.");

            GameObject instance = null;
            while (pool[resId].Count > 0 && null == (instance = pool[resId].Pop())) ;

            if (instance == null) {
                instance = ResMgr.LazyInst.Create<GameObject>(pathCache[resId]); // 原来是直接做备份所以没问题
                totalCreated.Inc(resId, 1);
                instance.name = instance.name.Replace("(Clone)", "_" + totalCreated[resId].ToString());
            }

            var hash = instance.GetHashCode();
            if (!objIds.ContainsKey(hash)) {
                objIds[hash] = resId;
            }
            else {
                Debug.LogError("hash code " + hash + "are banned");
                objIds[hash] = 0; // this object and the origin object will not be recycled in the next Push operation.
            }

            if (setActive) instance.SetActive(true);
            return instance;
        }


        public void Kill(GameObject gObj) {
            var objHash = gObj.GetHashCode();

            if (!gameObject.activeInHierarchy) {
                if (objIds.ContainsKey(objHash)) {
                    objIds.Remove(objHash);
                }
                Destroy(gObj);
                return;
            }
            
            if (!objIds.ContainsKey(objHash)) {
                // throw new Exception($"ResPool: gObj {gObj} with absent hash are killed.");
                Debug.LogError($"ResPool: gObj {gObj} with absent hash are killed.");
                Destroy(gObj);
                return;
            }
            
            if (objIds[objHash] == 0) {
                Debug.LogError($"ResPool: gObj {gObj} with banned hash are killed.");
                Destroy(gObj);
                return;
            }

            gObj.SetActive(false);
            gObj.transform.SetParent(Inst.transform);
            gObj.transform.localPosition = Vector3.zero;

            var resId = objIds[objHash];
            pool[resId].Push(gObj);
            objIds.Remove(objHash);
        }
    }
}