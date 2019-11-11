/** == ChartLayoutGroup.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/11 14:19:02
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.core;
using UniKh.extensions;
using UniKh.utils;
using System.Collections;
using System.Collections.Generic;
using UniKh.comp.ui;
using UnityEngine;
using UnityEngine.UI;

namespace UniKh.comp {
    public class ChartLayoutGroup : LayoutGroup {
        public KhLineChart chart;

        protected void SetChildPos(RectTransform rect, int axis, Vector2 pos) {
            if (rect == null)
                return;

            m_Tracker.Add(this, rect,
                DrivenTransformProperties.Anchors
                | DrivenTransformProperties.AnchoredPositionX
                | DrivenTransformProperties.AnchoredPositionY
            );


            // Inlined rect.SetInsetAndSizeFromParentEdge(...) and refactored code in order to multiply desired size by scaleFactor.
            // sizeDelta must stay the same but the size used in the calculation of the position must be scaled by the scaleFactor.

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;

            var anchoredPosition = rect.anchoredPosition;
            var offset = padding;
            anchoredPosition[axis] = pos[axis];
            var axisNormal = 1 ^ axis;
            anchoredPosition[axisNormal] = pos[axisNormal] - (axisNormal == 0 ? offset.left : offset.top);
            rect.anchoredPosition = anchoredPosition;
        }

        public override void CalculateLayoutInputVertical() {
            SetLayoutInputForAxis(minWidth, preferredWidth, flexibleWidth, 0);
//            SetLayoutInputForAxis(minHeight, preferredHeight, flexibleHeight, 1);
        }

        public override void SetLayoutHorizontal() {
            m_Tracker.Clear();
            SetChildrenAlongAxis(0);
        }

        public override void SetLayoutVertical() {
            SetChildrenAlongAxis(1);
        }

        private void SetChildrenAlongAxis(int axis) {
            if (axis == 0) {
                var horizontalTextGroup = chart.GetHorizontalIndicators();
                var hChildPoses = chart.GetHorizontalPoses(horizontalTextGroup.Length);
                for (var i = 0; i < horizontalTextGroup.Length; i++) {
                    var child = horizontalTextGroup[i];
                    SetChildPos(child, axis, hChildPoses[i]);
                }
            }
            else {
                var verticalTextGroup = chart.GetVerticalIndicators();
                var vChildPoses = chart.GetVerticalPoses(verticalTextGroup.Length);
                for (var i = 0; i < verticalTextGroup.Length; i++) {
                    var child = verticalTextGroup[i];
                    SetChildPos(child, axis, vChildPoses[i]);
                }
            }
        }
    }
}