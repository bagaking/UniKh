/** NewBehaviourScript.cs
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/08 15:41:38
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniKh.core;
using UniKh.extensions;

public class NewBehaviourScript : BetterBehavior {

#region Events
    /// <summary>
    /// The initial method of NewBehaviourScript.
    /// This method **CAN** be removed
    /// </summary>
    protected override void OnInit() {

    }

    /// <summary>
    /// when the active state changes, the method will be call.
    /// This method **CAN** be removed
    /// </summary>
    protected override void OnSetActive(bool active) {
        if(active) {
            // When this behavior are activated
        } else {
            // When this behavior are disabled
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// This method **CAN** be removed
    /// </summary>
    void Update() {
        
    }
#endregion Events


}
