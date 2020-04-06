/** == MotionTweenMove.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/01 19:17:34
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.core;
using UniKh.extensions;
using UniKh.utils;

using System.Collections;
using System.Collections.Generic;
using UniKh.core.tween;
using UniKh.utils.Inspector;
using UnityEngine; 

namespace UniKh.comp.ui {
    public class MotionTweenMoveOffsetFrom : MotionObject<MotionTweenMoveOffsetFrom> {
        
        [Header("Motion Setting")]
        public Vector3 startOffset = Vector3.zero;

        private Vector3 _initPosition = Vector3.zero; 
        protected override void OnInit() {
            base.OnInit();
            var trans = transform;
            _initPosition = trans.localPosition;  
        }

        protected override void OnSetActive(bool active) {
            transform.localPosition = _initPosition + startOffset;
            base.OnSetActive(active);
        }

        protected override IEnumerator PlayAdmissionAnimation() {
            if (startOffset == Vector3.zero) {
                yield break;
            }
            transform.TweenMoveOffsetLocal(- startOffset, durationShow).SetEase(easeActive);
            yield return null;
        }

        [Btn(true)]
        public void Hide() {
            transform.TweenMoveOffsetLocal(startOffset, durationShow).SetEase(easeActive).OnStateChanged +=
                (state, state1) => { this.SetObjectActive(false); };
        }
    }
} 