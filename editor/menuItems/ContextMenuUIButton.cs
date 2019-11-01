/** == ContextMenuUIButton.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/01 23:18:16
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.core;
using UniKh.extensions;
using UniKh.utils;

using System.Collections;
using System.Collections.Generic;
using UniKh.comp.ui;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    public class ContextMenuUIButton : ContextMenuUI {

       
        
        [MenuItem("GameObject/Kh UI (Molecules)/btn <Button>", false, 0)]
        internal static void CreateUINodeButton(MenuCommand mc) { 
            var go = CreateNewGameObject(mc.context as GameObject, "btn");

            var btn = go.AddComponent<KhBtn>();
            btn.image = go.AddComponent<KhImage>();
        }
        
//        [MenuItem("GameObject/Kh UI (Molecules)/btn <Button>", true)]
//        internal static bool ValidateCreateUINodeButton(MenuCommand mc) {
//            var goParent = (mc.context as GameObject);
//            return goParent != null && goParent.GetComponent<RectTransform>() != null;
//
//        }
    }
}