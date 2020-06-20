using UniKh.dataStructure;
using UniKh.extensions;
using UniKh.utils;

namespace UniKh.dataSource {
    public class Grid2D {
        public uint SizeVertical { get; }
        public uint SizeHorizontal { get; }

        public uint XCenter { get; }
        public uint XMin { get; }
        public uint XMax { get; }
        public uint YCenter { get; }
        public uint YMin { get; }
        public uint YMax { get; }

        private readonly int[][] gridMarks;

        public int this[int y, int x] {
            get => gridMarks[y][x];
            set => gridMarks[y][x] = value;
        }
        
        public int this[V2I pos] {
            get => gridMarks[pos.y][pos.x];
            set => gridMarks[pos.y][pos.x] = value;
        }

        public Grid2D(uint verticalSize, uint horizontalSize) {
            SizeVertical = verticalSize;
            SizeHorizontal = horizontalSize;
            gridMarks = new int[SizeVertical][];
            gridMarks.ForEach((_, i) => gridMarks[i] = new int[SizeVertical]);

            XCenter = SizeHorizontal / 2;
            XMin = 0;
            XMax = SizeHorizontal - 1;
            YCenter = SizeVertical / 2;
            YMin = 0;
            YMax = SizeVertical - 1;
        }

        public override string ToString() {
            var sb = SGen.New["grid2D("][SizeVertical][','][SizeHorizontal][')'];
            gridMarks.ForEach(
                (row, i) => {
                    sb.AppendLine().AppendFormat("{0:D2}", i).Append(". ");
                    row.ForEach(v => sb.Append(v).Append(' '));
                });
            return sb.End;
        }
    }
}