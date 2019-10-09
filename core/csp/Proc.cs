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
    using waiting;

    public class Proc : CustomYieldInstruction {

        public static int IDCounter { get; private set; } = 1;
        public int ID { get; private set; }
        public string Tag { get; private set; }
        public long timeLatestExecute { get; private set; }
        public long monitLatestTickFrameCount { get; private set; }

        public bool isActive { get; private set; }
        public override bool keepWaiting => isActive;

        public List<IEnumerator> ProcStack { get; } = new List<IEnumerator>();

        public List<object> Channel { get; } = new List<object>();

        public WaitingOperation GetOpCurr(long realTimeMS = 0) {
            if (null != m_opCurr && m_opCurr.IsExpired(realTimeMS > 0 ? realTimeMS : CSP.LazyInst.sw.ElapsedMilliseconds)) {
                m_opCurr = null;
            }
            return m_opCurr;
        }
        public WaitingOperation SetOpCurr(WaitingOperation op) {
            op.SetStartTimeMS(timeLatestExecute);
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
            long time_start = CSP.LazyInst.sw.ElapsedMilliseconds;
            long span = 0;

            monitLatestTickFrameCount = 0;

            while (isActive && (timeLatestExecute - time_start) <= maxFrameTime) {
                monitLatestTickFrameCount++;
                var shouldContinue = Frame(time_start);
                if (!shouldContinue) break;
                //if (PrintInfo != null && latest_ret != null && latest_ret is string) {
                //    PrintInfo(Helper.SGen.New[id]["-"][total_system_frame]["|span["][time_start]["->"][time_last_execute]["("][span][")]: "][latest_ret].End);
                //}
            }

            return monitLatestTickFrameCount;
        }

        public bool Frame(long realTimeMS) {

            if (null != GetOpCurr(realTimeMS)) { // 存在 Operation 则退出
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

            timeLatestExecute = CSP.LazyInst.sw.ElapsedMilliseconds;

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
                SetOpCurr(UnitySecond.General.Restart(value));
                return false;
            } else if (yieldVal is uint) {
                float value = ((uint)yieldVal);
                SetOpCurr(UnitySecond.General.Restart(value));
                return false;
            } else if (yieldVal is float) {
                SetOpCurr(UnitySecond.General.Restart((float)yieldVal));
                return false;
            } else if (yieldVal is double) {
                SetOpCurr(UnitySecond.General.Restart((float)(double)yieldVal));
                return false;
            } else if (yieldVal is decimal) {
                SetOpCurr(RealSecond.General.Restart((float)(decimal)yieldVal));
                return false;
            } else if (yieldVal is CustomYieldInstruction) {
                SetOpCurr(new UnityCustomYieldInstruction(yieldVal as CustomYieldInstruction));
                return false;
            } else if (yieldVal is AsyncOperation) {
                SetOpCurr(new UnityAsync(yieldVal as AsyncOperation));
                return false;
            } else { // yieldVal == null or undefined 
                SetOpCurr(Skip.General.Restart()); //yield return null 的情况跳过一帧
                return false;
            }
        }


    }
}

