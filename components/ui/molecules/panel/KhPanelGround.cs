using System.Collections;
using UniKh.core.tween;
using UniKh.extensions;
using UnityEngine;

namespace UniKh.comp.ui {
    public class KhPanelGround : KhPanel<MotionTweenAlphaFrom> { 
        
        [ContextMenu("Create Motion")]
        public override MotionTweenAlphaFrom CreateDefaultMotionComponents() {
            motion = GetComponent<MotionTweenAlphaFrom>();
            if (!motion) {
                motion = gameObject.AddComponent<MotionTweenAlphaFrom>();
                motion.startAlphaRate = 0.4f;
                motion.durationShow = 0.3f;
            }
            return motion;
        }
    }
}