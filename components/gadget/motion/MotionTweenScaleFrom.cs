/** == MotionTweenScale.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/02 02:54:17
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
    public class MotionTweenScaleFrom : MotionObject<MotionTweenMoveOffsetFrom> {
        
        [Header("Motion Setting")]
        public float startScaleRate = 1;

        private Vector3 _initScale = Vector3.one; 
        protected override void OnInit() {
            base.OnInit();
            var trans = transform;
            _initScale = trans.localScale;  
        }

        protected override void OnSetActive(bool active) {
            transform.localScale = _initScale * startScaleRate;
            base.OnSetActive(active);
        }

        protected override IEnumerator PlayAdmissionAnimation() {
            if (startScaleRate == 1) {
                yield break;
            }
            transform.TweenScale(_initScale, durationShow).SetEase(easeActive);
            yield return null;
        }
    }
}