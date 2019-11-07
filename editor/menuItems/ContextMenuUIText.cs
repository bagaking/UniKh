/** == ContextMenuUIText.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/01 16:34:29
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
    
    public class ContextMenuUIText : ContextMenuUI {

        static KhText CreateUINodeText(GameObject goParent) {
            var go = CreateNewGameObject(goParent, "a");
            var txt = go.AddComponent<KhText>();
            
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            txt.verticalOverflow = VerticalWrapMode.Overflow;
//            txt.rectTransform.SetAnchorStretchAll();
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = new Color(0.3f, 0.3f, 0.3f);
            
            if (KhPreferenceStatic.Inst) {
                txt.fontSize = KhPreferenceStatic.Inst.defaultFontSize;
            }

            return txt;
        }
        
        [MenuItem("GameObject/Kh UI (Atom)/a <Text>/Ch", false, 0)]
        internal static KhText CreateUINodeTextCh(MenuCommand mc) {
            var txt = CreateUINodeText(mc.context as GameObject); 
            if (KhPreferenceStatic.Inst && KhPreferenceStatic.Inst.fontCH) {
                txt.font = KhPreferenceStatic.Inst.fontCH;
            }
            txt.text = "中文内容";
            return txt;
        }

        
        [MenuItem("GameObject/Kh UI (Atom)/a <Text>/En", false, 0)]
        internal static KhText CreateUINodeTextEn(MenuCommand mc) {
            var txt = CreateUINodeText(mc.context as GameObject); 
            if (KhPreferenceStatic.Inst && KhPreferenceStatic.Inst.fontEN) {
                txt.font = KhPreferenceStatic.Inst.fontEN;
            }

            txt.text = "English Contents";
            return txt;
        }
        
        [MenuItem("GameObject/Kh UI (Atom)/a <Text>/Number", false, 0)]
        internal static KhText CreateUINodeTextNumber(MenuCommand mc) {
            var txt = CreateUINodeText(mc.context as GameObject);
            txt.m_type = KhText.Type.NumberText;
            txt.NumberValue = 0;
            
            if (KhPreferenceStatic.Inst && KhPreferenceStatic.Inst.fontNUM) {
                txt.font = KhPreferenceStatic.Inst.fontNUM;
            } 
            return txt;
        }

         
    }
}