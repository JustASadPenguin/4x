using System;
using UnityEngine;

namespace Simple4X {
    public struct Axial {
        public int q;
        public int r;

        public static readonly Axial Zero = new Axial(0, 0);

        public static readonly Axial TopLeft = new Axial(0, -1);
        public static readonly Axial TopRight = new Axial(1, -1);
        public static readonly Axial Right = new Axial(1, 0);
        public static readonly Axial BottomRight = new Axial(-1, 0);
        public static readonly Axial BottomLeft = new Axial(-1, 1);
        public static readonly Axial Left = new Axial(0, 1);

        public static readonly Axial[] Neighbours = {
            TopLeft, TopRight, Right, BottomRight, BottomLeft, Left
        };

        public Axial(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        public Axial[] GetNeighbours() {
            Axial[] neighbours = new Axial[Neighbours.Length];
            Array.Copy(Neighbours, neighbours, neighbours.Length);
            for (int i = 0; i < neighbours.Length; ++i) {
                neighbours[i] += this;
            }
            return neighbours;
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

        public static float Distance(Axial a, Axial b) {
            int dq = a.q - b.q;
            int dr = a.r - b.r;
            return Mathf.Sqrt(dq * dq + dr * dr + dq * dr);
        }

        public static explicit operator Axial(Offset offset)
        {
            int q = offset.col - (offset.row - (offset.row & 1)) / 2;
            int r = offset.row;
            return new Axial(q, r);
        }
    }
}
