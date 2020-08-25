using System;
using System.Linq;
using UnityEngine;

namespace Simple4X {
    public struct Offset {
        public int row;
        public int col;

        public Offset(int row, int col) {
            this.row = row;
            this.col = col;
        }

        public static explicit operator Offset(Axial axial) {
            int row = axial.r;
            int col = axial.q + (axial.r - (axial.r & 1)) / 2;
            return new Offset(row, col);
        }
    }

    public struct Axial {
        public int q, r;

        public Axial(int q, int r) {
            this.q = q;
            this.r = r;
        }

        public static explicit operator Axial(Offset offset) {
            int q = offset.col - (offset.row - (offset.row & 1)) / 2;
            int r = offset.row;
            return new Axial(q, r);
        }
    }

    public class Grid : MonoBehaviour {
        public const float HexSize = 1.0f;
        public static readonly float HexWidth = Mathf.Sqrt(3) * HexSize;
        public static readonly float HexHeight = 2 * HexSize;

        [SerializeField, Range(1, 512)] int height_ = 128;
        [SerializeField, Range(1, 512)] int width_ = 128;
        [SerializeField] GameObject[] tilePrefabs_ = new GameObject[0];

        TileComponent[][] tiles_;

        void Awake() {
            Debug.AssertFormat(width_ > 0, "Width must be greater that zero, width is currently: {0}", width_);
            Debug.AssertFormat(height_ > 0, "Height must be greater that zero, height is currently: {0}", height_);

            tiles_ = new TileComponent[width_][];
            for (int i = 0; i < height_; ++i) {
                tiles_[i] = new TileComponent[height_];
            }

            for (int row = 0; row < height_; ++row) {
                for (int col = 0; col < width_; ++col) {
                    var center = GetBlockCenter(new Offset(row, col));
                    var odds = UnityEngine.Random.Range(0.0f, 1.0f);
                    Instantiate(tilePrefabs_[odds > 0.11f ? 1 : 0], center, Quaternion.identity);
                }
            }
        }

        public Vector3 GetBlockCenter(Axial axial) {
            return GetBlockCenter((Offset)axial);
        }

        public Vector3 GetBlockCenter(Offset offset) {
            float x = offset.col * HexWidth + 0.5f * HexWidth * (offset.row & 1);
            float z = offset.row * HexHeight * 0.75f;
            return new Vector3(x, transform.position.y, z);
        }
    }
}
