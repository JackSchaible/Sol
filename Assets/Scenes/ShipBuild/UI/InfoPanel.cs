using System;
using System.Collections.Generic;
using Assets.Data;
using Assets.Ships.Modules;
using Assets.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.ShipBuild.UI
{
    public class InfoPanel : MonoBehaviour
    {
        public GameObject ContentArea;
        public Text Text;
        public GameObject ConnectorViewPrefab;
        public GameObject ConnectorIconPrefab;

        protected List<GameObject> GameObjects;

        private void Start()
        {
            GameObjects = new List<GameObject>();
        }

        public virtual void ShowConnectors(Connector[] connectors)
        {
            foreach (var obj in GameObjects)
                Destroy(obj);

            GameObjects = new List<GameObject>();

            Connector[] conns;
            if (connectors == null || connectors.Length == 0)
                conns = new[]
                {
                    new Connector(Vector3Int.up, null), 
                    new Connector(Vector3Int.right, null), 
                    new Connector(Vector3Int.down, null), 
                    new Connector(Vector3Int.left, null), 
                    new Connector(new Vector3Int(0, 0, 1), null), 
                    new Connector(new Vector3Int(0, 0, -1), null), 
                };
            else
                conns = connectors;

            foreach (Connector con in conns)
            {
                var view = Instantiate(ConnectorViewPrefab, ContentArea.transform);
                view.transform.SetAsFirstSibling();
                var layoutGroupTransform = view.GetComponentInChildren<HorizontalLayoutGroup>().transform;
                var dir = Instantiate(ConnectorIconPrefab, layoutGroupTransform);
                dir.GetComponent<Image>().sprite = GraphicsUtils.GetSpriteFromPath(GetSpriteFromDirections(con.Direction));

                GameObjects.Add(view);

                if (con.MaterialsConveyed == null) continue;

                foreach (Materials mat in con.MaterialsConveyed)
                {
                    var go = Instantiate(ConnectorIconPrefab, layoutGroupTransform);
                    go.GetComponent<Image>().sprite = GraphicsUtils.GetSpriteFromPath(GetSpriteFromMaterial(mat));
                }
            }
        }

        private string GetSpriteFromDirections(Vector3Int dir)
        {
            if (dir == Vector3Int.up)
                return "ShipBuild/Info Panel/Connector Direction - Up";

            if (dir == Vector3Int.down)
                return "ShipBuild/Info Panel/Connector Direction - Down";

            if (dir == Vector3Int.left)
                return "ShipBuild/Info Panel/Connector Direction - Left";

            if (dir == Vector3Int.right)
                return "ShipBuild/Info Panel/Connector Direction - Right";

            if (dir == new Vector3Int(0, 0, 1))
                return "ShipBuild/Info Panel/Connector Direction - Forward";

            if (dir == new Vector3Int(0, 0, -1))
                return "ShipBuild/Info Panel/Connector Direction - Back";

            return "";
        }
        private string GetSpriteFromMaterial(Materials m)
        {
            switch (m)
            {
                case Materials.Power:
                    return "ShipBuild/Info Panel/Connector - Power";
                case Materials.Water:
                    return "ShipBuild/Info Panel/Connector - Water";
                case Materials.Air:
                    return "ShipBuild/Info Panel/Connector - Air";
                case Materials.Waste:
                    return "ShipBuild/Info Panel/Connector - Waste";
                case Materials.Heat:
                    return "ShipBuild/Info Panel/Connector - Heat";
                case Materials.People:
                    return "ShipBuild/Info Panel/Connector - People";
                default:
                    throw new ArgumentOutOfRangeException("m", m, null);
            }
        }
    }
}
