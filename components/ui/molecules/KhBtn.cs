/** KhBtn.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/14 15:08:33
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Collections;
using UnityEngine;
using UniKh.extensions;
using UniKh.core.tween;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UniKh.comp.ui {
    public class KhBtn : Button {
        
        public const float ClickMinIntervalS = 0.2f;
        
        public Text text;

        public string audioName = "khBtn";


        public enum ClickAnimation {
            None,
            Shrink,
            ShrinkAlpha,
        }

        public ClickAnimation clickAnimation = ClickAnimation.Shrink;
        protected Tweener tweener = null;
        public float tweenScale = 0.97f;


        public override void OnPointerDown(PointerEventData eventData) { 
            
            if (tweener != null) {
                tweener.Terminate();
                tweener = null;
            }

            switch (clickAnimation) {
                case ClickAnimation.Shrink:
                    //tweener = transform.DOScale(Vector3.one * tweenScale, 0.1f);
                    transform.localScale = Vector3.one * tweenScale;
                    break;
                case ClickAnimation.ShrinkAlpha:

                    tweener = transform.TweenScale(Vector3.one * tweenScale, 0.1f);
                    var canvasGroup = transform.GetOrAdd<CanvasRenderer>();
                    canvasGroup.SetAlpha(0.5f);
                    break;
                default:
                    break;
            }

            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData) { 
            
            if (tweener != null) {
                tweener.Terminate();
                tweener = null;
            }

            tweener = transform.TweenScale(Vector3.one, 0.1f);
            if (clickAnimation == ClickAnimation.ShrinkAlpha) {
                var canvasRenderer = transform.GetComponent<CanvasRenderer>();

                if (canvasRenderer != null) {
                    canvasRenderer.SetAlpha(1f);
                }
            }

            base.OnPointerUp(eventData);
        }
 
        public void PlayClickAudio() {
            if (!audioName.Exists()) return;
            AudioMgr.LazyInst.Play(audioName);
        }

        private static float _lastClick = 0; 
        public override void OnPointerClick(PointerEventData eventData) {
            if ((Time.realtimeSinceStartup - _lastClick) < ClickMinIntervalS) {
                return;
            }
            _lastClick = Time.realtimeSinceStartup;
            base.OnPointerClick(eventData);
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            PlayClickAudio();
        }
    }
}