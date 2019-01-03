using System;
using System.Collections.Generic;
using System.Numerics;
using UniKh.dataStructure;
using UniKh.extensions;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace UniKh.utils {
#if UNITY_EDITOR

    public static class Gizmo2D {
        public static Gizmo2DDrawer drawer => _drawer ?? (_drawer = new Gizmo2DDrawer());

        private static Gizmo2DDrawer _drawer;

        public static Gizmo2DDrawer C(Color c) => new WithColorDrawer(c);

        private static List<Color> colorQueue = new List<Color>();
        public static Gizmo2DDrawer StartColor(Color c) {
            colorQueue.Push(UnityEditor.Handles.color);
            UnityEditor.Handles.color = c;
            return _drawer;
        }
    
    
        public static void EndColor() {
            UnityEditor.Handles.color = colorQueue.Pop();
        }
    }

    public class Drawer<T> where T : Drawer<T> {
        private bool entered;

        protected virtual void BeforeDraw() { entered = true; }

        protected virtual T AfterDraw() {
            if (entered == false) throw new Exception("If the drawer method has AfterDraw procedure, BeforeDraw must be called.");
            entered = true;
            return this as T;
        }
    }

    public class WithColorDrawer : Gizmo2DDrawer {
        private Color color { get; }

        public WithColorDrawer(Color c) {
            color = c;
        }


        private Color cOrigin;
        protected override void BeforeDraw() {
            base.BeforeDraw();
            cOrigin = UnityEditor.Handles.color;
            UnityEditor.Handles.color = color;
        }

        protected override Gizmo2DDrawer AfterDraw() {
            UnityEditor.Handles.color = cOrigin;
            return base.AfterDraw();
        }
    }

    public class Gizmo2DDrawer : Drawer<Gizmo2DDrawer> {

        public Gizmo2DDrawer Color(Color c) { return new WithColorDrawer(c); }
    
        public Gizmo2DDrawer DrawGrid(Vector2 center, int count, float span = 1) {
            BeforeDraw();
            var segments = new List<Vector3>(count << 8 + 4);
            var bound = span * count;
            for (var i = -count; i <= count; i++) {
                var offset = span * i;

                segments.Add(new Vector3(center.x + offset, center.y - bound));
                segments.Add(new Vector3(center.x + offset, center.y + bound));
                segments.Add(new Vector3(center.x - bound, center.y + offset));
                segments.Add(new Vector3(center.x + bound, center.y + offset));
            }

            UnityEditor.Handles.DrawDottedLines(segments.ToArray(), 2);
            return AfterDraw();
        }

        public Gizmo2DDrawer DrawBoundLBRT(BoundLBRT bound, uint split) {
            BeforeDraw();
            UnityEditor.Handles.DrawPolyLine(
                bound.leftBottom,
                bound.rightBottom,
                bound.rightTop,
                bound.leftTop,
                bound.leftBottom
            );
            return AfterDraw();
        }

        public Gizmo2DDrawer DrawBoundLBRT(BoundLBRT bound) {
            BeforeDraw();
            UnityEditor.Handles.DrawPolyLine(
                bound.leftBottom,
                bound.rightBottom,
                bound.rightTop,
                bound.leftTop,
                bound.leftBottom
            );
            return AfterDraw();
        }

        public Gizmo2DDrawer DrawRound(Vector2 pos, float radius) {
            BeforeDraw();
            UnityEditor.Handles.DrawSolidDisc(pos, Vector3.back, radius);
            return AfterDraw();
        }
        
        public Gizmo2DDrawer DrawLabel(Vector2 pos, string text) {
            BeforeDraw();
            UnityEditor.Handles.Label(pos, text);
            return AfterDraw();
        }

        public Gizmo2DDrawer DrawLine(Vector2[] poses) {
            BeforeDraw();
            var maxInd = poses.Length - 1;
            var lines = new Vector3[maxInd * 2];
            for (var i = 0; i < poses.Length; i++) {
                var node = poses[i];
                if (i != 0) lines[i * 2 - 1] = node;
                if (i != maxInd) lines[i * 2] = node;
            }
            UnityEditor.Handles.DrawLines(lines);
            return AfterDraw();
        }
    
        public Gizmo2DDrawer DrawLine(List<Vector2> poses) {
            BeforeDraw();
            var maxInd = poses.Count - 1;
            var lines = new Vector3[maxInd * 2];
            for (var i = 0; i < poses.Count; i++) {
                var node = poses[i];
                if (i != 0) lines[i * 2 - 1] = node;
                if (i != maxInd) lines[i * 2] = node;
            }
            UnityEditor.Handles.DrawLines(lines);
            return AfterDraw();
        }
    }
#endif
}