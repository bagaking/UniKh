using System;
using System.Collections.Generic;
using System.Text;
using UniKh.core;
using UniKh.utils.Inspector;
using Unity.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniKh.mgr {
    public class ResMgr : Singleton<ResMgr> {
        public int pageSize = 32;

        private Dictionary<string, Object> _cache1;

        private Dictionary<string, Object> _cache2;

        private readonly HashSet<string> _keysInSync = new HashSet<string>();

        public void SetPageSize(int size) => pageSize = size;

        internal Object LoadFromCache(string key) {
            if (_cache1 != null && _cache1.ContainsKey(key)) {
                return _cache1[key];
            }

            if (_cache2 != null && _cache2.ContainsKey(key)) {
                return _cache2[key];
            }

            return null;
        }

        internal T SetCache<T>(string key, T pref) where T : Object {
            if (_cache1 == null) {
                _cache1 = new Dictionary<string, Object>(pageSize);
            }

            if (!_cache1.ContainsKey(key) && _cache1.Count >= pageSize) {
                _cache2 = _cache1;
                _cache1 = new Dictionary<string, Object>(pageSize);
            }

            _cache1[key] = pref;
            return pref;
        }

        public T Load<T>(string path) where T : Object {
            var pref = LoadFromCache(path);
            if (null != pref) return pref as T;

            var prefT = Resources.Load<T>(path);

            return SetCache(path, prefT);
        }
        
        public void LoadAsync<T>(string path, Action<T> callback) where T : Object {
            StartCoroutine(LoadAsyncCorou(path, callback));
        }
        
        private IEnumerator<object> LoadAsyncCorou<T>(string path, Action<T> callback) where T : Object {
            if (_keysInSync.Contains(path)) { // lock
                yield return new WaitUntil(()=>!_keysInSync.Contains(path));
            }
            var pref = LoadFromCache(path);
            if (null != pref) callback(pref as T);
            
            var request = Resources.LoadAsync<T>(path);
            _keysInSync.Add(path);
            yield return request;
            
            var prefT = request.asset as T;
            SetCache(path, prefT);
            _keysInSync.Remove(path);
            callback(prefT);
        }

        public T Create<T>(string path) where T : Object {
            var pref = Load<T>(path);
            var inst = Instantiate(pref);
            return inst;
        }
        
        [Btn]
        public void PrintCache() { // todo: show in inspector 
            print(ToString());
        }

        public override string ToString() {
            var sb = new StringBuilder("ResMgr : ");
            sb.AppendLine().Append("cache 1 :");
            if(_cache1 != null)foreach (var key in _cache1.Keys) {
                sb.Append(key).Append(' ');
            }

            sb.AppendLine().Append("cache 2 :");
            if(_cache2 != null)foreach (var key in _cache2.Keys) {
                sb.Append(key).Append(' ');
            }

            return sb.ToString();
        }
    }
}