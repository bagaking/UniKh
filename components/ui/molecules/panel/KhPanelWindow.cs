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

        protected override IEnumerator PlayAdmissionAnimation() {
            if (startOffset == Vector3.zero && startScaleRate ==1) {
                yield break;
            }
//            var endPos = transform.localPosition;
            var trans = transform;
            var startPos = trans.localPosition + startOffset;
            var finalScale = trans.localScale;
            trans.localPosition = startPos;
            trans.localScale *= startScaleRate;
            transform.TweenScale(finalScale, durationShow).SetEase(easeActive);
            transform.TweenMoveOffsetLocal(- startOffset, durationShow).SetEase(easeActive);
            yield return null;
        }
    }
}