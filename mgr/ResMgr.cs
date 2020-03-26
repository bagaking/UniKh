using System;
using System.Collections.Generic;
using System.Text;
using Giu.Basic.Helper;
using UniKh.core;
using UniKh.core.csp;
using UniKh.utils.Inspector;
using Unity.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniKh.mgr {
    public class ResMgr : Singleton<ResMgr> {
        public int pageSize = 32;

        public Dictionary<string, Object> Cache1 { get; private set; }

        public Dictionary<string, Object> Cache2 { get; private set; }

        private readonly HashSet<string> _keysInSync = new HashSet<string>();

        public void SetPageSize(int size) => pageSize = size;

        internal Object LoadFromCache(string key) {
            if (Cache1 != null && Cache1.ContainsKey(key)) {
                return Cache1[key];
            }

            if (Cache2 != null && Cache2.ContainsKey(key)) {
                return Cache2[key];
            }

            return null;
        }

        internal T SetCache<T>(string key, T pref) where T : Object {
            if (Cache1 == null) {
                Cache1 = new Dictionary<string, Object>(pageSize);
            }

            if (!Cache1.ContainsKey(key) && Cache1.Count >= pageSize) {
                Cache2 = Cache1;
                Cache1 = new Dictionary<string, Object>(pageSize);
            }

            Cache1[key] = pref;
            return pref;
        }

        public T Load<T>(string path) where T : Object {
            var pref = LoadFromCache(path);
            if (null != pref) return pref as T;

            var prefT = Resources.Load<T>(path);

            return SetCache(path, prefT);
        }

        public void LoadAsync<T>(string path, Action<T> callback) where T : Object {
            LoadAsyncCorou(path, callback).AsPromise();
        }
        
        public Promise<T> LoadAsync<T>(string path) where T : Object {
            T val = null;
            return LoadAsyncCorou<T>(path, ret => { 
                val = ret;
            }).AsPromise().Then(_ => val);
        }

        private IEnumerator<object> LoadAsyncCorou<T>(string path, Action<T> callback) where T : Object {
            if (_keysInSync.Contains(path)) { // lock
                yield return new WaitUntil(() => !_keysInSync.Contains(path));
            }

            var pref = LoadFromCache(path);
            if (null != pref) callback(pref as T); // 去过确实是 null 则会击穿

            var request = Resources.LoadAsync<T>(path);
            _keysInSync.Add(path);
            yield return request;

            var prefT = request.asset as T;

            if (!prefT) {
                Debug.LogError(SGen.New["Cannot find asset "][path][" of type "][typeof(T)]);
            }
            
            SetCache(path, prefT);
            _keysInSync.Remove(path);
            callback(prefT);
        }

        public T Create<T>(string path) where T : Object {
            var pref = Load<T>(path);
            try {
                var inst = Instantiate(pref);
                return inst;
            }
            catch (Exception ex) {
                Debug.LogError(SGen.New["Create "][path][" failed."].End);
                throw ex;
            }
        }

        [Btn]
        public void PrintCache() { // todo: show in inspector 
            print(ToString());
        }

        public override string ToString() {
            var sb = new StringBuilder("ResMgr : ");
            sb.AppendLine().Append("cache 1 :");
            if (Cache1 != null)
                foreach (var key in Cache1.Keys) {
                    sb.Append(key).Append(' ');
                }

            sb.AppendLine().Append("cache 2 :");
            if (Cache2 != null)
                foreach (var key in Cache2.Keys) {
                    sb.Append(key).Append(' ');
                }

            return sb.ToString();
        }
    }
}