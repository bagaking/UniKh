/** Tweener.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 16:45:50
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UnityEngine;

namespace UniKh.core.tween {
    public class Tweener<TVal> : Tweener {
        // Static Setting
        public Func<TVal> Getter { get; private set; }

        public Action<TVal> Setter { get; private set; }

        // Runtime Setting
        public TVal ValFrom { get; private set; }
        public TVal ValTo { get; private set; }
        public TweenEvaluator<TVal> Evaluator { get; private set; }

        public Func<bool> Validator { get; private set; }

        public Tweener(
            Func<TVal> getter,
            Action<TVal> setter,
            TVal valFrom,
            TVal valTo,
            TweenEvaluator<TVal> evaluator = null,
            Func<bool> fnValidate = null
        ) {
            this.Setter = setter;
            this.Getter = getter;
            this.ValFrom = valFrom;
            this.ValTo = valTo;
            this.Evaluator = evaluator; // != null ? evaluator : DefaultTweenEvaluators.Get<TVal>();
            this.Validator = fnValidate;
        }

        public Tweener(
            Func<TVal> getter,
            Action<TVal> setter,
            TVal valTo,
            TweenEvaluator<TVal> evaluator = null,
            Func<bool> fnValidate = null
        ) : this(getter, setter, getter(), valTo, evaluator, fnValidate) { }

        public override Tweener MoveTo(float tweenPos) {
            if (Duration <= 0) {
                throw new Exception("Tweener.MoveTo failed: duration error");
            }

            if (Status != State.Active) {
                return this;
            }

            if ((null != Validator) && !Validator()) {
                StateTransition(State.Terminated);
                return this;
            }

            TweenPos = tweenPos;
            FinishedLoops = Mathf.FloorToInt(TweenPos / Duration);
            if (Loop > 0 && FinishedLoops >= Loop) { // todo: consider about this
                StateTransition(State.Complete);
            }

            var inLoopPos = TweenPos % Duration;
            if (Status == State.Complete) { // edge condition: 0 or Duration(max) ?
                inLoopPos = Duration; // if the tween is completed, the tween pos should be the duration * loops 
            }

            var posScale = inLoopPos / Duration;

            if (
                Direction == Directions.Backward
                || (Direction == Directions.PingPong && (FinishedLoops & 1) == 1)
            ) {
                posScale = 1 - posScale;
            }

            Setter(Evaluator.Evaluate(ValFrom, ValTo, Ease.Convert(posScale)));

            return this;
        }
    }
}