/** == DelayBase.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/01 14:50:34
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Collections;
using UniKh.core;
using UniKh.core.csp;

namespace UniKh.comp {
    
    public abstract class DelayBase<T> : BetterBehavior where T : DelayBase<T> {

        public enum TriggerOn {
            OnInit = 0,
            OnActive = 1,
            OnClose = 2
        }

        public TriggerOn triggerOn = TriggerOn.OnActive;
        public float delaySec = 1;

        private static string tagOnInit = typeof(T).Name + ":OnInit";
        private static string tagOnActive = typeof(T).Name + ":OnActive";
        private static string tagOnClose = typeof(T).Name + ":OnClose";

        public abstract IEnumerator DoDelayEvent();

        protected override void OnInit() {
            base.OnInit();
            if (triggerOn == TriggerOn.OnInit) {
                DoDelayEvent()
                    .Go(tagOnInit)
                    .Delay(core.csp.waiting.UnitySecond.New.Start(delaySec));
            }
        }

        /// <summary>
        /// when the active state changes, the method will be call.
        /// This method **CAN** be removed
        /// </summary>
        protected override void OnSetActive(bool active) {
            if (active) {
                if (triggerOn == TriggerOn.OnActive) {
                    DoDelayEvent()
                        .Go(tagOnActive)
                        .Delay(core.csp.waiting.UnitySecond.New.Start(delaySec));
                }
            }
            else {
                if (triggerOn == TriggerOn.OnClose) {
                    DoDelayEvent()
                        .Go(tagOnClose)
                        .Delay(core.csp.waiting.UnitySecond.New.Start(delaySec));
                }
            }
        }
    }
}