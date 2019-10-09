/** Operation.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/08 17:43:51
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniKh.core;
using UniKh.extensions;
using UniKh.utils;
using System;

namespace UniKh.core.csp.waiting {

    public class Result {

        public object Val { get; private set; }

        public Result(object val) {
            Val = val;
        }
    }

    public abstract class WaitingOperation {

        public long TimeStart { get; private set; }

        public void SetStartTimeMS(long timeMS) {
            TimeStart = timeMS;
        }

        public abstract bool IsExpired(long timeMS);


    }

    public abstract class WaitingOperation<T> : WaitingOperation where T : WaitingOperation<T> {

        public static List<T> pool = new List<T>(); // todo
    }

    public class Skip : WaitingOperation<Skip> {

        public static Skip General { get; } = new Skip(1);

        public uint frame_count { get; private set; }

        public long frame_created { get; private set; }

        public Skip(uint frameCountToSkip = 1) {
            Restart(frameCountToSkip);
        }
        public Skip Restart(uint frameCountToSkip = 1) {
            this.frame_count = frameCountToSkip;
            frame_created = CSP.LazyInst.total_system_frame;
            return this;
        }

        public override bool IsExpired(long timeMS) {
            return CSP.LazyInst.total_system_frame >= frame_created + frame_count;
        }

        public override string ToString() {
            return SGen.New["uniKh.csp.waiting.Skip("][frame_count][")from:"][frame_created].End;
        }
    }

    public class Condition : WaitingOperation<Condition> {

        public Predicate<long> condition { get; private set; }

        public Condition(Predicate<long> condition) {
            Restart(condition);
        }

        public Condition Restart(Predicate<long> condition) {
            this.condition = condition;
            return this;
        }


        public override bool IsExpired(long timeMS) { // todo: persistence this
            return condition == null || condition(timeMS - TimeStart);
        }

        public override string ToString() {
            return "uniKh.csp.waiting.Condition";
        }
    }

    public class UnityAsync : WaitingOperation<UnityAsync> {

        public AsyncOperation asyncOp { get; private set; }

        public UnityAsync(AsyncOperation asyncOp) {
            Restart(asyncOp);
        }

        public UnityAsync Restart(AsyncOperation asyncOp) {
            this.asyncOp = asyncOp;
            return this;
        }

        public override bool IsExpired(long timeMS) {
            return asyncOp == null || asyncOp.isDone;
        }

        public override string ToString() {
            return asyncOp.ToString();
        }
    }

    public class UnityCustomYieldInstruction : WaitingOperation<UnityCustomYieldInstruction> {

        public CustomYieldInstruction customYield { get; private set; }

        public UnityCustomYieldInstruction(CustomYieldInstruction customYield) {
            Restart(customYield);
        }

        public UnityCustomYieldInstruction Restart(CustomYieldInstruction customYield) {
            this.customYield = customYield;
            return this;
        }

        public override bool IsExpired(long timeMS) {
            return customYield == null || !customYield.keepWaiting;
        }

        public override string ToString() {
            return customYield.ToString();
        }
    }

    public class RealSecond : WaitingOperation<RealSecond> {

        public static RealSecond General { get; } = new RealSecond(0);

        public float FixedTimeS { get; private set; }

        public RealSecond(float timeSpanS) {
            Restart(timeSpanS);
        }

        public RealSecond Restart(float timeSpanS) {
            FixedTimeS = timeSpanS;
            return this;
        }

        public override bool IsExpired(long timeMS) {
            return timeMS >= TimeStart + (FixedTimeS * 1000);
        }

        public override string ToString() {
            return SGen.New["uniKh.csp.waiting.RealSecond("][FixedTimeS][")"].End;
        }
    }


    public class UnitySecond : WaitingOperation<RealSecond> {

        public static UnitySecond General { get; } = new UnitySecond(0);

        public float TimeSpanS { get; private set; }

        public float UnityTimeStartS { get; private set; }

        public UnitySecond(float timeSpanS) {
            Restart(timeSpanS);
        }

        public UnitySecond Restart(float timeSpanS) {
            TimeSpanS = timeSpanS;
            UnityTimeStartS = Time.time;
            return this;
        }

        public override bool IsExpired(long timeMS) {
            return Time.time >= UnityTimeStartS + TimeSpanS;
        }

        public override string ToString() {
            return SGen.New["uniKh.csp.waiting.UnitySecond("][TimeSpanS][")"].End;
        }
    }

}

