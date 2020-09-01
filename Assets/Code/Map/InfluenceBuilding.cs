using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple4X
{
    public class InfluenceBuilding : TileComponent
    {
        [SerializeField] int radius = 3;
        [SerializeField] int centerInfluence = 3;

        public override void SetUpTile(Grid grid, Axial position, Transform root)
        {
            base.SetUpTile(grid, position, root);

            Queue<Axial> toVisit = new Queue<Axial>();
            foreach (Axial neighbour in position.GetNeighbours()) {
                toVisit.Enqueue(neighbour);
            }

            HashSet<Axial> visited = new HashSet<Axial>();
            while (toVisit.Count > 0) {
                Axial neighbour = toVisit.Dequeue();
                if (visited.Contains(neighbour)) {
                    continue;
                }

                visited.Add(neighbour);

                if (grid.IsWithinBounds(neighbour)) {
                    grid.playerInfluence[neighbour.q][neighbour.r] += Mathf.FloorToInt(centerInfluence * (1.0f - Axial.Distance(neighbour, position) / (float)radius));
                }

                foreach(Axial next in neighbour.GetNeighbours()) {
                    if (Axial.Distance(next, position) < radius){
                        toVisit.Enqueue(next);
                    }
                }
            }
        }

        public override void RemoveTile(Grid grid, Axial position, Transform root) {
            base.RemoveTile(grid, position, root);

            Queue<Axial> toVisit = new Queue<Axial>();
            foreach (Axial neighbour in position.GetNeighbours()) {
                toVisit.Enqueue(neighbour);
            }

            HashSet<Axial> visited = new HashSet<Axial>();
            while (toVisit.Count > 0) {
                Axial neighbour = toVisit.Dequeue();
                if (visited.Contains(neighbour)) {
                    continue;
                }

                visited.Add(neighbour);

                if (grid.IsWithinBounds(neighbour)) {
                    grid.playerInfluence[neighbour.q][neighbour.r] -= Mathf.FloorToInt(centerInfluence * (1.0f - Axial.Distance(neighbour, position) / (float)radius));
                }

                foreach(Axial next in neighbour.GetNeighbours()) {
                    if (Axial.Distance(next, position) < radius){
                        toVisit.Enqueue(next);
                    }
                }
            }
        }
    }
}