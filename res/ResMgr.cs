using System;
using System.Collections.Generic;  
using UniKh.core;
using UniKh.core.csp;
using UniKh.utils;
using UniKh.utils.Inspector; 
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniKh.res {
     
    public class ResMgr : Singleton<ResMgr> {
        public CacheLru<Object> cache = new CacheLru<Object>(64);

        private readonly HashSet<string> _keysInSync = new HashSet<string>();

        public T Load<T>(string path) where T : Object {
            var pref = cache.Get(path);
            if (null != pref) return pref as T;
            var prefT = Resources.Load<T>(path);
            return cache.Set(path, prefT);
        }

        public void LoadAsync<T>(string path, Action<T> callback) where T : Object {
            LoadAsyncCorou(path, callback).AsPromise();
        }

        public Promise<T> LoadAsync<T>(string path) where T : Object {
            T val = null;
            return LoadAsyncCorou<T>(path, ret => { val = ret; }).AsPromise().Then(_ => val);
        }

        private IEnumerator<object> LoadAsyncCorou<T>(string path, Action<T> callback) where T : Object {
            if (_keysInSync.Contains(path)) yield return new WaitUntil(() => !_keysInSync.Contains(path)); // sync lock 

            var pref = cache.Get(path);
            if (null != pref) callback(pref as T); // 如果确实是 null 则会击穿

            var request = Resources.LoadAsync<T>(path);
            _keysInSync.Add(path);
            yield return request;

            var prefT = request.asset as T;

            if (!prefT) {
                Debug.LogError(SGen.New["Cannot find asset "][path][" of type "][typeof(T)]);
            }

            cache.Set(path, prefT);
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
            print(cache.ToString());
        }
    }
}