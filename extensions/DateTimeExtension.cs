/** == DateTimeExtension.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/08 12:10:09
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.core;

namespace UniKh.extensions {
    public static class DateTimeExtension {
        private static readonly DateTime StartTimeForTimeSamp = new DateTime(1970, 1, 1);
        
        public static long GetTimeStamp(this DateTime dateTime) { 
            //var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
            return (long)(dateTime - StartTimeForTimeSamp).TotalMilliseconds; 
        }
        
    }
}