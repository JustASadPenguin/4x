using UnityEngine;

namespace Simple4X {
    public class TileComponent : MonoBehaviour {
        [SerializeField] Tile type_;
        [SerializeField] int[] influence = new int[1];

        public Tile Type {
            get { return type_; }
        }
    }
}