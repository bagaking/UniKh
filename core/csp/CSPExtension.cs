/** CSPExtension.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/11 13:56:01
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniKh.core;
using UniKh.extensions;


namespace UniKh.core.csp {
    using waiting;

    public static class CSPExtension {

        public static Proc Go(this IEnumerator payload, string tag = "_") {
            return CSP.LazyInst.Do(payload, tag);
        }

        public static List<object> GoSync(this IEnumerator payload, string tag = "_") {
            List<object> ret = null;
            while (payload.MoveNext()) {
                if (!(payload.Current is Result)) continue;
                if (null == ret) { ret = new List<object>(); }
                ret.Push((payload.Current as Result).Val);
            }
            return ret;
        }

        public static void Then(this AsyncOperation yieldInstruction, Action<AsyncOperation> callback = null) {
            YieldOnce(yieldInstruction, callback).Go();
        }
        
        public static void Then(this CustomYieldInstruction yieldInstruction, Action<CustomYieldInstruction> callback = null) {
            YieldOnce(yieldInstruction, callback).Go();
        }
        
        public static void Then(this WaitForSeconds yieldInstruction, Action<WaitForSeconds> callback = null) {
            YieldOnce(yieldInstruction, callback).Go();
        }
        
        public static void Then(this WaitForEndOfFrame yieldInstruction, Action<WaitForEndOfFrame> callback = null) {
            YieldOnce(yieldInstruction, callback).Go();
        }
        
        public static void Then(this IEnumerator yieldInstruction, Action<IEnumerator> callback = null) {
            YieldOnce(yieldInstruction, callback).Go();
        }

        public static IEnumerator YieldOnce<T>(T yieldVal, Action<T> callback = null) {
            yield return yieldVal;
            callback?.Invoke(yieldVal);
        }
    }
}

