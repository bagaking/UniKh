/** Tween.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 16:38:54
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniKh.core;
using UniKh.core.tween;
using UniKh.extensions;

namespace UniKh.core {
    public class Tween : Singleton<Tween> {
        public float UnScaledTime { get; private set; }
        public float UnScaledStartTime { get; private set; }
        public float UnScaledDeltaTime { get; private set; }

        public List<Tweener> ActiveTweeners { get; private set; } = new List<Tweener>();

        protected override void Awake() {
            base.Awake();
            UnScaledStartTime = Time.realtimeSinceStartup;
        }

        public Tweener Activate(Tweener tweener) {
            tweener.Status = Tweener.State.Active;
            return ActiveTweeners.QueuePush(tweener);
        }

        private void Update() {
            UnScaledTime = Time.realtimeSinceStartup;
            UnScaledDeltaTime = UnScaledTime - UnScaledStartTime;

            ActiveTweeners.ForEach(tweener => {
                if (tweener.Status != Tweener.State.Active) return;

                var targetPos = tweener.TweenPos + Time.deltaTime;
                tweener.MoveTo(targetPos);
            });
        }
    }
}