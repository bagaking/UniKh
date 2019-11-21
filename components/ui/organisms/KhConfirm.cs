using System;
using UniKh.core.tween;
using UniKh.extensions;
using UnityEngine;
using UnityEngine.Events;

namespace UniKh.comp.ui {
    public class KhConfirm : MotionTweenMoveOffsetFrom {
        public KhText content;
        public KhBtn btnOk;
        public KhBtn btnCancel;
        public KhBtn btnAbort;

        public bool IsWorking => gameObject.activeInHierarchy;

        public void Show(
            string text
            , UnityAction cbOk = null
            , UnityAction cbCancel = null
            , UnityAction cbAbort = null) {
            if (content) content.text = text;
            ResetListeners();

            if (cbOk != null) {
                btnOk.SetObjectActive();
                btnOk.onClick.AddListener(cbOk);
            }
            else {
                btnOk.SetObjectActive(false); 
            }

            if (cbCancel != null) {
                btnCancel.SetObjectActive();
                btnCancel.onClick.AddListener(cbCancel);
            }
            else {
                btnCancel.SetObjectActive(false); 
            }

            if (cbAbort != null) {
                btnAbort.SetObjectActive();
                btnAbort.onClick.AddListener(cbAbort);
            }
            else {
                btnAbort.SetObjectActive(false); 
            }

            this.SetObjectActive();
        }

        public void ResetListeners() {
            btnOk.onClick.RemoveAllListeners();
            btnOk.onClick.AddListener(() => this.SetObjectActive(false));
            btnAbort.onClick.RemoveAllListeners();
            btnAbort.onClick.AddListener(() => this.SetObjectActive(false));
            btnCancel.onClick.RemoveAllListeners();
            btnCancel.onClick.AddListener(() => this.SetObjectActive(false));
        }
    }
}