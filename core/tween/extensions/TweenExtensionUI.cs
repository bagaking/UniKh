/** TweenExtensionTransform.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 21:18:12
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;
using UnityEngine.UI;

namespace UniKh.core.tween {
    
    public static class TweenExtensionUI {

        public static Tweener TweenAlpha(this MaskableGraphic img, float alpha, float duration) {
            var alphaOrg = img.color.a;
            var tweener = new Tweener<float>(
                () => img.color.a,
                val => {
                    Color color;
                    img.color = new Color((color = img.color).r, color.g, color.b, val);
                },
                alphaOrg,
                alpha,
                EvaluateFloat.Inst,
                () => img
            );
            return Tween.LazyInst.Activate(tweener.SetMove(duration));
        }

        public static Tweener TweenAlpha(this CanvasGroup cg, float alpha, float duration)
        {
            var alphaOrg = cg.alpha;
            var tweener = new Tweener<float>(
                () => cg.alpha,
                val => cg.alpha = val,
                alphaOrg,
                alpha,
                EvaluateFloat.Inst,
                () => cg
            );
            return Tween.LazyInst.Activate(tweener.SetMove(duration));
        }

    }
}