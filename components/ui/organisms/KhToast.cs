using System.Collections;
using System.Collections.Generic;
using UniKh.core.csp;
using UniKh.core.csp.waiting;
using UniKh.core.tween;
using UnityEngine;

namespace UniKh.comp.ui {
    
    public class KhToast : MotionTweenMoveOffsetFrom {
        public CanvasGroup cg;

        [EaseDetail(true)] public StandardEase.Type easeDisappear = StandardEase.Type.InOutCubic;

        public float durationDisappear = 0.3f;

        public bool autoHide = true;
        public float stayTime = 2f;

        public KhText content;

        public bool IsWorking => gameObject.activeInHierarchy;

        protected override void OnSetActive(bool active) {
            if (!cg) cg = GetComponent<CanvasGroup>();
            
            if (active) { 
                cg.alpha = 0.5f;
                cg.DoFade(1, durationShow).SetEase(easeActive);
                if (autoHide) {
                    AutoHide().Go().Delay(UnitySecond.New.Start(durationShow)); 
                } 
            } 
            base.OnSetActive(active);
        }

        IEnumerator AutoHide() {
            transform.TweenMoveOffsetLocal(Vector3.up * 80, stayTime).SetEase(StandardEase.Type.OutQuad);
            yield return stayTime;
            Disappear();
        }
        
        public void Show(string text) {
            if (content) content.text = text;
            gameObject.SetActive(true);
        }

        public void Disappear() {
            if (!cg) cg = GetComponent<CanvasGroup>();
            cg.alpha = 1f;
            cg.DoFade(0f, durationShow).SetEase(easeDisappear).OnStateChanged += (stateFrom, stateTo) => {
                if (stateTo != Tweener.State.Complete) return;
                gameObject.SetActive(false);
            };
        }
    }
}