using UnityEngine;

namespace Simple4X {
    public struct Offset
    {
        public int row;
        public int col;

        public Offset(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public static explicit operator Offset(Axial axial)
        {
            int row = axial.r;
            int col = axial.q + (axial.r - (axial.r & 1)) / 2;
            return new Offset(row, col);
        }
    }

}