using System.Collections;
using System.Runtime.CompilerServices;
using UniKh.core.csp;
using UniKh.core.tween;
using UnityEngine;

namespace UniKh.comp.ui
{
    public class KhToast: MonoBehaviour
    {
        public CanvasGroup cg;
        [EaseDetail(true)]
        public StandardEase.Type easeActive = StandardEase.Type.OutExpo;
        [EaseDetail(true)]
        public StandardEase.Type easeDisappear = StandardEase.Type.InOutCubic;

        public float durationShow = 0.3f;
        public float durationDissappear = 0.1f;

        public bool autoHide = true;
        public float stayTime = 2f;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnEnable() {
            if (!cg) cg = GetComponent<CanvasGroup>();
            cg.alpha = 0.5f;
            cg.DoFade(1, durationShow).SetEase(easeActive);
            if (autoHide) {
                AutoHide().Go();
            }
        }
        IEnumerator AutoHide() {
            yield return stayTime;
            Disappear();
        }
        public void Disappear() {
            if (!cg) cg = GetComponent<CanvasGroup>();
            cg.alpha = 1f;
            cg.DoFade(0f, durationShow).SetEase(easeDisappear).OnStateChanged+= (state1, state2) =>
            {
                gameObject.SetActive(false);
            };
        }
    }
}