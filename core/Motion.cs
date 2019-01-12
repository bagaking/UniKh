using System;
using UnityEngine;

namespace UniKh.core {

    public abstract class Motion {
        
        public enum State {
            Ready,
            Running,
            Complete
        }
        
        public delegate void MotionEvent(Motion m);

        public event MotionEvent OnReady;
        public event MotionEvent OnStart;
        public event MotionEvent OnComplete;
        
        public State state { get; private set; } = State.Ready;
        
        public float duration = 1;
        protected float accumulateX = 0;
        
        public void Pulse(float deltaX) {
            if (accumulateX < 0) return;
            if (State.Complete == state) return;
            accumulateX += deltaX;
            
            if (accumulateX >= duration) {
                Update(1);
                state = State.Complete;
                OnComplete?.Invoke(this);
            } else {
                if (State.Running != state) {
                    state = State.Running;
                    OnStart?.Invoke(this);
                }
                Update(accumulateX / duration);
            }
        }

        public Motion Reset() {
            accumulateX = 0;
            Update(0);
            state = State.Ready;
            OnReady?.Invoke(this);
            return this;
        }
        
        public abstract void Update(float rate);
    }
    
    public class Motion<T> : Motion {
        public T posStart;
        public T posEnd;
        public Func<T, T, float, T> Evaluate;
        public Action<T> SetValue;

        public Motion(T posStart, T posEnd, float duration, Func<T, T, float, T> Evaluate, Action<T> SetValue) {
            this.posStart = posStart;
            this.posEnd = posEnd;
            this.duration = duration;
            this.Evaluate = Evaluate;
            this.SetValue = SetValue;
            Reset();
        }

        public override void Update(float rate) {
            SetValue(Evaluate(posStart, posEnd, rate));
        }
    }
}