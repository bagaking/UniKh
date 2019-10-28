using System.Collections;
using UniKh.core;
using UniKh.core.csp;
using UniKh.core.tween;
using UniKh.extensions;
using UnityEngine;

namespace UniKh.comp.ui
{
    public abstract class KhPanel : BetterBehavior
    {
        
        [EaseDetail(true)]
        public StandardEase.Type easeActive = StandardEase.Type.OutExpo;
        public float durationShow = 0.3f;

        protected abstract IEnumerator PlayAdmissionAnimation();

        protected override void OnSetActive(bool active) {
            base.OnSetActive(active);
            if (active) {
                PlayAdmissionAnimation().Go();
            }
            
        }
    }
}