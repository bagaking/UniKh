/** == MotionObject.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/01 19:13:48
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.core;
using UniKh.extensions;
using UniKh.utils;
using System.Collections;
using System.Collections.Generic;
using UniKh.core.csp;
using UniKh.core.tween;
using UnityEngine;

namespace UniKh.comp {
    public abstract class MotionObject<T> : BetterBehavior where T: MotionObject<T> {

        public static string className { get; } = typeof(T).Name;
        
        [EaseDetail(true)]
        public StandardEase.Type easeActive = StandardEase.Type.OutExpo;
        public float durationShow = 1f;

        protected abstract IEnumerator PlayAdmissionAnimation();

        protected override void OnSetActive(bool active) {
            base.OnSetActive(active);
            if (active) {
                PlayAdmissionAnimation().Go(className);
            }
            
        }
    }
}