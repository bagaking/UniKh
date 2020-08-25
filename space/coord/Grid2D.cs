using UniKh.dataStructure;
using UniKh.space;
using UniKh.utils;

namespace UniKh.space {
    public class Grid2D : Coord {
        public V2I Center { get; }

        private readonly int[] gridMarks;

        public int this[int row, int col] {
            get => gridMarks[Index(col, row)];
            set => gridMarks[Index(col, row)] = value;
        }
        
        public int this[V2I coord] {
            get => gridMarks[Index(coord)];
            set => gridMarks[Index(coord)] = value;
        }
        
        public int this[int index] {
            get => gridMarks[index];
            set => gridMarks[index] = value;
        }

        public Grid2D(int rowSize, int colSize): base(rowSize, colSize) {
            gridMarks = new int[Length];
            Center = Size / 2;
        }

        public override string ToString() {
            var sb = SGen.New["grid2D"][FullWindow];
            for (var row = 0; row < Size.Row; row++) {
                sb.AppendLine().AppendFormat("{0:D2}", row).Append(". ");
                for (var col = 0; col < Size.Col; col++) {
                    sb[this[col, row]].Append(' ');
                }    
            }
            return sb.End;
        }
    }
}