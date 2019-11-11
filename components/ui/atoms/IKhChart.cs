/** == IKhChart.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/11 14:19:16
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

 
using UnityEngine;
using UnityEngine.UI;

namespace UniKh.comp.ui {
    public interface IKhChart: ILayoutElement {
        Vector2[] GetVerticalPoses(int count);
        Vector2[] GetHorizontalPoses(int count);
        
        RectTransform[] GetVerticalIndicators();
        
        RectTransform[] GetHorizontalIndicators();
        
    }
}