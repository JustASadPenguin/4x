using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple4X
{
    public class InfluenceBuilding : TileComponent
    {
        [SerializeField] int radius = 4;

        public override void SetUpTile(Grid grid, Axial position, Transform root)
        {
            base.SetUpTile(grid, position, root);

            //iterate over neighbours
            // TODO: Add Neighbours() to axial: foreach (neighbour in position.Neighbours())
            Queue<Axial> toVisit = new Queue<Axial>();
            foreach (Axial neighbour in position.GetNeighbours()) {
                toVisit.Enqueue(neighbour);
            }

            HashSet<Axial> visited = new HashSet<Axial>();
            while (toVisit.Count > 0) {
                Axial neighbour = toVisit.Dequeue();
                visited.Add(neighbour);

                if (grid.IsWithinBounds(neighbour)) {
                    grid[neighbour] = Tile.Buildings;
                }

                foreach(Axial next in neighbour.GetNeighbours()) {
                    if (!visited.Contains(next) && Axial.Distance(next, position) < radius){
                        Debug.Log(Axial.Distance(next, position));
                        toVisit.Enqueue(next);
                    }
                }
            }
        }
    }
}