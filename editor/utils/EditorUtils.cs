/** EditorUtils.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/22 22:22:22
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UnityEngine;

namespace UniKh.editor {
    
    public static class EditorUtils {

        public static int GetLastControlID() {
            return GUIUtility.GetControlID(FocusType.Passive) - 1;
        }
    }

}
