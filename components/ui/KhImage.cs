/** == KhImage.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/23 17:54:52
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

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

//        public new Rect GetPixelAdjustedRect() {
//            var rect = base.GetPixelAdjustedRect();
//            return m_rotate % 2 == 0 ? rect : new Rect(rect.center.x - rect.height / 2, rect.center.y - rect.width / 2, rect.height, rect.width);
//        }

        protected override void OnPopulateMesh(VertexHelper vh) {
            if (color.a * 256f < 5) {
                vh.Clear();
                return; // dont draw anything if transparent
            }


            base.OnPopulateMesh(vh);

            if (Mirror == MirrorType.None && m_rotate <= 0 && m_skew == Vector2.zero)
                return; // condition of shape changes 

//            var vs = new List<UIVertex>();
//            var vsNew = new List<UIVertex>(vs.Count);
//            vh.GetUIVertexStream(vs);
//            vh.Clear();
            var r = GetPixelAdjustedRect();

            var vCur = new UIVertex();
            for (var i = 0; i < vh.currentVertCount; i++) {
                vh.PopulateUIVertex(ref vCur, i);
                var retX = vCur.position.x;
                var retY = vCur.position.y;
                var center = r.center;
                if (Mirror != MirrorType.None) {
                    retX = ((int) Mirror % 2 == 1) ? center.x * 2 - vCur.position.x : vCur.position.x;
                    retY = ((int) Mirror > 1) ? center.y * 2 - vCur.position.y : vCur.position.y;
                }

                if (m_rotate > 0) {
                    var offX = retX - center.x;
                    var offY = retY - center.y;
                    switch (m_rotate) {
                        case 1:
                            retX = center.x + offY * r.width / r.height;
                            retY = center.y - offX * r.height / r.width;
                            break;
                        case 2:

                            retX = center.x - offX;
                            retY = center.y - offY;
                            break;
                        case 3:
                            retX = center.x - offY * r.width / r.height;
                            retY = center.y + offX * r.height / r.width;
                            break;
                    }
                }

                if (m_skew != Vector2.zero) {
                    var offX = retX - r.xMin;
                    var offY = retY - r.yMin;
                    retX += Mathf.Lerp(- m_skew.x, m_skew.x, offY / r.height) ;
                    retY += Mathf.Lerp(- m_skew.y, m_skew.y, offX / r.width);
                }

                vCur.position.x = retX;
                vCur.position.y = retY;
                vh.SetUIVertex(vCur, i);
            }

//            vh.AddUIVertexTriangleStream(vsNew);
        }
    }
}