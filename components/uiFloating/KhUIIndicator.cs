using UniKh.core;
using UniKh.extensions;
using UniKh.utils;
using UnityEngine;

namespace UniKh.comp.ui {
    public class KhUiIndicator<T> : BetterBehavior where T : MonoBehaviour {
        [Header("Setting")] 
        public bool lockInScene = false;
        public Vector2 lockWindowShrink = default;


        [Header("Runtime: Following")] [SerializeField]
        public Camera mSceneCamera;

        public GameObject followingTarget3D;
        public Vector2 followingOffset;
        [SerializeField] private Canvas mCanvas;


        public Camera SceneCam {
            get {
                if (mSceneCamera == null) {
                    mSceneCamera = Camera.main; // todo: think about this
                }

                return mSceneCamera;
            }
        }

        public Canvas Canvas {
            get {
                if (mCanvas == null) {
                    mCanvas = GetComponentInParent<Canvas>();
                }

                return mCanvas;
            }
        }

        private void OnCanvasHierarchyChanged() {
            mCanvas = null;
        }

        public KhUiIndicator<T> BindTo(Transform indicatorRoot, T target) {
            Transform trans;
            (trans = transform).SetParent(indicatorRoot);
            trans.localScale = Vector3.one;
            followingTarget3D = target.gameObject;
            SetInfo(target);
            return this;
        }

        public virtual void SetInfo(T warCraft) { }

        public void LateUpdate() {
            if (null == followingTarget3D || null == Canvas || null == SceneCam) return;
            var rectParent = transform.parent as RectTransform;
            if (rectParent == null) return;
            var trans = transform;
            var pos = followingOffset + 
                PositionProjector.ScenePositionToUILocalPosition(
                    followingTarget3D.transform,
                    rectParent,
                    SceneCam,
                    Canvas
                );
            
            trans.localPosition = lockInScene ?  rectParent.rect.GetProjectionOf(pos, lockWindowShrink) : pos; // pos
        }
    }
} 