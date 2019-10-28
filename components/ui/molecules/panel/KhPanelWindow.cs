/** == KhPanelWindow.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/28 14:19:53
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Collections;
using UniKh.core.tween;
using UnityEngine;

namespace UniKh.comp.ui {
    public class KhPanelWindow : KhPanel {
        public Vector3 startOffset = Vector3.zero;
        public float startScaleRate = 1;

        private Vector3 finalScale = Vector3.one;
        protected override void OnInit() {
            base.OnInit();
            var trans = transform;
            trans.localPosition += startOffset;
            finalScale = trans.localScale;
            trans.localScale *= startScaleRate;
        }

        protected override IEnumerator PlayAdmissionAnimation() {
            if (startOffset == Vector3.zero && startScaleRate == 1) {
                yield break;
            }
//            var endPos = transform.localPosition;
            transform.TweenScale(finalScale, durationShow).SetEase(easeActive);
            transform.TweenMoveOffsetLocal(- startOffset, durationShow).SetEase(easeActive);
            yield return null;
        }
    }
}