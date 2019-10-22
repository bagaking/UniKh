/** Tween.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 16:38:54
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
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
            tweener.StateTransition(Tweener.State.Active);
            ActiveTweeners.PushLast(tweener);
            return tweener;
        }

        private int _absentTweenerCounts = 0;
        private void Update() {
            UnScaledTime = Time.realtimeSinceStartup;
            UnScaledDeltaTime = UnScaledTime - UnScaledStartTime;

            if(null == ActiveTweeners) return;
            ActiveTweeners.ForEach((tweener, ind) => {
                if (tweener == null) return;
                if (tweener.Status != Tweener.State.Active) {
                    if (tweener.Status > Tweener.State.Active) {
                        _absentTweenerCounts++;
                        ActiveTweeners[ind] = null;
                    }
                    return;
                }

                var targetPos = tweener.TweenPos + Time.smoothDeltaTime;// Time.deltaTime;
                tweener.MoveTo(targetPos);
            });

            if (_absentTweenerCounts > 16 && _absentTweenerCounts >= ActiveTweeners.Count / 2) {
                ActiveTweeners.InPlaceFilter(x => x != null);
                _absentTweenerCounts = 0;
            }
             
        }

    }
}