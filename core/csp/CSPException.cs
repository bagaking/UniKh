/** CspException.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/11 13:56:01
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */


using System;

namespace UniKh.core.csp {
    using waiting;

    public class CspException : Exception {
        public CspException() { }
        public CspException(string message) : base(message) { }
        public CspException(string message, Exception innerException) : base(message, innerException) { }
    }
}

