using Assets.Ships.Modules;
using UnityEngine;

namespace Assets.Scenes.ShipBuild.UI
{
    public class GridCell : MonoBehaviour
    {
        public Vector3Int Position { get; set; }
        public Connector[] Connectors { get; set; }
        public ExclusionVector[] ExclusionVectors { get; set; }

        public GridCell()
        {
            
        }

        public GridCell(Vector3Int position, Connector[] connectors, ExclusionVector[] exclusionVectors)
        {
            Position = position;
            Connectors = connectors;
            ExclusionVectors = exclusionVectors;
        }
    }
}
