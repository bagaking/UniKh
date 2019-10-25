using System.Collections;
using UniKh.core.csp;
using UniKh.core.tween;
using UnityEngine;

namespace UniKh.comp.ui
{
    public class KhPanel : MonoBehaviour
    {
        public CanvasGroup cg;
        public float startAlpha = 0;
        public Vector3 startPos = Vector3.zero;
        [EaseDetail(true)]
        public StandardEase.Type easeActive = StandardEase.Type.OutExpo;
        public float durationShow = 0.3f;
        
        void OnEnable() {
            if (!cg) cg = GetComponent<CanvasGroup>();
            cg.alpha = startAlpha;
            cg.DoFade(1, durationShow).SetEase(easeActive);
            Vector3 endPos = transform.position;
            transform.SetPositionAndRotation(startPos,transform.rotation);
            transform.TweenMoveOffset(endPos, durationShow);
        }
    }
}