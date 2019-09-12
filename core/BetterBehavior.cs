using UnityEngine;

namespace UniKh.core {
    public abstract class BetterBehavior : MonoBehaviour {
        private bool initiated = false;
        private bool started = false;

        private void OnEnable() {
            if (!started) return;
            WhenSetActive(true);
        }

        private void Start() {
            if (!initiated) {
                initiated = true;
                Initial();
            }
            WhenSetActive(true);
            started = true;
        }

        private void OnDisable() { WhenSetActive(false); }

        protected abstract void Initial();

        protected abstract void WhenSetActive(bool active);
    }
}