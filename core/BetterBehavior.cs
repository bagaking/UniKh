/** Copyright (C) 2019 Bagaking, All Rights Reserved */

using UnityEngine;

namespace UniKh.core {
    public abstract class BetterBehavior : MonoBehaviour {

        private bool m_initiated = false;
        private bool m_started = false;

        /** events */

        private void OnEnable() {
            if (!m_started) return;
            OnSetActive(true);
        }

        private void Start() {
            if (!m_initiated) {
                m_initiated = true;
                OnInit();
            }
            OnSetActive(true);
            m_started = true;
        }

        private void OnDisable() { OnSetActive(false); }

        /// <summary>
        /// The initial method, put your init code here
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// when the active state changes, the method will be call.
        /// </summary>
        protected virtual void OnSetActive(bool active) { }

        /** helppers */

        [System.NonSerialized]
        private Transform m_myTransform;
        protected Transform MyTransform {
            get {
                if (m_myTransform == null) m_myTransform = transform;
                return m_myTransform;
            }
        }
    }
}