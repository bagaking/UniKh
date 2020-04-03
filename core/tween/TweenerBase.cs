/** TweenerBase.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 16:45:50
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System; 

namespace UniKh.core.tween {
    
    public abstract class Tweener {
        // Pos related apis
        public enum State {
            None = 0,
            Prepared = 1,
            Active = 2,
            Complete = 200,
            Terminated = 201,
            Error = 500,
        }

        public enum Directions {
            Forward = 1,
            Backward = 2,
            PingPong = 3,
        }
        
        // events
        public event Action<State, State> OnStateChanged;
        
        // Runtime Setting
        public Easing Ease { get; protected set; } = EaseLinear.Default;

        public Directions Direction { get; protected set; } = Directions.Forward;

        public float Duration { get; protected set; } = 0;

        public int Loop { get; protected set; } = 1;
        
        // Runtime Values
        public State Status { get; protected set; }
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
        
        public Tweener SetDuration(float duration) {
            Duration = duration;
            return this;
        }
        
        public Tweener SetEase(Easing ease) {
            Ease = ease;
            return this;
        }
        
        public Tweener SetEase(StandardEase.Type type) {
            Ease = StandardEase.Get(type);
            return this;
        }
        
        public State StateTransition(State terminal) {// todo: state conversion
            if (terminal == State.None) {
                throw new Exception("Tweener.StateTransition failed: cannot transfer to State.None");
            }
            this.Status = terminal; 
            OnStateChanged?.Invoke(this.Status, terminal);
            return this.Status;
        }

        public void Terminate() {
            StateTransition(State.Terminated);
        }

        public Promise<Tweener> AsPromise() {
            var promise = new Promise<Tweener>();
            
            this.OnStateChanged += (from, to) => {
                if (to == State.Complete) {
                    promise.Resolve(this);
                } else if(to == State.Terminated || to == State.Error) {
                    promise.Reject(new Exception("Tweener state fall into " + to));
                }
            };

            return promise;
        }
        
    }
}