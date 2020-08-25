using System;
using UniKh.dataStructure;

namespace UniKh.space {
    
    [Serializable]
    public class Coord {
        public V2I Size { get;  }
        
        public int XMax => Size.x;
        public int YMax => Size.y;

        public int Length => Size.Area00;

        public Window FullWindow => fullWindow ?? (fullWindow = GetWindow(V2I.zero, Size));

        public Window fullWindow = null;

        public Coord() { }

        public Coord(int rowSize, int colSize) {
            this.Size = new V2I(colSize, rowSize);
        }
        
        public Coord(uint rowSize, uint colSize) : this((int)rowSize, (int)colSize){
        }

        public int Index(int coordRow, int coordCol) {
            return coordRow * Size.Col + coordCol;
        }
        
        public int Index(V2I coord) {
            return coord.Row * Size.Col + coord.Col;
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
        
        public Window GetWindow(int rowFrom, int rowTo, int colFrom, int colTo) {
            return new Window(ClampInPlace(new V2I(colFrom, rowFrom)), ClampInPlace(new V2I(colTo, rowTo)));
        }
        
        public Window GetWindow(V2I from, V2I to) {
            return GetWindow(from.Row, to.Row, from.Col, to.Col);
        }
        
        public Window GetWindow(int coordRow, int coordCol,  int distance = 1) {
            return GetWindow(coordRow - distance, coordRow + distance, coordCol - distance, coordCol + distance);
        }
        
        [Serializable]
        public class Window {

            public Window(int colFrom, int rowFrom, int colTo, int rowTo) {
                From = new V2I(colFrom, rowFrom);
                To = new V2I(colTo, rowTo);
            }
        
            public Window(V2I from, V2I to) {
                From = from;
                To = to;
            }
        
            public V2I From { get; }
            public V2I To { get; }

            public int RowMin => From.y;
            public int RowMax => To.y;
            public int ColMin => From.x;
            public int ColMax => To.x;

            public int SizeX => To.x - From.x + 1;
            public int SizeY => To.y - From.y + 1;
 
            public void ForEachPos(Action<int, int> func) {
                for (var i = ColMin; i <= ColMax; i++) {
                    for (var j = RowMin; j <= RowMax; j++) {
                        func(i, j);
                    }
                }
            }
        }
    }

    
}