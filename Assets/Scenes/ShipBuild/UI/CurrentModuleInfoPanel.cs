using System.Collections.Generic;
using System.Linq;
using Assets.Ships.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.ShipBuild.UI
{
    public class CurrentModuleInfoPanel : InfoPanel
    {
        public Text TitleText;
        private Dictionary<Vector3Int, Connector[]> _connectors;
        private int _index;

        private void Start()
        {
            _connectors = new Dictionary<Vector3Int, Connector[]>();
            GameObjects = new List<GameObject>();
        }

        public void Initialize(string moduleName, Connector[] connectors)
        {
            TitleText.text = moduleName + " Info";
            _index = 0;

            var groups = connectors.GroupBy(x => x.Position);
            foreach (var connGroup in groups)
                _connectors.Add(connGroup.Key, connGroup.ToArray());

            ShowConnectors(_connectors.Values.First(), false);
            gameObject.SetActive(true);
        }

        public void Show(Vector3Int connectorGroup)
        {
            ShowConnectors(_connectors[connectorGroup], false);
        }

        public void Disable()
        {
            _connectors = new Dictionary<Vector3Int, Connector[]>();
            TitleText.text = "";
            gameObject.SetActive(false);
        }
    }
}
