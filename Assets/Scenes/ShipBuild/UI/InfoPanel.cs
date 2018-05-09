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

        private List<GameObject> _gameObjects;

        private void Start()
        {
            _gameObjects = new List<GameObject>();
        }

        public void ShowConnectors(Connector[] connectors)
        {
            foreach (var obj in _gameObjects)
                Destroy(obj);

            _gameObjects = new List<GameObject>();

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

            for (var i = 0; i < conns.Length; i++)
            {
                int yOffset = 55 * i;

                var dir = new GameObject("Direction " + conns[i].Direction + " Icon", typeof(Image));
                dir.GetComponent<Image>().sprite = GraphicsUtils.GetSpriteFromPath(GetSpriteFromDirections(conns[i].Direction));
                dir.transform.SetParent(ContentArea.transform, false);
                dir.transform.position = new Vector3(0, yOffset, 0);

                _gameObjects.Add(dir);

                if (conns[i].MaterialsConveyed != null)
                    for (int j = 0; j < conns[i].MaterialsConveyed.Length; j++)
                    {
                        int xOffset = 55 + (j * 50);

                        var mat = new GameObject(conns[i].MaterialsConveyed[j] + " Icon", typeof(Image));
                        mat.GetComponent<Image>().sprite = GraphicsUtils.GetSpriteFromPath(GetSpriteFromMaterial(conns[i].MaterialsConveyed[j]));
                        mat.transform.SetParent(ContentArea.transform, false);
                        mat.transform.position = new Vector3(xOffset, yOffset, 0);

                        _gameObjects.Add(mat);
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
