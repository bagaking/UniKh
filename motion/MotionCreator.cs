using UnityEngine;
using UniKh.core;
using UnityEngine.UI;

namespace UniKh.motion {
    public static class MotionCreator {
        public static Motion<Vector3>
            CMLocalPosition(this Transform target, Vector3 startOffset, float duration) =>
            new Motion<Vector3>(
                target.localPosition + startOffset,
                target.localPosition,
                duration,
                Vector3.Lerp,
                v3 => target.localPosition = v3);
        
        public static Motion<float>
            CMImageAlpha(this Image target, float startOffset, float duration) {
            Color color;
            return new Motion<float>(
                (color = target.color).a + startOffset,
                color.a,
                duration,
                Mathf.Lerp,
                v => target.color = new Color(color.r, color.g, color.b, v)
            );
        }
    }
}