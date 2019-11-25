/** Proc.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/08 17:43:51
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniKh.core;
using UniKh.extensions;


namespace UniKh.core.csp {
    using UniKh.utils;
    using waiting;

    public class Proc : CustomYieldInstruction {
        public static int IDCounter { get; private set; } = 1;
        public int ID { get; private set; }
        public string Tag { get; private set; }
        public long ExecutedTime { get; private set; }
        public long MonitTickFrameCount { get; private set; }
        public long MonitTickTimeCost { get; private set; }

        public bool isActive { get; private set; }
        
        public bool isFinished { get; private set; } = false;
        
        public override bool keepWaiting => !isFinished;

        public List<IEnumerator> ProcStack { get; } = new List<IEnumerator>();

        public List<object> Channel { get; } = new List<object>();

        public WaitingOperation GetOpCurr(long realTimeMS = 0) {
            if (null != m_opCurr &&
                m_opCurr.IsExpired(realTimeMS > 0 ? realTimeMS : CSP.LazyInst.sw.ElapsedMilliseconds)) {
                m_opCurr.Recycle();
                m_opCurr = null;
            }

            return m_opCurr;
        }

        public WaitingOperation SetOpCurr(WaitingOperation op) {
            op.SetStartTimeMS(ExecutedTime);
            return m_opCurr = op;
        }

        private WaitingOperation m_opCurr;

        internal Func<bool> Validator { get; private set; } = null;

        internal Proc(IEnumerator _procedure, string _tag = "_", Func<bool> fnValidate = null) {
            ProcStack.PushLast(_procedure);
            ID = IDCounter++;
            Tag = _tag;
            Validator = fnValidate;
        }

        internal Proc Start() {
            isActive = true;
            return this;
        }

        internal Proc End() {
            isActive = false;
            isFinished = true;
            return this;
        }


        public long Tick(long maxFrameTime = CONST.TIME_SPAN_MS_MIN) {
            var timeStart = CSP.LazyInst.sw.ElapsedMilliseconds;
            MonitTickFrameCount = 0;
            
            ExecutedTime = timeStart;
            while (isActive && (ExecutedTime - timeStart) <= maxFrameTime) {
                if (null != GetOpCurr(ExecutedTime)) break;
                MonitTickFrameCount++;
                var shouldContinue = Frame(timeStart);
                if (!shouldContinue) break;
            }

            MonitTickTimeCost = ExecutedTime - timeStart;
            return MonitTickFrameCount;
        }

        public bool Frame(long realTimeMs) {
            if (null != GetOpCurr(realTimeMs)) {
                return false;
            }

            IEnumerator procTop = null;
            while (ProcStack.Count > 0 && null == procTop) {
                procTop = ProcStack[ProcStack.Count - 1];
                if (!procTop.MoveNext()) {
                    ProcStack.PopLast();
                    procTop = null;
                }
            }

            if (null == procTop) {
                End();
                return false;
            }

            ExecutedTime = CSP.LazyInst.sw.ElapsedMilliseconds;

            return EnqueueOperation(procTop.Current);
        }

        public bool EnqueueOperation(object yieldVal) {
            
            if (yieldVal is null) {
                // skip this frame
                return EnqueueOperation(Skip.New.Start());
            }
            
            if (yieldVal is WaitingOperation) {
                // WaitingOperation < CustomYieldInstruction < IEnumerator, the default 
                SetOpCurr(yieldVal as WaitingOperation);
                return false;
            }

            if (yieldVal is CustomYieldInstruction) {
                SetOpCurr(UnityCustomYieldInstruction.New.Start(yieldVal as CustomYieldInstruction));
                return false;
            }
            
            if (yieldVal is IEnumerator) {
                ProcStack.StackPush(yieldVal as IEnumerator);
                return false;
            }

            if (yieldVal is Result) {
                Channel.QueuePush((yieldVal as Result).Val);
                return true;
            }

            if (yieldVal is int) {
                float value = ((int) yieldVal);
                return EnqueueOperation(UnitySecond.New.Start(value));
            }

            if (yieldVal is uint) {
                float value = ((uint) yieldVal);
                return EnqueueOperation(UnitySecond.New.Start(value));
            }

            if (yieldVal is float) {
                return EnqueueOperation(UnitySecond.New.Start((float) yieldVal));
            }

            if (yieldVal is double) {
                return EnqueueOperation(UnitySecond.New.Start((float) (double) yieldVal));
            }

            if (yieldVal is decimal) {
                return EnqueueOperation(RealSecond.New.Start((float) (decimal) yieldVal));
            }

            if (yieldVal is AsyncOperation) {
                return EnqueueOperation(UnityAsync.New.Start(yieldVal as AsyncOperation));
            }

            if (yieldVal is WaitForSeconds) {
                var ts = (yieldVal as WaitForSeconds).GetType();
                var field = ts.GetField("m_Seconds",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var sec = field.GetValue(yieldVal);
                return EnqueueOperation(UnitySecond.New.Start((float) sec));
            }

            if (yieldVal is WaitForEndOfFrame) {
                // WaitForEndOfFrame: skip this frame, but try execute in the next tickFrame
                return
                    EnqueueOperation(Skip.New
                        .Start()); // todo: This implementation is incomplete, and it needs to be reconsidered in the future.
            }

            if (yieldVal is string) {
                // wait a frame and show this string 
                Debug.Log(yieldVal);
                return EnqueueOperation(Skip.New.Start());
            }
 
            Debug.LogError(SGen.New["yield return value of type "][yieldVal.GetType()]["are not supported."].End);
            End();
            return false; 
        }

        
        public Proc Delay(WaitingOperation waitionOp) {
            if (!CSP.Inst) {
                Debug.LogError("UniKH/CSP/Proc: delay proc failed, CSP are not loaded.");
            }

            if (isActive) {
                Debug.LogError("UniKH/CSP/Proc: delay proc failed, this proc is currently running.");
            }

            if (GetOpCurr() != null) {
                Debug.LogError(
                    "UniKH/CSP/Proc: delay proc failed, delay object are already set. Maybe you can use QueueJumping to achieve similar effects.");
            }

            m_opCurr = waitionOp;
            m_opCurr.SetStartTimeMS(CSP.Inst.sw.ElapsedMilliseconds);
            return this;
        }

        public Proc QueueJump(IEnumerator queueJumper) {
            if (isActive) {
                Debug.LogError("UniKH/CSP/Proc: delay proc failed, this proc is currently running.");
            }
            ProcStack.StackPush(queueJumper);
            return this;
        }
        
    }
}