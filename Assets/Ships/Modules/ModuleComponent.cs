﻿using UnityEngine;

namespace Assets.Ships.Modules
{
    public struct ModuleComponent
    {
        public GameObject GameObject { get; set; }
        public Vector3Int LocalPosition { get; set; }
        public Connector[] Connectors { get; set; }
        public ExclusionVector[] ExclusionVectors { get; set; }

        public ModuleComponent(GameObject gameObject, Vector3Int localPosition, Connector[] connectors, ExclusionVector[] exclusionVectors)
        {
            GameObject = gameObject;
            LocalPosition = localPosition;
            Connectors = connectors;
            ExclusionVectors = exclusionVectors;
        }
    }
}
