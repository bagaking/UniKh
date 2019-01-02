using System.Collections;
using UnityEngine;

namespace UniKh.utils {
    public class DestroySelf : MonoBehaviour {

        public float time;

        private void OnEnable() { StartCoroutine(DestroySelfInTime()); }

        private IEnumerator DestroySelfInTime() {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}