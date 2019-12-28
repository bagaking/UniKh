/** == ContextMenuUI.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/12/29 04:08:57
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */
 
using UniKh.core;
using UnityEngine;
using UnityEngine.UI;
using UniKh.extensions;

namespace UniKh.comp.ui {
    public class KhQuantityBar : BetterBehavior {

        [Header("Binding:Show")] 
        public KhText aAmount;
        public KhImage iSym;
        
        [Header("Binding:Event")]
        public KhBtn btnBar;
        public KhBtn btnCharge;
 

        [Header("Runtime")] public float amount = 0;
 
        public void TryRepaintAmount() {
            if (aAmount == null || amount.EqualsTo(aAmount.NumberValue)) return;
            aAmount.NumberValue = amount;
        }
      
        public void Update() {
            TryRepaintAmount();
        }

        public void SetSymIcon(Sprite sprite) { 
            if (iSym == null || iSym.sprite == sprite || sprite == null) return;
            iSym.sprite = sprite;
        }
    }
}