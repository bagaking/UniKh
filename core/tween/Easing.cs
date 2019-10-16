/** Easing.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 18:48:51
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

namespace UniKh.core.tween {
    
    public abstract class Easing {

        public abstract float Convert(float convert);
    }
    
    /// <summary>
    /// Easings Reference
    /// </summary>
    /// <see cref="https://easings.net/">Easings Reference</see>
    public abstract class Easing<TEase> : Easing where TEase : Easing<TEase>, new() {
        
        public static readonly TEase Default = new TEase();
    }
}