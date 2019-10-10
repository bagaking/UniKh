/** TestCSP.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/08 19:24:25
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniKh.core;
using UniKh.extensions;
using UniKh.core.csp;
using UniKh.core.csp.waiting;

public class ExampleCSP : BetterBehavior {

    /// <summary>
    /// The initial method of TestCSP.
    /// This method **CAN** be removed
    /// </summary>
    protected override void OnSetActive(bool active) {
        CSP.LazyInst.Do(Go());
    }

    protected IEnumerator Go() {
        yield return Test1();
        yield return Test2();
        yield return Test3();
    }

    protected IEnumerator Test1() {
        Time.timeScale = 0.5f;
        var ind = 1;
        Debug.Log($"Test 1.{ind++}: {Time.time} : {CSP.LazyInst.sw.ElapsedMilliseconds}");
        yield return 1; // wait for unity time - 1 sec
        Debug.Log($"Test 1.{ind++}: {Time.time} : {CSP.LazyInst.sw.ElapsedMilliseconds}");
        yield return 1.2f; // wait for unity time - 1.2 sec
        Debug.Log($"Test 1.{ind++}: {Time.time} : {CSP.LazyInst.sw.ElapsedMilliseconds}");
        yield return 1.5m; // wait for real time - 1.5 sec
        Debug.Log($"Test 1.{ind++}: {Time.time} : {CSP.LazyInst.sw.ElapsedMilliseconds}");
        yield return null; // wait for unity frame - 1
        Debug.Log($"Test 1.{ind++}: {Time.time} : {CSP.LazyInst.sw.ElapsedMilliseconds}");
        Time.timeScale = 1;
    }

    protected IEnumerator Test2() {
        var timeNow = Time.time;
        var ind = 1;
        yield return new Condition(t => Time.time > timeNow + 1);
        Debug.Log($"Test 2.{ind++}: {timeNow} => {Time.time} : {CSP.LazyInst.sw.ElapsedMilliseconds}");
    }

    protected IEnumerator Test3() {
        var timeNow = Time.time;
        for (float i = 0, v = 1; i < 10000000; i ++) {
            v /= 2;
            v += i;
        }
        yield return 1;
        Debug.Log($"Test 3: {timeNow} => {Time.time} : {CSP.LazyInst.sw.ElapsedMilliseconds}");
    }


}
