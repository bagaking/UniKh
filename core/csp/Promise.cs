/** == Promise.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/04 13:20:44
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.core;
using UniKh.core.csp;
using UniKh.core.csp.waiting;
using UnityEngine;

namespace UniKh.core {
    
    public interface IPromise<TVal> {
        Promise<TVal> Resolve(TVal val);
        Promise<TVal> Reject(Exception exception);
    }

    public class Promise<TVal> : CustomYieldInstruction, IPromise<TVal> {
        
        private Action<TVal> _resolve;
        private Action<Exception> _reject;

        private bool _executed = false;

        public Promise<TVal> Resolve(TVal val) {
            if (_executed) return this; // only execute once 
            Skip.New.Start().Go(() => _resolve?.Invoke(val));
            _executed = true;
            return this;
        }
 
        public Promise<TVal> Reject(Exception exception) {
            if (_executed) return this; // only execute once
            Skip.New.Start().Go(() => _reject?.Invoke(exception));
            _executed = true;
            return this;
        }

        public Promise<TVal> Verbose(Action<TVal> cbThen) {
            _resolve += cbThen;
            return this;
        }
        
        public Promise<TRet> Then<TRet>(Func<TVal, TRet> cbThen) {
            var nextPromise = new Promise<TRet>();
            _resolve += (val) => {
                var ret = cbThen(val);
                nextPromise.Resolve(ret);
            };
            _reject += ex => {
                nextPromise.Reject(ex);
            };
            return nextPromise;
        }
        
        public Promise<TVal> Then(Action<TVal> cbThen) {
            var nextPromise = new Promise<TVal>();
            _resolve += (val) => {
                cbThen(val);
                nextPromise.Resolve(val);
            };
            _reject += ex => {
                nextPromise.Reject(ex);
            };
            return nextPromise;
        }
        
        public Promise<TRet> Then<TRet>(Func<TVal, Promise<TRet>> cbThenPromise) {
            var nextPromise = new Promise<TRet>(); 
            _resolve += (val) => {
                var anotherPromise = cbThenPromise(val);
                anotherPromise.Then(ret => nextPromise.Resolve(ret));
            };
            _reject += ex => {
                nextPromise.Reject(ex);
            };
            return nextPromise;
        }

        public Promise<TVal> Catch(Action<Exception> cbCatch) {
//            var nextPromise = new Promise<TVal>();
            _reject += cbCatch;
            return this;
        }

        public override bool keepWaiting => _executed;
    }
}