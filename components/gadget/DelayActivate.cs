/** == DelayActivate.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/29 00:06:35
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.core;
using UniKh.extensions;
using UniKh.utils;

using System.Collections;
using System.Collections.Generic;
using UniKh.core.csp;
using UnityEngine;

namespace UniKh.comp {
    public class DelayActivate : BetterBehavior {

        public enum TriggerOn {
            OnInit = 0,
            OnActive = 1,
            OnClose = 2
        }

        public TriggerOn triggerOn = TriggerOn.OnActive;
        public float delaySec = 1;
        public List<GameObject> targets;
        public bool meanwhile = false;

        public IEnumerator DoActive() {
            if(targets == null) yield break;
            foreach (var go in targets) {
                if (null == go) continue;
                go.SetActive(true);
                if(meanwhile) continue;
                yield return null;
            }
        }

        protected override void OnInit() {
            base.OnInit();
            if (triggerOn == TriggerOn.OnInit) {
                DoActive().Go("DelayActivate:OnInit").Delay(core.csp.waiting.UnitySecond.New.Start(delaySec));
            }
        }

        /// <summary>
        /// when the active state changes, the method will be call.
        /// This method **CAN** be removed
        /// </summary>
        protected override void OnSetActive(bool active) {
            if (active) {
                if (triggerOn == TriggerOn.OnActive) {
                    DoActive().Go("DelayActivate:OnActive").Delay(core.csp.waiting.UnitySecond.New.Start(delaySec));
                }
            }
            else {
                if (triggerOn == TriggerOn.OnClose) {
                    DoActive().Go("DelayActivate:OnClose").Delay(core.csp.waiting.UnitySecond.New.Start(delaySec));
                }
            }
        }
 

    }
}