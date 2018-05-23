using System.Collections.Generic;
using System.Linq;
using Assets.Ships.Modules;
using Assets.Utils.ModuleUtils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.ShipBuild.UI
{
    public class CurrentModuleInfoPanel : InfoPanel
    {
        public Text TitleText;
        private Dictionary<Vector3Int, List<Connector>> _connectors = new Dictionary<Vector3Int, List<Connector>>();
        private int _index;

        private void Start()
        {
        }

        public void Initialize(string moduleName, Connector[] connectors, Vector3Int currentModule, int rotations, int[] flips)
        {
            TitleText.text = moduleName + " Info";
            _index = 0;

            _connectors = new Dictionary<Vector3Int, List<Connector>>();
            var cons = new Connector[connectors.Length];
            connectors.CopyTo(cons, 0);
            cons = ModuleVectorUtils.RotateConnectorPositions(cons, rotations);
            cons = ModuleVectorUtils.FlipConnectorPositions(cons, flips);

            foreach (var connector in cons)
                if (_connectors.ContainsKey(connector.Position))
                    _connectors[connector.Position].Add(connector);
                else
                    _connectors.Add(connector.Position, new List<Connector> {connector});

            Show(currentModule);
            gameObject.SetActive(true);
        }

        public void Show(Vector3Int connectorGroup)
        {
            if (_connectors.ContainsKey(connectorGroup))
                ShowConnectors(_connectors[connectorGroup], false);
            else
                ShowConnectors(new List<Connector>(), false);
        }

        public void Disable()
        {
            _connectors = new Dictionary<Vector3Int, List<Connector>>();
            TitleText.text = "";
            gameObject.SetActive(false);
        }
    }
}
