/** EaseLinear.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 19:00:02
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

namespace UniKh.core.tween {
    
    public class EaseLinear: Easing {

        public static readonly EaseLinear Default = new EaseLinear();
        
        public override float Convert(float convert) {
            return convert;
        }
    }
}