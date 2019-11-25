/** TweenExtensionTransform.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 21:18:12
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;

namespace UniKh.core.tween {
    
    public static class TweenExtensionTransform {

        public static Tweener TweenMoveY(this Transform trans, float value, float duration) {
            var posOrg = trans.position;
            var tweener = new Tweener<float>(
                () => trans.position.y,
                val => {
                    var pos = trans.position;
                    trans.SetPositionAndRotation(new Vector3(pos.x, val, pos.z), trans.rotation);
                },
                posOrg.y,
                posOrg.y + value,
                EvaluateFloat.Inst,
                () => trans
            );
            return Tween.LazyInst.Activate(tweener.SetMove(duration));
        }
        
        public static Tweener TweenMoveOffset(this Transform trans, Vector3 value, float duration) {
            var posOrg = trans.position;
            var tweener = new Tweener<Vector3>(
                () => trans.position,
                val => trans.position = val,
                posOrg,
                posOrg + value,
                EvaluateUnityVector3.Inst,
                () => trans
            );
            return Tween.LazyInst.Activate(tweener.SetMove(duration));
        }
        
        public static Tweener TweenMoveOffsetLocal(this Transform trans, Vector3 offset, float duration) {
            var posOrg = trans.localPosition; 
            return trans.TweenMoveLocal(posOrg, posOrg + offset, duration);
        }
        
        public static Tweener TweenMoveLocal(this Transform trans, Vector3 to, float duration) {
            var posOrg = trans.localPosition;
            return trans.TweenMoveLocal(posOrg, to, duration);
        }
        
        public static Tweener TweenMoveLocal(this Transform trans, Vector3 from, Vector3 to, float duration) {
            var posOrg = trans.localPosition;
            var tweener = new Tweener<Vector3>(
                () => trans.localPosition,
                val => trans.localPosition = val,
                from,
                to,
                EvaluateUnityVector3.Inst,
                () => trans
            );
            return Tween.LazyInst.Activate(tweener.SetMove(duration));
        }

        public static Tweener TweenScale(this Transform trans, Vector3 value, float duration) {
            var sclOrg = trans.localScale;
            return trans.TweenScale(sclOrg, value, duration);
        }
        
        public static Tweener TweenScale(this Transform trans, Vector3 from, Vector3 to, float duration) { 
            var tweener = new Tweener<Vector3>(
                () => trans.localScale,
                val => trans.localScale = val,
                from,
                to,
                EvaluateUnityVector3.Inst,
                () => trans
            );
            return Tween.LazyInst.Activate(tweener.SetMove(duration));
        }
    }
}