using System.Collections;
using UniKh.core.tween;
using UniKh.extensions;
using UnityEngine;

namespace UniKh.comp.ui {
    public class KhPanelGround : KhPanel {
        public CanvasGroup cg;

        public float startAlpha = 0;
        
        private float finalAlpha = 1f;
        protected override void OnInit() {
            cg = this.GetOrAdd<CanvasGroup>();
            if (startAlpha == 1) return;
            finalAlpha = cg.alpha;
            cg.alpha *= startAlpha;
        }
        protected override void PlayAdmissionAnimation() {
            if (startAlpha == 1) return;
            cg.DoFade(finalAlpha, durationShow).SetEase(easeActive);
        }
    }
}