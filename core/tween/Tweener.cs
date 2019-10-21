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
            this.Evaluator = evaluator; // != null ? evaluator : DefaultTweenEvaluators.Get<TVal>();
        }

        public Tweener(
            Func<TVal> getter,
            Action<TVal> setter,
            TVal valTo,
            TweenEvaluator<TVal> evaluator = null
        ) : this(getter, setter, getter(), valTo, evaluator) { }

        public override Tweener MoveTo(float tweenPos) {
            if (Duration <= 0) {
                throw new Exception("Tweener.MoveTo failed: duration error");
            }
            
            if (Status != State.Active) {
                return this;
            }
            
            TweenPos = tweenPos;
            FinishedLoops = Mathf.FloorToInt(TweenPos / Duration);
            if (Loop > 0 && FinishedLoops >= Loop) { // todo: consider about this
                StateTransition(State.Complete);
            }

            var inLoopPos = TweenPos % Duration; 
            if (this.Status == State.Complete) { // edge condition: 0 or Duration(max) ?
                switch (Direction) { // if the tween is completed
                    case Directions.Forward: // In forward mode, the tween pos should be the duration * loops 
                        inLoopPos = Duration;
                        break;
                    case Directions.Backward: // In Backward mode, the tween pos should be the 0
                        inLoopPos = 0;
                        break;
                    case Directions.PingPong:
                        inLoopPos = Loop % 2 == 0 ? 0 : Duration;
                        break;
                }
            }
            var posScale = inLoopPos / Duration;
            
            this.Setter(this.Evaluator.Evaluate(ValFrom, ValTo, Ease.Convert(posScale)));

            return this;
        }

        
    }
}