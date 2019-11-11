/** == KhCandlestickChart.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/08 16:03:08
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UniKh.comp.ui {
    public class KhCandlestickChart : Image {

        public enum GraphType {
            CandlestickChart,
            LineChart
        }
        
        [Serializable]
        public class Node {
            public float first;
            public float last;
            public float max;
            public float min;
        }

//        public List<Node> nodes;
        public List<Node> Nodes {
            get { return m_nodes; }
            set {
                SetVerticesDirty();
                m_nodes = value;
            }
        }
        public List<Node> m_nodes = new List<Node>();

        public GraphType type = GraphType.CandlestickChart;

        [ContextMenu("CreateRandomData")]
        public void CreateRandomData() {
            if(Nodes.Count > 0) return;
            float firstValue = Random.Range(4000, 6000);
            var newNodes = new List<Node>();
            for (var i = 0; i < 30; i++) {
                var node = new Node {
                    first = firstValue,
                    last = firstValue + Random.Range(-100, 100),
                    min = firstValue + Random.Range(-100, -200),
                    max = firstValue + Random.Range(100, 200)
                };
                firstValue = node.last;
                newNodes.Add(node);
            } 
            Nodes = newNodes;
        }


        public Color colorUp = Color.red;
        public Color colorDown = Color.green;

        public bool showBorder = true;
        public float innerLineSize = 4;

        public bool showPings = false;

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

        private Vector2[] standardUVs = new[] {Vector2.zero, Vector2.up, Vector2.one, Vector2.left};

        private void AddUIVertexDefault(VertexHelper vh, Color c, Vector2[] verts) {
            vh.AddUIVertexQuad(SetVbo(verts, standardUVs, c));
        }

        private void DrawCandlestickChart(VertexHelper vh, Vector2 offset) {
            Vector2 pos0 = Vector2.zero;
            Vector2 pos1 = Vector2.zero;
            Vector2 pos2;
            Vector2 pos3;

            var scaleSpanX = rectTransform.rect.width / Nodes.Count;
            var min = float.MaxValue;
            var max = float.MinValue;
            foreach (var t in Nodes) {
                var v = t.last;
                if (showPings) {
                    if (min > t.min) min = t.min;
                    if (max < t.max) max = t.max;
                }
                else {
                    if (min > t.first) min = t.first;
                    if (max < t.last) max = t.last;
                }
            } 
            
            var scaleY = max == min ? 0 : (rectTransform.rect.height - innerLineSize) / (max - min);

            float Projection(float y) {
                return (y - min) * scaleY;
            }

            var c = colorUp;
            for (var i = 0; i < Nodes.Count; i++) {
                var height = Projection(Nodes[i].last) + innerLineSize;
                c = height >= pos1.y ? colorUp : colorDown;
                pos0 = new Vector2(scaleSpanX * i, Projection(Nodes[i].first));
                pos1 = new Vector2(pos0.x, height);
                pos2 = new Vector2(pos1.x + scaleSpanX, pos1.y);
                pos3 = new Vector2(pos2.x, pos0.y);

                if (showPings) {
                    var middlePosX = (pos0.x + pos2.x - innerLineSize) / 2;
                    AddUIVertexDefault(vh, c, new[] {
                        new Vector2(middlePosX, Projection(Nodes[i].min)) + offset,
                        new Vector2(middlePosX, Projection(Nodes[i].max)) + offset,
                        new Vector2(middlePosX + innerLineSize, Projection(Nodes[i].max)) + offset,
                        new Vector2(middlePosX + innerLineSize, Projection(Nodes[i].min)) + offset
                    });
                }

                AddUIVertexDefault(vh, c, new[] {pos0 + offset, pos1 + offset, pos2 + offset, pos3 + offset});
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear(); 

            var rcTrans = rectTransform;
            Rect rc;
            var offset = -rcTrans.pivot * (rc = rcTrans.rect).size;
            
            if (showBorder) {
                AddUIVertexDefault(vh, color,new[] {
                    new Vector2(-innerLineSize, rc.height) + offset,
                    new Vector2(0, rc.height) + offset,
                    new Vector2(0, 0) + offset,
                    new Vector2(-innerLineSize, 0) + offset
                });
                AddUIVertexDefault(vh, color,new[] {
                    new Vector2(-innerLineSize, -innerLineSize) + offset,
                    new Vector2(rc.width, -innerLineSize) + offset,
                    new Vector2(rc.width, 0) + offset,
                    new Vector2(-innerLineSize, 0) + offset
                });
            }


            switch (type) {
                case GraphType.CandlestickChart:
                    DrawCandlestickChart(vh, offset);
                    break;
                case GraphType.LineChart:
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}