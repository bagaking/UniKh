/** == InspectorKhLineChart.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/11 14:09:20
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.comp.ui;
using UnityEditor; 

namespace UniKh.editor {
    
    [CustomEditor(typeof(KhLineChart), true)]
    [CanEditMultipleObjects]
    public class InspectorKhLineChart : Editor { 

        public override void OnInspectorGUI() {
            base.OnInspectorGUI(); 
        }
    }
}