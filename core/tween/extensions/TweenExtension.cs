/** TweenExtension.cs
 *  Author:         zhaoyunpeng <zyp94021@gmail.com>
 *  CreateTime:     2019/10/22 15:18:12
 *  Copyright:      (C) 2019 - 2029 zhaoyunpeng, All Rights Reserved
 */
using UnityEngine;

namespace UniKh.core.tween {
    public static class TweenExtension {
        public static Tweener DoFade(this CanvasGroup cg, float value, float duration) {
            var alphaOrg = cg.alpha;
            var tweener = new Tweener<float>(
                () => cg.alpha,
                val => { cg.alpha = val; },
                alphaOrg,
                value,
                EvaluateFloat.Inst,
                () => cg
            );
            return Tween.LazyInst.Activate(tweener.SetMove(duration));
        }
    }
}