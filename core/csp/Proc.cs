/** Proc.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/08 17:43:51
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

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
        public override bool keepWaiting => isActive;

        public List<IEnumerator> ProcStack { get; } = new List<IEnumerator>();

        public List<object> Channel { get; } = new List<object>();

        public WaitingOperation GetOpCurr(long realTimeMS = 0) {
            if (null != m_opCurr && m_opCurr.IsExpired(realTimeMS > 0 ? realTimeMS : CSP.LazyInst.sw.ElapsedMilliseconds)) {
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


        internal Proc(IEnumerator _procedure, string _tag = "_") {
            ProcStack.PushLast(_procedure);
            ID = IDCounter++;
            Tag = _tag;
        }

        internal Proc Start() {
            isActive = true;
            return this;
        }

        internal Proc End() {
            isActive = false;
            return this;
        }


        public long Tick(long maxFrameTime = CONST.TIME_SPAN_MS_MIN) {
            long timeStart = CSP.LazyInst.sw.ElapsedMilliseconds;
            ExecutedTime = timeStart;
            //long span = 0;

            MonitTickFrameCount = 0;

            while (isActive && (ExecutedTime - timeStart) <= maxFrameTime) {
                MonitTickFrameCount++;
                var shouldContinue = Frame(timeStart);
                if (!shouldContinue) break;
                //if (PrintInfo != null && latest_ret != null && latest_ret is string) {
                //    PrintInfo(Helper.SGen.New[id]["-"][total_system_frame]["|span["][time_start]["->"][time_last_execute]["("][span][")]: "][latest_ret].End);
                //}
            }
            MonitTickTimeCost = ExecutedTime - timeStart;

            return MonitTickFrameCount;
        }

        public bool Frame(long realTimeMS) {

            if (null != GetOpCurr(realTimeMS)) { // ´æÔÚ Operation ÔòÍË³ö
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

            var yieldVal = procTop.Current;

            if (yieldVal is IEnumerator) {
                ProcStack.StackPush(yieldVal as IEnumerator);
                return true;
            } else if (yieldVal is Result) {
                Channel.QueuePush((yieldVal as Result).Val);
                return true;
            } else if (yieldVal is WaitingOperation) {
                SetOpCurr(yieldVal as WaitingOperation);
                return false;
            } else if (yieldVal is int) {
                float value = ((int)yieldVal);
                SetOpCurr(UnitySecond.New.Start(value));
                return false;
            } else if (yieldVal is uint) {
                float value = ((uint)yieldVal);
                SetOpCurr(UnitySecond.New.Start(value));
                return false;
            } else if (yieldVal is float) {
                SetOpCurr(UnitySecond.New.Start((float)yieldVal));
                return false;
            } else if (yieldVal is double) {
                SetOpCurr(UnitySecond.New.Start((float)(double)yieldVal));
                return false;
            } else if (yieldVal is decimal) {
                SetOpCurr(RealSecond.New.Start((float)(decimal)yieldVal));
                return false;
            } else if (yieldVal is CustomYieldInstruction) {
                SetOpCurr(UnityCustomYieldInstruction.New.Start(yieldVal as CustomYieldInstruction));
                return false;
            } else if (yieldVal is AsyncOperation) {
                SetOpCurr(UnityAsync.New.Start(yieldVal as AsyncOperation));
                return false;
            } else if (yieldVal is WaitForSeconds) {
                var Ts = (yieldVal as WaitForSeconds).GetType();
                var field = Ts.GetField("m_Seconds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var sec = field.GetValue(yieldVal);
                SetOpCurr(UnitySecond.New.Start((float)sec));
                return false;
            } else if (yieldVal is WaitForEndOfFrame) { // WaitForEndOfFrame: skip this frame, but try execute in the next tickFrame
                SetOpCurr(Skip.New.Restart()); // todo: This implementation is incomplete, and it needs to be reconsidered in the future.
                return true; 
            } else if(yieldVal is string) { // wait a frame and show this string
                Debug.Log(yieldVal);
                SetOpCurr(Skip.New.Restart());
                return false;
            } else if (yieldVal is null) { // skip this frame
                SetOpCurr(Skip.New.Restart());
                return false;
            } else { // yieldVal == null or undefined 
                Debug.LogError(SGen.New["yield return value of type "][yieldVal.GetType()]["are not supported."].End);
                End();
                return false;
            }
        }


    }
}

