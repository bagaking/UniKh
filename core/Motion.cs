using System;
using UnityEngine;

namespace UniKh.core {
    public class Motion<T> {

        public enum State {
            Ready,
            Running,
            Complete
        }
        
        public delegate void MotionEvent(Motion<T> m);

        public event MotionEvent OnReady;
        public event MotionEvent OnStart;
        public event MotionEvent OnComplete;
        
        public T posStart;
        public T posEnd;
        public float duration = 1;

        public Func<T, T, float, T> Evaluate;
        public Action<T> SetValue;

        public State state { get; private set; } = State.Ready;

        private float accumulateX = 0;

        public Motion(T posStart, T posEnd, float duration, Func<T, T, float, T> Evaluate, Action<T> SetValue) {
            this.posStart = posStart;
            this.posEnd = posEnd;
            this.duration = duration;
            this.Evaluate = Evaluate;
            this.SetValue = SetValue;
            Reset();
        }
        
        public void Pulse(float deltaX) {
            if (accumulateX < 0) return;
            if (State.Complete == state) return;
            accumulateX += deltaX;
            
            if (accumulateX >= duration) {
                SetValue(Evaluate(posStart, posEnd, 1));
                state = State.Complete;
                OnComplete?.Invoke(this);
            } else {
                if (State.Running != state) {
                    state = State.Running;
                    OnStart?.Invoke(this);
                }
                SetValue(Evaluate(posStart, posEnd, accumulateX / duration));
            }
            
        }

        public Motion<T> Reset() {
            SetValue(posStart);
            state = State.Ready;
            accumulateX = 0;
            OnReady?.Invoke(this);
            return this;
        }
    }
}