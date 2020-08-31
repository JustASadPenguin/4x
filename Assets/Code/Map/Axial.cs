using UnityEngine;

namespace Simple4X {
    public struct Axial {
        public int q;
        public int r;

        public const Axial Zero = new Axial(0, 0);

        public const Axial TopLeft = new Axial(0, -1);
        public const Axial TopRight = new Axial(1, -1);
        public const Axial Right = new Axial(1, 0);
        public const Axial BottomRight = new Axial(-1, 0);
        public const Axial BottomLeft = new Axial(-1, 1);
        public const Axial Left = new Axial(0, 1);

        public const Axial[] Neighbours = {
            TopLeft, TopRight, Right, BottomRight, BottomLeft, Left
        };

        public Axial(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        public static Axial operator +(Axial lhs, Axial rhs) {
            return new Axial(lhs.q + rhs.q, lhs.r + rhs.r);
        }

        public static Axial operator -(Axial lhs, Axial rhs) {
            return new Axial(lhs.q - rhs.q, lhs.r - rhs.r);
        }

        public static Axial operator *(float scalar, Axial rhs) {
            return Axial.Round(rhs.q * scalar, rhs.r * scalar);
        }

        public static Axial operator *(Axial lhs, float scalar) {
            return Axial.Round(lhs.q * scalar, lhs.r * scalar);
        }

        public  static Axial operator /(float scalar, Axial rhs)
        {
            return Axial.Round(rhs.q / scalar, rhs.r / scalar);
        }

        public static Axial operator /(Axial lhs, float scalar)
        {
            return Axial.Round(lhs.q / scalar, lhs.r / scalar);
        }
        
        public static bool operator ==(Axial lhs, Axial rhs) {
            return lhs.q == rhs.q && lhs.r == rhs.r;
        }

        public static bool operator !=(Axial lhs, Axial rhs) {
            return !(lhs == rhs);
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
