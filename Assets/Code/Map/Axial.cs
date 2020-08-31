using UnityEngine;

namespace Simple4X {
    public struct Axial {
        public int q;
        public int r;

        public Axial(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        public static Axial Round(float q, float r) {
            float x = q;
            float z = r;
            float y = -x - z;

            int rx = Mathf.RoundToInt(x);
            int ry = Mathf.RoundToInt(y);
            int rz = Mathf.RoundToInt(z);

            float xdiff = Mathf.Abs(rx - x);
            float ydiff = Mathf.Abs(ry - y);
            float zdiff = Mathf.Abs(rz - z);

            if (xdiff > ydiff && xdiff > zdiff) {
                rx = -ry - rz;
            }
            else if (ydiff > zdiff) {
                ry = -rx - rz;
            }
            else {
                rz = -rx - ry;
            }

            return new Axial(rx, rz);
        }

        public static explicit operator Axial(Offset offset)
        {
            int q = offset.col - (offset.row - (offset.row & 1)) / 2;
            int r = offset.row;
            return new Axial(q, r);
        }
    }
}
