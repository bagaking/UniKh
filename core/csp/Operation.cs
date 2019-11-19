/** Operation.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/08 17:43:51
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Collections.Generic;
using UnityEngine;

using UniKh.extensions;
using UniKh.utils;
using System;
using System.Collections;

namespace UniKh.core.csp.waiting {

    public class Result {

        public object Val { get; private set; }

        public Result(object val) {
            Val = val;
        }
    }

    public abstract class WaitingOperation : CustomYieldInstruction {

        public long TimeStart { get; private set; }

        public void SetStartTimeMS(long timeMS) {
            TimeStart = timeMS;
        }

        public abstract bool IsExpired(long timeMS);

        public override bool keepWaiting {
            get {
                if (null == CSP.Inst) return true;
                return !IsExpired(CSP.Inst.sw.ElapsedMilliseconds);
            }
        }

        public abstract void Recycle();

        public Proc Go(Action callback) {
            return CSP.LazyInst.Do(Yield(callback));
        }
        
        private IEnumerator Yield(Action callback) {
            yield return this;
            callback?.Invoke();
        }
    }

    public abstract class WaitingOperation<T> : WaitingOperation where T : WaitingOperation<T>, new() {

        public static List<T> pool = new List<T>(); // todo

        public static T New {
            get {
                T ret = null;
                while (null == ret && pool.Count > 0) {
                    ret = pool.StackPop();
                }
                return null == ret ? new T() : ret;
            }
        }

        public override void Recycle() {
            pool.StackPush(this as T); // todo: pool.Append(this as T); ?
        }
    }

    public class Skip : WaitingOperation<Skip> {

        public uint frame_count { get; private set; }

        public long frame_created { get; private set; }

        public Skip() { }

        public Skip Restart(uint frameCountToSkip = 1) {
            this.frame_count = frameCountToSkip;
            frame_created = CSP.LazyInst.TotalTicks;
            return this;
        }

        public override bool IsExpired(long timeMS) {
            return CSP.LazyInst.TotalTicks >= frame_created + frame_count;
        }

        public override string ToString() {
            return SGen.New["Skip("][frame_count][")from:"][frame_created].End;
        }
    }

    public class Condition : WaitingOperation<Condition> {

        public Predicate<long> condition { get; private set; }

        public Condition() { }

        public Condition Start(Predicate<long> condition) {
            this.condition = condition;
            return this;
        }


        public override bool IsExpired(long timeMS) { // todo: persistence this
            return condition == null || condition(timeMS - TimeStart);
        }

        public override string ToString() {
            return "Condition";
        }
    }

    public class UnityAsync : WaitingOperation<UnityAsync> {

        public AsyncOperation asyncOp { get; private set; }

        public UnityAsync() { }

        public UnityAsync Start(AsyncOperation asyncOp) {
            this.asyncOp = asyncOp;
            return this;
        }

        public override bool IsExpired(long timeMS) {
            return asyncOp == null || asyncOp.isDone;
        }

        public override string ToString() {
            return SGen.New["UnityAsyncOperation("][asyncOp.ToString()][")"].End;
        }
    }

    public class UnityCustomYieldInstruction : WaitingOperation<UnityCustomYieldInstruction> {

        public CustomYieldInstruction customYield { get; private set; }

        public UnityCustomYieldInstruction() { }

        public UnityCustomYieldInstruction Start(CustomYieldInstruction customYield) {
            this.customYield = customYield;
            return this;
        }
        
        // Cuz the Current field of CustomYieldInstruction always return null in waiting, there is no needs to provide this.
        // But I provided this approach to make it easier to make it clearer and easier to monitor.
        public override bool IsExpired(long timeMS) { 
            return customYield == null || !customYield.keepWaiting;
        }

        public override string ToString() {
            return customYield.ToString();
        }
    }

    public class RealSecond : WaitingOperation<RealSecond> {

        public float FixedTimeS { get; private set; }

        public RealSecond() { }

        public RealSecond Start(float timeSpanS) {
            FixedTimeS = timeSpanS;
            return this;
        }

        public override bool IsExpired(long timeMS) {
            return timeMS >= TimeStart + (FixedTimeS * 1000);
        }

        public override string ToString() {
            return SGen.New["RealSecond("][FixedTimeS][")"].End;
        }
    }


    public class UnitySecond : WaitingOperation<UnitySecond> {

        public float TimeSpanS { get; private set; }

        public float UnityTimeStartS { get; private set; }

        public UnitySecond() { }

        public UnitySecond Start(float timeSpanS) {
            TimeSpanS = timeSpanS;
            UnityTimeStartS = Time.time;
            return this;
        }

        public override bool IsExpired(long timeMS) {
            return Time.time >= UnityTimeStartS + TimeSpanS;
        }

        public override string ToString() {
            return SGen.New["UnitySecond("][TimeSpanS][")"].End;
        }
    }

}

