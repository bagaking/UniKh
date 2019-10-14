/** TweenExtensionTransform.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 21:18:12
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;

namespace UniKh.core.tween {
    
    public static class TweenExtensionTransform {

        public static Transform TweenMoveY(this Transform trans, float value, float duration) {
            var tweener = new Tweener<float>(
                () => trans.position.y,
                val => trans.position = trans.position + new Vector3(0,val,0),
                trans.position.y,
                trans.position.y + value,
                EvaluateFloat.Inst
            );
            Tween.LazyInst.Activate(tweener.SetMove(duration));
            return trans;
        }
    }
}