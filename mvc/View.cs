using System.Collections;
using UniKh.core;
using UniKh.mgr;
using UnityEngine;

namespace UniKh.mvc {
    public abstract class View<TModel> : MonoBehaviour where TModel : ViewModel {
        public TModel Model { get; private set; }

        public void Bind(TModel model_) {
            Model = model_;
            Model.SetDirty();
            // OnBind(model_);
        }

        public void Debind() {
            var oldModel = Model;
            Model = null;
            StartCoroutine(OnDebind(oldModel));
        }

        public void UpdateView() {
            if (!Model.dirty) return;
            CorouMgr.Run(OnUpdateView());
            Model.MarkRendered();
        }

        protected abstract IEnumerator OnUpdateView();

        // protected abstract IEnumerator OnBind(TModel oldModel);

        protected abstract IEnumerator OnDebind(TModel oldModel);
    }
}