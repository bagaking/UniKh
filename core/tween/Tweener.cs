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

        public enum Directions {
            Forward = 1,
            Backward = 2,
            PingPong = 3,
        }
        
        // Runtime Setting
        public Easing Ease { get; protected set; } = EaseLinear.Default;

        public Directions Direction { get; protected set; } = Directions.Forward;

        public float Duration { get; protected set; } = 0;

        public int Loop { get; protected set; } = 1;
        
        // Runtime Values
        public State Status { get; internal set; }
        public float TweenPos { get; protected set; }
        public int FinishedLoops { get; protected set; }
        
        
        public abstract Tweener MoveTo(float tweenPos);

        public Tweener SetMove(float duration, Easing ease = null) {
            this.Duration = duration;
            this.TweenPos = 0;
            this.Status = State.Prepared;
            if (ease != null) this.Ease = ease;
            return this;
        }

        public Tweener SetLoop(int loop) {
            Loop = loop;
            return this;
        }
        
        public Tweener SetDirection(Directions direction) {
            Direction = direction;
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

        public override Tweener MoveTo(float tweenPos) {
            
            FinishedLoops = Mathf.FloorToInt(tweenPos / Duration);
            if (Loop > 0 && FinishedLoops >= Loop) { // todo: consider about this
                StateTransition(State.Complete);
            }

            var inLoopPos = tweenPos % Duration; 
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

        protected State StateTransition(State terminal) {// todo: state conversion
            if (terminal == State.None) {
                throw new Exception("Tweener.StateTransition failed: cannot transfer to State.None");
            }
            
            return this.Status = terminal; 
        }
    }
}