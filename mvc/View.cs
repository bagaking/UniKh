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

        public void UnBind() {
            var oldModel = Model;
            Model = null;
            OnUnbind(oldModel);
        }

        public void UpdateView() {
            if (!Model.dirty) return;
            OnUpdateView();
            Model.MarkRendered();
        }

        protected abstract void OnUpdateView();

        // protected abstract IEnumerator OnBind(TModel oldModel);

        protected abstract void OnUnbind(TModel oldModel);
    }
}