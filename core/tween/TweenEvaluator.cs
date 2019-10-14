/** TweenEvaluator.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 16:45:50
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

namespace UniKh.core.tween {
    
    public abstract class TweenEvaluator<TVal> {
            
        public abstract TVal Evaluate(TVal from, TVal to, float evaluatePos);
    }
    
    public abstract class TweenEvaluator<TVal, TEvaluator>: TweenEvaluator<TVal> where TEvaluator : TweenEvaluator<TVal, TEvaluator>, new() {

        public static TweenEvaluator<TVal, TEvaluator> Inst = new TEvaluator();
    }
}