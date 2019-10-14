/** Tweener.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 16:45:50
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UniKh.core;
using UniKh.extensions;

namespace UniKh.core.tween {
    public abstract class Tweener {
        // Pos related apis
        public enum State {
            None = 0,
            Prepared = 1,
            Active = 2,
            Complete = 3,
        }

        // Runtime Setting
        public Easing Ease { get; protected set; } = EaseLinear.Default;

        public float Duration { get; protected set; }

        // Runtime Values
        public State Status { get; internal set; }
        public float TweenPos { get; protected set; }
        public abstract Tweener MoveTo(float tweenPos, State terminal = State.None);

        public Tweener SetMove(float duration, Easing ease = null) {
            this.Duration = duration;
            this.TweenPos = 0;
            this.Status = State.Prepared;
            if (ease != null) this.Ease = ease;
            return this;
        }
    }

    public class Tweener<TVal> : Tweener {
        // Static Setting
        public Func<TVal> Getter { get; private set; }

        public Action<TVal> Setter { get; private set; }

        // Runtime Setting
        public TVal ValFrom { get; private set; }
        public TVal ValTo { get; private set; }
        public TweenEvaluator<TVal> Evaluator { get; private set; }

        public Tweener(
            Func<TVal> getter,
            Action<TVal> setter,
            TVal valFrom,
            TVal valTo,
            TweenEvaluator<TVal> evaluator = null
        ) {
            this.Setter = setter;
            this.Getter = getter;
            this.ValFrom = valFrom;
            this.ValTo = valTo;
        }

        public Tweener(
            Func<TVal> getter,
            Action<TVal> setter,
            TVal valTo,
            TweenEvaluator<TVal> evaluator = null
        ) : this(getter, setter, getter(), valTo, evaluator) { }


        public override Tweener MoveTo(float tweenPos, State terminal) {
            this.Setter(this.Evaluator.Evaluate(ValFrom, ValTo, Ease.Convert(tweenPos)));
            this.Status = terminal; // todo: state convertion
            return this;
        }
    }
}