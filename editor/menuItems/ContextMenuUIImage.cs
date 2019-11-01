/** == ContextMenuUIImage.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/01 23:17:36
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
    public class ContextMenuUIImage : ContextMenuUI {
 
        [MenuItem("GameObject/Kh UI (Atom)/i <Image>", false, 0)]
        internal static void CreateUINodeImage(MenuCommand mc) {
            var go = CreateNewGameObject(mc.context as GameObject, "i");
            go.AddComponent<KhImage>();
        }
    }
} 