/** == KhImage.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/23 17:54:52
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using UniKh.core.tween;
using UniKh.extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UniKh.comp.ui {
    public class KhImage : Image {
        public enum MirrorType {
            None = 0,
            Horizontal = 1,
            Vertical = 2,
            Both = 3
        }

        public bool Gray {
            get { return m_gray; }
            set {
                m_gray = value;
                SetImageShader();
            }
        }

        public bool m_gray = false;
        protected Material grayMaterial = null;

        private void SetImageShader() {
            if (m_gray) {
                if (null == grayMaterial)
                    grayMaterial = new Material(Shader.Find("Unlit/TextureGray"));
                material = grayMaterial;
            }
            else
                material = null;
        }

        public MirrorType Mirror {
            get { return m_mirror; }
            set {
                m_mirror = value;
                SetVerticesDirty();
            }
        }

        [SerializeField] private MirrorType m_mirror = MirrorType.None;
        [SerializeField] [Range(0, 3)] private int m_rotate = 0;
        [SerializeField] private Vector2 m_skew = Vector2.zero;

        [SerializeField] [Vector2Plate(-1, -1, 1, 1)]
        public Vector2 gradientDirection = Vector2.zero;

        [SerializeField] public Color gradientColor = Color.white;
        [SerializeField] [EaseDetailAttribute] public StandardEase.Type gradientEase = StandardEase.Type.Linear;

        protected override void OnPopulateMesh(VertexHelper vh) {
            if (color.a * 256f < 5) {
                vh.Clear();
                return; // dont draw anything if transparent
            }


            base.OnPopulateMesh(vh);

            if (Mirror == MirrorType.None
                && m_rotate <= 0
                && m_skew == Vector2.zero
                && gradientDirection == Vector2.zero) { // condition of shape changes 
                return;
            }

//            var vs = new List<UIVertex>();
//            var vsNew = new List<UIVertex>(vs.Count);
//            vh.GetUIVertexStream(vs);
//            vh.Clear();
            var rect = GetPixelAdjustedRect();

            Easing gradientEaseCurve = null;
            var gradientDir = Vector2.zero;
            var gradientStart = 0f;
            var gradientDistance = 1f;
            if (gradientDirection != Vector2.zero) {
                gradientEaseCurve = StandardEase.Get(gradientEase);
                gradientDir = gradientDirection.normalized;
                var gradientSegment = rect.ProjectionTo(gradientDir);
                gradientStart = gradientSegment.x;
                gradientDistance = gradientSegment.y - gradientSegment.x;
            }

            var vCur = new UIVertex();
            for (var i = 0; i < vh.currentVertCount; i++) {
                vh.PopulateUIVertex(ref vCur, i);
                var retX = vCur.position.x;
                var retY = vCur.position.y;
                var center = rect.center;
                if (Mirror != MirrorType.None) {
                    retX = ((int) Mirror % 2 == 1) ? center.x * 2 - vCur.position.x : vCur.position.x;
                    retY = ((int) Mirror > 1) ? center.y * 2 - vCur.position.y : vCur.position.y;
                }

                if (m_rotate > 0) {
                    var offX = retX - center.x;
                    var offY = retY - center.y;
                    switch (m_rotate) {
                        case 1:
                            retX = center.x + offY * rect.width / rect.height;
                            retY = center.y - offX * rect.height / rect.width;
                            break;
                        case 2:
                            retX = center.x - offX;
                            retY = center.y - offY;
                            break;
                        case 3:
                            retX = center.x - offY * rect.width / rect.height;
                            retY = center.y + offX * rect.height / rect.width;
                            break;
                    }
                }

                if (m_skew != Vector2.zero) {
                    var offX = retX - rect.xMin;
                    var offY = retY - rect.yMin;
                    retX += Mathf.Lerp(-m_skew.x, m_skew.x, offY / rect.height);
                    retY += Mathf.Lerp(-m_skew.y, m_skew.y, offX / rect.width);
                }

                if (gradientDir != Vector2.zero && gradientEaseCurve != null) {
                    var projectionPos = Vector2.Dot(vCur.position, gradientDir) - gradientStart;
                    var scaledPos = projectionPos / gradientDistance;
                    var ratio = gradientEaseCurve.Convert(scaledPos);
                    vCur.color = Color.Lerp(color, gradientColor, ratio);
                }

                vCur.position.x = retX;
                vCur.position.y = retY;


                vh.SetUIVertex(vCur, i);
            }


//            vh.AddUIVertexTriangleStream(vsNew);
        }
    }
}