using System.Collections;
using UniKh.res;
using UnityEngine;

namespace UniKh.comp {
    
    public class DelayDestroy : DelayBase<DelayActivate>  {

        [Header("Target GameObject (or self)")]
        public GameObject target;
        
        [Header("tid of using ResPool or 0")]
        public uint tid;
 
        public override IEnumerator DoDelayEvent() {
            yield return new WaitForSeconds(delaySec);

            if (tid > 0 && ResPool.LazyInst.TIDExist(tid)) {
                ResPool.LazyInst.Push(target ? target : gameObject);
            } else {
                Destroy(target ? target : gameObject);
            }
        }
    }
}