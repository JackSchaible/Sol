using Assets.Ships.Modules;
using UnityEngine;

public class ModuleComponent : MonoBehaviour
{
    public GameObject GameObject { get; set; }
    public Vector3Int LocalPosition { get; set; }
    public Connector[] Connectors { get; set; }
    public Connector[] IncomingConnectors { get; set; }
    public ExclusionVector[] ExclusionVectors { get; set; }

    public ModuleComponent(Vector3Int localPosition, Connector[] connectors, ExclusionVector[] exclusionVectors)
    {
        LocalPosition = localPosition;
        Connectors = connectors;
        ExclusionVectors = exclusionVectors;
    }
}
