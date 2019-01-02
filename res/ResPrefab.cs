using UniKh.mgr;
using UnityEngine;

namespace UniKh.res {
    public class ResPrefab : MonoBehaviour {
        public uint rid;

        public void Bind() {
            ResMgr.Inst.SetCache($"rid:{rid}", this);
        }
    }
}