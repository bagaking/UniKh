using System;
using UniKh.dataStructure;

namespace UniKh.space {
    
    [Serializable]
    public class Coord {
        public V2I Size { get;  }
        
        public int XMax => Size.x;
        public int YMax => Size.y;

        public int Length => Size.Area00;

        public Coord() { }

        public Coord(int colSize, int rowSize) {
            this.Size = new V2I(colSize, rowSize);
        }
        
        public Coord(uint colSize, uint rowSize) : this((int)colSize, (int)rowSize){
        }

        public static float Manhattan(float disX, float disY) {
            return (disX < 0 ? -disX : disX) + (disY < 0 ? -disY : disY);
        }

        public int Index(int coordRow, int coordCol) {
            return coordRow * Size.Col + coordCol;
        }

        public int[] CreateMatrix() {
            return new int[Length];
        }

        public V2I ClampInPlace(V2I coord) {
            if (coord.Col < 0) coord.x = 0;
            else if (coord.Col >= Size.Col) coord.x = XMax;
            
            if (coord.Row < 0) coord.y = 0;
            else if (coord.Row >= Size.Row) coord.y = YMax;

            return coord;
        }
    
        public Window GetWindow(V2I from, V2I to) {
            return new Window(ClampInPlace(from), ClampInPlace(to));
        }
        
        public Window GetWindow(int rowFrom, int rowTo, int colFrom, int colTo) {
            return GetWindow(new V2I(colFrom, rowFrom), new V2I(colTo, rowTo));
        }
        
        public Window GetWindow(int coordX, int coordY, int distance = 1) {
            return GetWindow(coordY - distance, coordY + distance, coordX - distance, coordX + distance);
        }
        
        [Serializable]
        public class Window {

            public Window(int xFrom, int yFrom, int xTo, int yTo) {
                From = new V2I(xFrom, yFrom);
                To = new V2I(xTo, yTo);
            }
        
            public Window(V2I from, V2I to) {
                From = from;
                To = to;
            }
        
            public V2I From { get; }
            public V2I To { get; }

            public int XMin => From.x;
            public int YMin => From.y;
            public int XMax => To.x;
            public int YMax => To.y;

            public int SizeX => To.x - From.x + 1;
            public int SizeY => To.y - From.y + 1;
 
            public void ForEachPos(Action<int, int> func) {
                for (var i = XMin; i <= XMax; i++) {
                    for (var j = YMin; j <= YMax; j++) {
                        func(i, j);
                    }
                }
            }

       
        }
    }

    
}