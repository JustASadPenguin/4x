using UnityEngine;

namespace Simple4X {
    public class TileComponent : MonoBehaviour {
        [SerializeField] Tile type_;

        public Tile Type {
            get { return type_; }
        }
    }
}