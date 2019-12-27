/** == MotionTweenAlphaFrom.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/05 18:59:46
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.core;
using UniKh.extensions;
using UniKh.utils;

using System.Collections;
using System.Collections.Generic;
using UniKh.core.tween;
using UnityEngine; 

namespace UniKh.comp.ui {
    public class MotionTweenAlphaFrom : MotionObject<MotionTweenAlphaFrom> {
        
        [Header("Motion Setting")]
        public float startAlphaRate = 1;

        private float _initAlpha = 1;
        private CanvasGroup _canvasGroup;

        public CanvasGroup CG => _canvasGroup;

        protected override void OnInit() {
            base.OnInit(); 
            _canvasGroup = this.GetOrAdd<CanvasGroup>();
            _initAlpha = _canvasGroup.alpha;  
        }

        protected override void OnSetActive(bool active) { 
            if (active) {
                _canvasGroup.alpha = _initAlpha * startAlphaRate;
            }
            base.OnSetActive(active);
        }

        protected override IEnumerator PlayAdmissionAnimation() {
            if (startAlphaRate == 1) {
                yield break;
            }
            _canvasGroup.DoFade(_initAlpha, durationShow).SetEase(easeActive);
            yield return null;
        }
    }
}