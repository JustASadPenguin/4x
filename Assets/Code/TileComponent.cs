using System;
using UnityEngine;

namespace Simple4X {
    public class TileComponent : MonoBehaviour {
        [SerializeField] Tile type;

        public Tile Type {
            get { return type; }
        }

        public virtual void SetUpTile(Grid grid, Axial position, Transform root) {
        }

        public virtual void RemoveTile(Grid grid, Axial position, Transform root) {
            // By default just remove all children from the root
            for (int i = 0; i < root.childCount; ++i) {
                Destroy(root.GetChild(i));
            }
        }
    }
}