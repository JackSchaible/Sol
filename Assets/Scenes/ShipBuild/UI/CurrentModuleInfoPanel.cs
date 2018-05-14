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
        private Dictionary<Vector3Int, Connector[]> _connectors = new Dictionary<Vector3Int, Connector[]>();
        private int _index;

        private void Start()
        {
        }

        public void Initialize(string moduleName, Connector[] connectors)
        {
            TitleText.text = moduleName + " Info";
            _index = 0;

            foreach (var connector in connectors)
                if (_connectors.ContainsKey(connector.Position))
                {
                    var newConns = _connectors[connector.Position].ToList();
                    newConns.Add(connector);
                    _connectors[connector.Position] = newConns.ToArray();
                }
                else
                    _connectors.Add(connector.Position, new[] {connector});

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
