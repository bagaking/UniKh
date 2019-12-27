/** == KhPanelWindow.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/28 14:19:53
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Collections; 
using UniKh.core.tween;
using UniKh.extensions;
using UnityEngine;  

namespace UniKh.comp.ui {
    public class KhPanelWindow : KhPanel<MotionWindow> {
 
        [ContextMenu("Create Motion")]
        public override MotionWindow CreateDefaultMotionComponents() {
            motion = GetComponent<MotionWindow>();
            if (motion) return motion;
            motion = gameObject.AddComponent<MotionWindow>();
            motion.startOffset = Vector3.up * 100;
            motion.startScaleRate = 0.85f; 
            return motion;
        }
         
        [ContextMenu("Show")]
        public void Show() {
            motion.Show();
        }
        
        [ContextMenu("Hide")]
        public void Hide() {
            motion.Hide();
        }
        
    }
}