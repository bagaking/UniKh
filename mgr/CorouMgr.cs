using System.Collections;
using UniKh.core;
using UnityEngine;

namespace UniKh.mgr {
    public class CorouMgr : Singleton<CorouMgr> {
        public static Coroutine Run(IEnumerator itr) { return Inst.StartCoroutine(itr); }
    }
}