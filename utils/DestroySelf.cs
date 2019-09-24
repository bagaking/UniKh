using System.Collections;
using UniKh.res;
using UnityEngine;

namespace UniKh.utils {
    public class DestroySelf : MonoBehaviour {

        [Header("tid of using ResPool or 0")]
        public uint tid;

        [Header("TTL in seconds")]
        public float time;

        private void OnEnable() {
            StartCoroutine(DestroySelfInTime());
        }

        private IEnumerator DestroySelfInTime() {
            yield return new WaitForSeconds(time);

            if (tid > 0 && ResPool.LazyInst.TIDExist(tid)) {
                ResPool.LazyInst.Push(gameObject);
            } else {
                Destroy(gameObject);
            }
        }
    }
}