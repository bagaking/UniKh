/** == MotionWindow.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/12/28 02:18:53
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.extensions;

using System.Collections;
using UniKh.core;
using UniKh.core.csp;
using UniKh.core.csp.waiting;
using UniKh.core.tween;
using UniKh.utils.Inspector;
using UnityEngine;

namespace UniKh.comp.ui {
    public class MotionWindow : MotionObject<MotionWindow> {

        [Header("Motion Setting")]
        public Vector3 startOffset = Vector3.zero;
        public float startScaleRate = 1;

        private Vector3 _initPosition = Vector3.zero;
        private Vector3 _initScale = Vector3.one;

        public bool phase;// { get; private set; } = false;

        public bool inAction = false;

        protected override void OnInit() {
            base.OnInit();
            var trans = transform;
            _initPosition = trans.localPosition;
            _initScale = trans.localScale;
        }

        protected override IEnumerator PlayAdmissionAnimation() {
            if (startOffset == Vector3.zero && startScaleRate == 1) {
                yield break;
            }
            Show();
            yield return null;
        }

        protected override void OnSetActive(bool active) {
            var trans = transform;
            trans.localScale = _initScale * startScaleRate;
            trans.localPosition = _initPosition + startOffset;
            base.OnSetActive(active);
        }

        private Tweener ExecuteAnimation(Tweener.Directions direction, Action<Tweener.State, Tweener.State> cb) {
            transform.TweenScale(_initScale * startScaleRate, _initScale, durationShow).SetEase(easeActive).SetDirection(direction).MoveTo(0);
            var tweener = transform.TweenMoveLocal(_initPosition + startOffset, _initPosition, durationShow).SetEase(easeActive).SetDirection(direction).MoveTo(0);
            tweener.OnStateChanged += cb;
            return tweener;
        }

        [Btn(true)]
        public Promise<object> Show() {
            if (startOffset == Vector3.zero && startScaleRate == 1) {
                return Skip.New.Start().AsPromise();
            }

            if (!gameObject.activeSelf) {
                this.SetObjectActive();
                return Skip.New.Start().AsPromise(); // When this function 'Show' is called directly, OnInit has not been called yet.
            }

            if (inAction) {
                return Condition.New.Start(_ => !inAction).AsPromise();
            }
            inAction = true;

            return ExecuteAnimation(Tweener.Directions.Forward, (from, to) => {
                if (to <= Tweener.State.Active) return;
                inAction = false;
                phase = true;
            }).AsPromise().Then(_ => (object)_);
        }


        [Btn(true)]
        private Promise<object> hide() {
            return Hide(1);
        }

        public Promise<object> Hide(float durationScale = 1) {
            if (startOffset == Vector3.zero && Math.Abs(startScaleRate - 1) < 0.0001f) {
                return Skip.New.Start().AsPromise();
            }

            if (inAction) {
                return Condition.New.Start(_ => !inAction).AsPromise();
            }

            inAction = true;
            return ExecuteAnimation(
                Tweener.Directions.Backward,
                (from, to) => {
                    if (to <= Tweener.State.Active) return;
                    inAction = false;
                    phase = false;
                }
            ).SetDuration(durationShow * durationScale).AsPromise().Then(_ => (object)_);
        }
    }
}