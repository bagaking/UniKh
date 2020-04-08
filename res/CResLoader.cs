using UniKh.core; 
using UniKh.utils;
using UnityEngine;

namespace UniKh.res {
    public class CResLoader {
        public static string root = "cr";

        public int[] Folders = new[] {10000000, 100000};

        public string GetPath(uint crId) {
            var builder = SGen.New[root];
            for (var i = 0; i < Folders.Length; i++) {
                var v = i == 0 ? crId : crId % Folders[i - 1];
                builder = builder["/"][v / Folders[i]];
            }

            return builder["/"][crId].End;
        }

        public GameObject Load(uint crId) {
            var path = GetPath(crId);
            var ret = ResPool.LazyInst.Bind(crId, path);
            if (ret != null) {
                ResPool.LazyInst.Bind(crId, path);
                Debug.Log($"load cr<{crId}> at {path} succeed! {path}");
            }
            else {
                Debug.LogError($"load cr<{crId}> at {path} failed!");
            }

            return ret;
        }

        public Promise<GameObject> LoadAsync(uint crId) {
            var path = GetPath(crId);
            return ResMgr.LazyInst.LoadAsync<GameObject>(path)
                .Then(_ => ResPool.LazyInst.Bind(crId, path))
                .Then(gObj => {
                    if (gObj != null) {
                        Debug.Log($"load cr<{crId}> at {path} succeed! {path}");
                    }
                    else {
                        Debug.LogError($"load cr<{crId}> at {path} failed!");
                    }

                    return gObj;
                });  
        }
    }
}