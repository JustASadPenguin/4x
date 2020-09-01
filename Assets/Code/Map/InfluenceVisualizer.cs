using UnityEngine;

namespace Simple4X {
    public class InfluenceVisualizer : TileComponent {
        [SerializeField] Material positive;
        [SerializeField] Material negative;

        public override void InfluenceUpdated(Grid grid, Axial position) {
            var renderers = grid.tileRoots[position.q][position.r].GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in renderers) {
                renderer.material = grid.TEMPplayerInfluence[position.q][position.r] > 0 ? positive : negative;
            }
        }
    }
}