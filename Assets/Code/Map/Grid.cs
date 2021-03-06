﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simple4X
{
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
        public int[][] TEMPplayerInfluence; // Just the 1 player for now lol. Each company will need a separate grid
        public Transform[][] tileRoots;

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
            }


            // Initialize tile roots
            // TODO: Only spawn them as needed...
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

            // Register tile types
            InitializeTileTypes();

            // Set all tiles to empty
            for (int q = 0; q < width; ++q) {
                for (int r = 0; r < height; ++r) {
                    var tile = (int)Tile.Empty;
                    tiles[q][r] = tile;
                    tileTypes[tile].SetUpTile(this, new Axial(q, r), tileRoots[q][r]);
                }
            }

            // TEMP
            TEMPplayerInfluence = new int[width][];
            for (int i = 0; i < width; ++i) {
                TEMPplayerInfluence[i] = new int[height];
            }
            int q_ = UnityEngine.Random.Range(0, width);
            int r_ = UnityEngine.Random.Range(0, height);
            tiles[q_][r_] = (int)Tile.Influence;
            tileTypes[(int)Tile.Influence].SetUpTile(this, new Axial(q_, r_), tileRoots[q_][r_]);
        }

        public void TEMPAddInfluence(Axial position, int value) {
            TEMPplayerInfluence[position.q][position.r] += value;
            var comp = GetTileComponentAt(position);
            comp.InfluenceUpdated(this, position);
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Axial position;
                if (RaycastMouse(out position)) {
                    var tile = GetTileComponent(Tile.Influence);
                    if (tile.CanBePlaced(this, position)) {
                        this[position] = Tile.Influence;
                    }
                }
            }

            for (int q = 0; q < width; ++q) {
                for (int r = 0; r < height; ++r) {
                    Vector3 worldPos = transform.TransformPoint(GetBlockCenter((Offset)new Axial(q, r)));
                    Debug.DrawRay(worldPos, Vector3.up * TEMPplayerInfluence[q][r], Color.red);
                }
            }
        }

        void InitializeTileTypes() {
            tileTypes = new TileComponent[Enum.GetValues(typeof(Tile)).Length];
            foreach (var component in GetComponentsInChildren<TileComponent>()) {
                Tile tileType = component.Type;
                if (tileType == Tile.Undefined) {
                    throw new Exception("TILE TYPE IS UNDEFINED!!! Make sure you add the new tile to Tile.cs and set the TileComponent field in the inspector.");
                }
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

        public bool RaycastMouse(out Axial position) {
            // TODO: Robustness - use the correct camera at all times
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Raycast(ray, out position);
        }

        public bool Raycast(Ray ray, out Axial position) {
            Plane horizontalPlane = new Plane(Vector3.up, transform.position);

            float distance = 0.0f;
            if (horizontalPlane.Raycast(ray, out distance)) {
                Vector3 hitWorldPosition = ray.origin + ray.direction * distance;
                Vector3 hitLocalPosition = transform.InverseTransformPoint(hitWorldPosition);

                float q = (Mathf.Sqrt(3.0f) / 3.0f * hitLocalPosition.x - 1.0f / 3.0f * hitLocalPosition.z) / HexSize;
                float r = (2.0f / 3.0f * hitLocalPosition.z) / HexSize;
                position = Axial.Round(q, r);
                return IsWithinBounds(position);
            }
            position = new Axial();
            return false;
        }

        public TileComponent GetTileComponent(Tile tile) {
            var tileComponent = tileTypes[(int)tile];
            if (tileComponent == null) {
                throw new ArgumentException(String.Format("Tile type {0} has not been initialized.", tile));
            }
            return tileComponent;
        }

        public TileComponent GetTileComponentAt(Axial axial) {
            var tileComponent = tileTypes[tiles[axial.q][axial.r]];
            return tileComponent;
        }

        public bool IsWithinBounds(Axial position) {
            return position.q >= 0 && position.q < width && position.r >= 0 && position.r < height;
        }

        void SetTile(Axial pos, Tile tile) {
            // TODO: Check bounds
            var currentType = tiles[pos.q][pos.r];
            tileTypes[currentType].RemoveTile(this, pos, tileRoots[pos.q][pos.r]);

            var newID = (int)tile;
            tiles[pos.q][pos.r] = newID;
            tileTypes[newID].SetUpTile(this, pos, tileRoots[pos.q][pos.r]);
        }

        public Tile this[int q, int r] {
            get {
                return this[new Axial(q, r)];
            }
            set {
                this[new Axial(q, r)] = value;
            }
        }

        public Tile this[Axial axial] {
            get {
                return (Tile)tiles[axial.q][axial.r];
            }
            set {
                SetTile(axial, value);
            }
        }

        public Tile this[Offset offset] {
            get {
                var axial = (Axial)offset;
                return (Tile)tiles[axial.q][axial.r];
            }
            set {
                SetTile((Axial)offset, value);
            }
        }

        public Vector3 GetBlockCenter(Offset offset)
        {
            float x = offset.col * HexWidth + 0.5f * HexWidth * (offset.row & 1);
            float z = offset.row * HexHeight * 0.75f;
            return new Vector3(x, transform.position.y, z);
        }
    }
}
