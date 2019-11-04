/** == AutoFitterLayoutGroup.cs ==
*  Author:         bagaking <kinghand@foxmail.com>
*  CreateTime:     2019/11/04 23:56:31
*  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
*/
 
using UnityEngine; 
using UnityEngine.UI;

namespace UniKh.comp {
    public class AutoFitterLayoutGroup : LayoutGroup, ILayoutSelfController {
        
        public RectTransform[] children;
 
        public float contentWidthLimit = 0;
        public Vector2 innerSizeMin = Vector2.zero;

        private void GetChildSizes(
            RectTransform child, int axis,
            out float min, out float preferred, out float flexible) {
            min = LayoutUtility.GetMinSize(child, axis);
            preferred = LayoutUtility.GetPreferredSize(child, axis);
            flexible = LayoutUtility.GetFlexibleSize(child, axis);
        }

        private void HandleSelfFittingAlongAxis(int axis, float size) {
            m_Tracker.Add(this, rectTransform,
                (axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));

            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta[axis] = size;
            rectTransform.sizeDelta = sizeDelta;
        }

        protected void CalcAlongAxis(int axis, bool isVertical) {
            if(null == children) return;
            
            float combinedPadding = (axis == 0 ? padding.horizontal : padding.vertical);

            float totalMin = combinedPadding;
            float totalPreferred = combinedPadding;
            float totalFlexible = 0;

            bool alongOtherAxis = (isVertical ^ (axis == 1));
            for (int i = 0; i < children.Length; i++) {
                RectTransform child = children[i];
                float min, preferred, flexible;
                GetChildSizes(child, axis, out min, out preferred, out flexible);

                float scaleFactor = child.localScale[axis];
                min *= scaleFactor;
                preferred *= scaleFactor;
                flexible *= scaleFactor;

                if (alongOtherAxis) {
                    totalMin = Mathf.Max(min + combinedPadding, totalMin);
                    totalPreferred = Mathf.Max(preferred + combinedPadding, totalPreferred);
                    totalFlexible = Mathf.Max(flexible, totalFlexible);
                }
                else {
                    totalMin += min;
                    totalPreferred += preferred;

                    // Increment flexible size with element's flexible size.
                    totalFlexible += flexible;
                }
            }

            totalPreferred = Mathf.Max(totalMin, totalPreferred);
            SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, axis);
        }

        protected void SetChildrenAlongAxis(int axis) {
            if(null == children) return;
            if (contentWidthLimit < 0) return;

            float size = rectTransform.rect.size[axis]; 
            if (axis == 0) {
                float innerSize = size - (axis == 0 ? padding.horizontal : padding.vertical);
                float maxChildWidth = 0;
                for (int i = 0; i < children.Length; i++) {
                    RectTransform child = children[i];
                    float min, preferred, flexible;
                    GetChildSizes(child, axis, out min, out preferred, out flexible);
                    float scaleFactor = child.localScale[axis];

//                    float requiredSpace = preferred;//Mathf.Clamp(innerSize, min,  flexible > 0 ? size : preferred);
                    float widthShould = flexible > 0 ? flexible : preferred;
                    float requiredSpace = contentWidthLimit <= 0 ? widthShould : Mathf.Min(widthShould, contentWidthLimit) ;
                     
                    float startOffset = GetStartOffset(axis, requiredSpace * scaleFactor);
                    SetChildAlongAxisWithScale(child, axis, startOffset, requiredSpace, scaleFactor);
                    maxChildWidth = Mathf.Max(requiredSpace, maxChildWidth);
                }
                HandleSelfFittingAlongAxis(axis, Mathf.Max(innerSizeMin.x, padding.horizontal + maxChildWidth));
            }
            else {
                float pos = (axis == 0 ? padding.left : padding.top);
                float itemFlexibleMultiplier = 0;
                float surplusSpace = size - GetTotalPreferredSize(axis);

                if (surplusSpace > 0) {
                    if (GetTotalFlexibleSize(axis) == 0)
                        pos = GetStartOffset(axis,
                            GetTotalPreferredSize(axis) - (axis == 0 ? padding.horizontal : padding.vertical));
                    else if (GetTotalFlexibleSize(axis) > 0)
                        itemFlexibleMultiplier = surplusSpace / GetTotalFlexibleSize(axis);
                }

                float minMaxLerp = 0;
                if (GetTotalMinSize(axis) != GetTotalPreferredSize(axis))
                    minMaxLerp = Mathf.Clamp01((size - GetTotalMinSize(axis)) /
                                               (GetTotalPreferredSize(axis) - GetTotalMinSize(axis)));

                float totalChildHeight = 0;
                for (int i = 0; i < children.Length; i++) {
                    RectTransform child = children[i];
                    float min, preferred, flexible;
                    GetChildSizes(child, axis, out min, out preferred, out flexible);
                    float scaleFactor = child.localScale[axis];

                    float childSize = preferred; //Mathf.Lerp(min, preferred, minMaxLerp);
                    childSize += flexible * itemFlexibleMultiplier;
                    SetChildAlongAxisWithScale(child, axis, pos, childSize, scaleFactor); 
                    pos += childSize * scaleFactor; 
                    totalChildHeight += childSize * scaleFactor; 
                }
                HandleSelfFittingAlongAxis(axis, Mathf.Max(innerSizeMin.y, padding.vertical + totalChildHeight));
            }
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void CalculateLayoutInputHorizontal() {
            CalcAlongAxis(0, true);
            base.CalculateLayoutInputHorizontal();
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void CalculateLayoutInputVertical() {
            CalcAlongAxis(1, true);
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void SetLayoutHorizontal() {
            m_Tracker.Clear();
            SetChildrenAlongAxis(0);
//            HandleSelfFittingAlongAxis(0);
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void SetLayoutVertical() {
            SetChildrenAlongAxis(1);
//            HandleSelfFittingAlongAxis(1);
        }

        protected override void OnEnable() {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnRectTransformDimensionsChange() {
            SetDirty();
        }

#if UNITY_EDITOR
        protected override void OnValidate() {
            SetDirty();
        }
#endif
    }
}