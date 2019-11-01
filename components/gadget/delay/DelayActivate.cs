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
    
    public class DelayActivate : DelayBase<DelayActivate> {
        
        [Header("Target GameObjects")]
        public List<GameObject> targets;
        
        [Header("Accuracy => Performance")]
        public bool meanwhile = false;
 
        public override IEnumerator DoDelayEvent() {
            if(targets == null) yield break;
            foreach (var go in targets) {
                if (null == go) continue;
                go.SetActive(true);
                if(meanwhile) continue;
                yield return null;
            }
        }
 
    }
}