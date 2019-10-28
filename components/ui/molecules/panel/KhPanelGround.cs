using System.Collections;
using UniKh.core.tween;
using UniKh.extensions;
using UnityEngine;

namespace UniKh.comp.ui {
    public class KhPanelGround : KhPanel {
        protected override void OnInit() {
            cg = this.GetOrAdd<CanvasGroup>();
        }

        public CanvasGroup cg;

        public float startAlpha = 0;

        protected override IEnumerator PlayAdmissionAnimation() {
            if (startAlpha == 1) yield break;
            var finalAlpha = cg.alpha;
            cg.alpha *= startAlpha;
            cg.DoFade(finalAlpha, durationShow).SetEase(easeActive);
            yield return null;
        }
    }
}