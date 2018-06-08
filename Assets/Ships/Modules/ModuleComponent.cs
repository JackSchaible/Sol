using UnityEngine;

namespace Assets.Ships.Modules
{
    public struct ModuleComponent
    {
        public GameObject GameObject { get; set; }
        public Sprite BuildSprite { get; set; }
        public Sprite Sprite { get; set; }
        public Vector3Int LocalPosition { get; set; }
        public Connector[] Connectors { get; set; }
        public ExclusionVector[] ExclusionVectors { get; set; }

        public ModuleComponent(GameObject gameObject, Sprite buildSprite, Sprite sprite, Vector3Int localPosition, Connector[] connectors, ExclusionVector[] exclusionVectors) : this()
        {
            GameObject = gameObject;
            BuildSprite = buildSprite;
            Sprite = sprite;
            LocalPosition = localPosition;
            Connectors = connectors;
            ExclusionVectors = exclusionVectors;
        }
    }
}
