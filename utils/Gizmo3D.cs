using System.Collections.Generic;
using UniKh.space;
using UniKh.extensions;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace UniKh.utils {
#if UNITY_EDITOR

    public static class Gizmo3D {
        public static Gizmo3DDrawer drawer => _drawer ?? (_drawer = new Gizmo3DDrawer());

        private static Gizmo3DDrawer _drawer;

        public static Gizmo3DDrawer C(Color c) => new WithColorDrawer3D(c);

        private static List<Color> colorQueue = new List<Color>();
        public static Gizmo3DDrawer StartColor(Color c) {
            colorQueue.Push(UnityEditor.Handles.color);
            UnityEditor.Handles.color = c;
            return _drawer;
        }
    
    
        public static void EndColor() {
            UnityEditor.Handles.color = colorQueue.Pop();
        }
    }
 

    public class WithColorDrawer3D : Gizmo3DDrawer {
        private Color color { get; }

        public WithColorDrawer3D(Color c) {
            color = c;
        }


        private Color cOrigin;
        protected override void BeforeDraw() {
            base.BeforeDraw();
            cOrigin = UnityEditor.Handles.color;
            UnityEditor.Handles.color = color;
        }

        protected override Gizmo3DDrawer AfterDraw() {
            UnityEditor.Handles.color = cOrigin;
            return base.AfterDraw();
        }
    }

    public class Gizmo3DDrawer : Drawer<Gizmo3DDrawer> {

        public Gizmo3DDrawer Color(Color c) { return new WithColorDrawer3D(c); }
     
        public Gizmo3DDrawer DrawPlane3D(Plane3D plane3D) {
            BeforeDraw();
            // Todo
            return AfterDraw();
        }

        public Gizmo3DDrawer DrawQuadrilateral3D(Quadrilateral3D quadrilateral) {
            BeforeDraw(); 
            UnityEditor.Handles.DrawAAPolyLine(
                quadrilateral.lb,
                quadrilateral.lt,
                quadrilateral.rt,
                quadrilateral.rb,
                quadrilateral.lb
            );
            return AfterDraw();
        }
 
        public Gizmo3DDrawer DrawRound(Vector3 pos, Vector3 normal, float radius, bool fill = false) {
            BeforeDraw();
            if (fill) {
                UnityEditor.Handles.DrawSolidDisc(pos, normal, radius);
            }
            else {
                UnityEditor.Handles.DrawWireDisc(pos, normal, radius);
            }

            return AfterDraw();
        }
        
        public Gizmo3DDrawer DrawLabel(Vector3 pos, string text, int fontSize = 0) {
            BeforeDraw();
            if (fontSize > 0) {
                var style = new GUIStyle(GUI.skin.label);
                style.fontSize = fontSize;
                UnityEditor.Handles.Label(pos, text, style);
            }
            else {
                UnityEditor.Handles.Label(pos, text);
            }
            return AfterDraw();
        }

        public Gizmo3DDrawer DrawAAPolyLine(bool closure, params Vector3[] poses) {
            if (poses.Length < 2) return this;
            BeforeDraw();
            var lst = poses;
            if (closure) {
                lst = new Vector3[poses.Length + 1];
                poses.CopyTo(lst, 0);
                lst[poses.Length] = poses[0];
            }
            UnityEditor.Handles.DrawAAPolyLine(lst);
            
            return AfterDraw();
        }
        
        public Gizmo3DDrawer DrawDottedLines(float screenSpaceSize, params Vector3[] poses) {
            if (poses.Length < 2) return this;
            BeforeDraw(); 
            UnityEditor.Handles.DrawDottedLines(poses, screenSpaceSize);
            
            return AfterDraw();
        }
    }
#endif
}