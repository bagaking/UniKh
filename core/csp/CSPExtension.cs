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
                if (null == ret) {
                    ret = new List<object>();
                }

                ret.Push((payload.Current as Result).Val);
            }

            return ret;
        }
  
        public static Promise<TVal> AsPromise<TVal>(this AsyncOperation yieldInstruction, TVal defaultVal) {
            var promise = new Promise<TVal>(); 
            YieldOnce(yieldInstruction, promise, defaultVal).Go();
            return promise;
        } 

        public static Promise<TVal> AsPromise<TVal>(this CustomYieldInstruction yieldInstruction, TVal defaultVal) {
            var promise = new Promise<TVal>(); 
            YieldOnce(yieldInstruction, promise, defaultVal).Go();
            return promise;
        }

        public static Promise<TVal> AsPromise<TVal>(this WaitForSeconds yieldInstruction, TVal defaultVal) {
            var promise = new Promise<TVal>(); 
            YieldOnce(yieldInstruction, promise, defaultVal).Go();
            return promise;
        }

        public static Promise<TVal> AsPromise<TVal>(this WaitForEndOfFrame yieldInstruction, TVal defaultVal) {
            var promise = new Promise<TVal>(); 
            YieldOnce(yieldInstruction, promise, defaultVal).Go();
            return promise;
        }

        public static Promise<TVal> AsPromise<TVal>(this IEnumerator yieldInstruction, TVal defaultVal) {
            var promise = new Promise<TVal>(); 
            YieldOnce(yieldInstruction, promise, defaultVal).Go();
            return promise;
        }

        public static Promise<object> AsPromise(this CustomYieldInstruction yieldVal) {
            var promise = new Promise<object>(); 
            YieldOnce(yieldVal, promise, null).Go();
            return promise;
        }
        
        public static IEnumerator YieldOnce<T, TVal>(T yieldVal, Promise<TVal> promise, TVal defaultVal) {
            yield return yieldVal;
            try {
                promise.Resolve(defaultVal); 
            }
            catch (Exception exception) {
                promise.Reject(exception);
            }
        }
         
         
    }
}