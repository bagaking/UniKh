/** == KhLineChart.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/11 12:22:35
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using UniKh.extensions;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UniKh.comp.ui {
    public class KhLineChart : Image, IKhChart {
        public List<float> Nodes {
            get { return m_nodes; }
            set {
                m_nodes = value;
                SetVerticesDirty();
                CalculateIndicators();
            }
        }

        public List<float> m_nodes = new List<float>();

        public Func<float, string> OnCalculateVIndicator;
        public Func<int, int, string> OnCalculateHIndicator;
         
        [ContextMenu("Calculate Indicators")]
        public void CalculateIndicators() {
            if (vIndicators.Length > 0) {
                getMinMax(out var min, out var max);
                var split = vIndicators.Length == 1 ? 0 : (max - min) / (vIndicators.Length - 1);
                vIndicators.ForEach((indicator, index) => {
                    if (!indicator) return;
                    var val = min + index * split;
                    indicator.text = OnCalculateVIndicator != null ? OnCalculateVIndicator(val) : val.ToString(CultureInfo.InvariantCulture);
                });
            }
            
            if (hIndicators.Length > 0) {
                hIndicators.ForEach((indicator, index) => {
                    if (!indicator) return; 
                    indicator.text = OnCalculateHIndicator != null ? OnCalculateHIndicator(index, hIndicators.Length) : index.ToString(CultureInfo.InvariantCulture);
                });
            }
        }

        [ContextMenu("CreateRandomData")]
        public void CreateRandomData() {
            if (Nodes.Count > 0) return;
            float firstValue = Random.Range(4000, 6000);
            var newNodes = new List<float>();
            for (var i = 0; i < 30; i++) {
                firstValue = Random.Range(-100, 100);
                newNodes.Add(firstValue);
            }

            Nodes = newNodes;
        }

        public RectOffset padding;
        
        public Color colorBorder = Color.black;

        public float borderWidth = 0;
        public float thickness = 6;

        public Vector2 jointSize = Vector2.zero;
        public Color colorJoint = Color.cyan;

        public bool reverseEffect = false;

        public Text[] vIndicators;
        public Text[] hIndicators;

        public RectTransform[] GetVerticalIndicators() {
            return vIndicators.Map(a => a.rectTransform);}

        public RectTransform[] GetHorizontalIndicators() {
            return hIndicators.Map(a => a.rectTransform);
        }

        protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs, Color c) {
            UIVertex[] vbo = new UIVertex[4];
            for (int i = 0; i < vertices.Length; i++) {
                var vert = UIVertex.simpleVert;
                vert.color = c;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }

            return vbo;
        }

        private static Vector2[][] standardUVs = new[] {
            new[] {new Vector2(0, 0), new Vector2(0, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0)},
            new[] {new Vector2(0.5f, 0), new Vector2(0.5f, 0.5f), new Vector2(1, 0.5f), new Vector2(1, 0)},
            new[] {new Vector2(0, 0.5f), new Vector2(0, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 0.5f)},
            new[] {new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1), new Vector2(1, 1), new Vector2(1, 0.5f)}
        };

        private void AddUIVertexDefault(VertexHelper vh, Color c, Vector2[] vertOfQuad, int position = 0) {
            // todo: refine this with vh.AddUIVertexStream;
            vh.AddUIVertexQuad(SetVbo(vertOfQuad, standardUVs[position], c));
        }

        private Rect GetViewport() {
            var rcTrans = rectTransform;
            var rc = rcTrans.rect;
            var marginLB = new Vector2(
                Mathf.Max(jointSize.x, thickness / 2) + padding.left,
                Mathf.Max(jointSize.y, thickness / 2) + padding.bottom
            );
            var marginRT = new Vector2(
                Mathf.Max(jointSize.x, thickness / 2) + padding.right,
                Mathf.Max(jointSize.y, thickness * 0.71f) + padding.top
            );
            return new Rect(
                marginLB,
                rc.size - marginRT - marginLB
            );
        }

        private void getMinMax(out float min,  out float max) {
            min = float.MaxValue;
            max = float.MinValue;
            foreach (var t in Nodes) {
                if (min > t) min = t;
                if (max < t) max = t;
            }
        }
        
        private void DrawLineChart(VertexHelper vh) {
            getMinMax(out var min, out var max);

            var rcViewport = GetViewport();
            var startOffset = rcViewport.position - rectTransform.pivot * rectTransform.rect.size;

            var scaleSpanX = rcViewport.width / Nodes.Count;
            var scaleY = max <= min ? 0 : rcViewport.height / (max - min);

            float Projection(float y) {
                return (y - min) * scaleY;
            }

            var posPrev = Vector2.zero;
            var posCurrent = Vector2.zero;

            Vector2[] lineSegment = null;


            for (var i = 0; i < Nodes.Count; i++) {
                var x = scaleSpanX * i;
                var y = Projection(Nodes[i]);
                posPrev = posCurrent;
                posCurrent = new Vector2(x, y) + startOffset;

                if (i <= 0) continue;
                var prevSegment = lineSegment;
                lineSegment = CalculateLineRect(posPrev, posCurrent);
                if (prevSegment != null) {
                    AddUIVertexDefault(vh, color,
                        new[] {prevSegment[2], prevSegment[3], lineSegment[0], lineSegment[1]});
                }

                AddUIVertexDefault(vh, color, lineSegment);
            }

            if (jointSize != Vector2.zero) {
                for (var i = 0; i < Nodes.Count; i++) {
                    var x = scaleSpanX * i;
                    var y = Projection(Nodes[i]);
                    posCurrent = new Vector2(x, y) + startOffset;


                    AddUIVertexDefault(vh, colorJoint, new[] {
                        posCurrent + new Vector2(-jointSize.x, -jointSize.y),
                        posCurrent + new Vector2(-jointSize.x, jointSize.y),
                        posCurrent + new Vector2(jointSize.x, jointSize.y),
                        posCurrent + new Vector2(jointSize.x, -jointSize.y)
                    }, 1);
                }
            }
        }

        private Vector2[] CalculateLineRect(Vector2 posFrom, Vector2 posTo) {
            var legY = posTo.y - posFrom.y;
            var legX = posTo.x - posFrom.x;
            var opposite = Mathf.Sqrt(legX * legX + legY * legY);

            var cos = legX / opposite;
            var sin = legY / opposite;
            var t = thickness / 2;
            var offset1 = new Vector2(t * sin, -t * cos);
            var offset2 = -offset1;

            return new[] {
                posFrom + offset1,
                posFrom + offset2,
                (reverseEffect ? offset1 : offset2) + posTo,
                (reverseEffect ? offset2 : offset1) + posTo
            };
        }

        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();

            var rcTrans = rectTransform;
            Rect rc;
            var offset = - rcTrans.pivot * (rc = rcTrans.rect).size;

            if (borderWidth > 0) {
                AddUIVertexDefault(vh, colorBorder, new[] {
                    new Vector2(-borderWidth, rc.height) + offset,
                    new Vector2(0, rc.height) + offset,
                    new Vector2(0, 0) + offset,
                    new Vector2(-borderWidth, 0) + offset
                });
                AddUIVertexDefault(vh, colorBorder, new[] {
                    new Vector2(-borderWidth, -borderWidth) + offset,
                    new Vector2(rc.width, -borderWidth) + offset,
                    new Vector2(rc.width, 0) + offset,
                    new Vector2(-borderWidth, 0) + offset
                });
            }

            DrawLineChart(vh);
        }

        public Vector2[] GetVerticalPoses(int count) {
            if (count <= 1) return null;
            var ret = new Vector2[count];
            var rc = GetViewport();
            var scale = rc.height / (count - 1);
            for (var i = 0; i < count; i++) {
                ret[i] = new Vector2(- borderWidth * 2, scale * i + rc.position.y);
            }
            return ret;
        }

        public Vector2[] GetHorizontalPoses(int count) {
            if (count <= 1) return null;
            var ret = new Vector2[count];
            var rc = GetViewport();
            var scale = rc.width / (count - 1);
            for (var i = 0; i < count; i++) {
                ret[i] = new Vector2(scale * i + rc.position.x, - borderWidth * 2 );
            } 
            return ret;
        }
    }
}