using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simple4X
{
    // TODO: Move these coordinate structs to separate files
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

    public struct Axial
    {
        public int q, r;

        public Axial(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        public static explicit operator Axial(Offset offset)
        {
            int q = offset.col - (offset.row - (offset.row & 1)) / 2;
            int r = offset.row;
            return new Axial(q, r);
        }
    }

    // TODO: Rename this to TileMap. Then we can have separate TileMap, InfluenceMap, PopulationMap, etc... classes 
    public class Grid : MonoBehaviour
    {
        public const float HexSize = 1.0f;
        public static readonly float HexWidth = Mathf.Sqrt(3) * HexSize;
        public static readonly float HexHeight = 2 * HexSize;

        [SerializeField, Range(1, 512)] int height = 128;
        [SerializeField, Range(1, 512)] int width = 128;

        TileComponent[] tileTypes;
        int[][] tiles;
        Transform[][] tileRoots;

        void Awake()
        {
            Debug.AssertFormat(width > 0, "Width must be greater that zero, width is currently: {0}", width);
            Debug.AssertFormat(height > 0, "Height must be greater that zero, height is currently: {0}", height);

            // Initialize the map
            // All zeroes for now.
            tiles = new int[width][];
            for (int i = 0; i < width; ++i)
            {
                tiles[i] = new int[height];
                for (int j = 0; j < height; ++j) {
                    tiles[i][j] = 0; // Might be redundant
                }
            }

            tileRoots = new Transform[width][];
            for (int q = 0; q < width; ++q)
            {
                tileRoots[q] = new Transform[height];
                for (int r = 0; r < height; ++r) {
                    var g = new GameObject();
                    g.transform.SetParent(transform);
                    g.transform.localPosition = GetBlockCenter((Offset)new Axial(q, r));
                    g.name = String.Format("{0}, {1}", q, r);
                    tileRoots[q][r] = g.transform;
                }
            }

            InitializeTileTypes();

            for (int q = 0; q < width; ++q) {
                for (int r = 0; r < height; ++r) {
                    SetTile(new Axial(q, r), Tile.Empty);
                }
            }
        }

        void InitializeTileTypes() {
            tileTypes = new TileComponent[Enum.GetValues(typeof(Tile)).Length];
            foreach (var component in GetComponentsInChildren<TileComponent>()) {
                Tile tileType = component.Type;
                int tileID = (int)tileType;
                if (tileTypes[tileID] == null) {
                    Debug.LogFormat("Initialized tile: {0} [ID: {1}]", tileType, tileID);
                    tileTypes[tileID] = component;
                }
                else {
                    throw new Exception(String.Format("Tile type {0} is already set.", tileType));
                }
            }
        }

        Tile GetTile(Axial pos) {
            // TODO: Check bounds
            return (Tile)tiles[pos.q][pos.r];
        }

        void SetTile(Axial pos, Tile tile) {
            // TODO: Check bounds
            var currentType = tiles[pos.q][pos.r];
            tileTypes[currentType].RemoveTile(this, pos, tileRoots[pos.q][pos.r]);

            var newID = (int)tile;
            tiles[pos.q][pos.r] = newID;
            tileTypes[newID].SetUpTile(this, pos, tileRoots[pos.q][pos.r]);
        }

        public Vector3 GetBlockCenter(Offset offset)
        {
            float x = offset.col * HexWidth + 0.5f * HexWidth * (offset.row & 1);
            float z = offset.row * HexHeight * 0.75f;
            return new Vector3(x, transform.position.y, z);
        }
    }

    // TileMap
    //  - GetTile(x, y)
    //  - SetTile(x, y)
}
