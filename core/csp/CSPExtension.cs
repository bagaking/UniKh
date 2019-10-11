/** CSPExtension.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/11 13:56:01
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

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
    }
}

