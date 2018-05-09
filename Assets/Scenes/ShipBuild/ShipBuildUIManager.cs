using Assets.Data;
using Assets.Scenes.ShipBuild.MenuManager;
using Assets.Scenes.ShipBuild.UI;
using Assets.Ships;
using Assets.Utils.ModuleUtils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.ShipBuild
{
    public class ShipBuildUiManager : MonoBehaviour
    {
        #region Props & fields

        //Other GameObject Manager
        public DynamicBuildMenuManager Menu;
        public ShipBuildManager ShipBuildManager;
        public DeckManager DeckManager;

        //Reference to the scene's main camera (used for the transformation matrix)
        public Camera Camera;

        //UI Elements needing to be updated
        public Text ModulesText;
        public Text PowerText;
        public Text PeopleText;
        public Text DebugText;
        public Modal Modal;
        public InfoPanel InfoPanel;

        //Internal state-tracking variables
        private bool _placeMode;
        private Module _newModule;
        private Color _previousColor;
        private GameObject _previousModule;
        private float _newModuleRotation;
        private bool _popupActive;

        #endregion

        #region Main methods

        void Start()
        {
            Modal.Initialize(Modals.BuildMenu.CommandModulesModalData);
            Modal.ShowModal();

            //TODO: Only run this if ship is new
            var isnew = true;

            if (isnew)
                NewShipInitialization();

            Initialize();
        }

        void Initialize()
        {
            int x = ShipBuildManager.Cells.GetLength(0) / 2;
            int y = ShipBuildManager.Cells.GetLength(1) / 2;
            var pos = ShipBuildManager.Cells[x, y, 0].gameObject.transform.position;

            Camera.transform.position = new Vector3(pos.x, pos.y, Camera.transform.position.z);
        }

        void Update()
        {
            ModulesText.text = ShipBuildManager.ControlUsed + " / " + ShipBuildManager.ControlAvailable;
            PowerText.text = ShipBuildManager.PowerUsed + " / " + ShipBuildManager.PowerAvailable;
            PeopleText.text = ShipBuildManager.PersonnelUsed + " / " + ShipBuildManager.PersonnelAvailable;

            ModulesText.color = ShipBuildManager.ControlUsed > ShipBuildManager.ControlAvailable ?
                new Color(1, 0, 0) : new Color(1, 1, 1);

            PowerText.color = ShipBuildManager.PowerUsed > ShipBuildManager.PowerAvailable ?
                new Color(1, 0, 0) : new Color(1, 1, 1);

            PeopleText.color = ShipBuildManager.PersonnelUsed > ShipBuildManager.PersonnelAvailable ?
                new Color(1, 0, 0) : new Color(1, 1, 1);

            if (_newModuleRotation >= 360)
                _newModuleRotation -= 360;

            if (_newModuleRotation <= -360)
                _newModuleRotation += 360;

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
                    out hit) && hit.transform.tag == "GridCell")
            {
                DebugText.text = hit.transform.name;
                if (_previousModule != hit.transform.gameObject)
                {
                    if (_previousModule != null)
                        _previousModule.GetComponent<SpriteRenderer>().color = _previousColor;

                    _previousModule = hit.transform.gameObject;
                    var sr = _previousModule.GetComponent<SpriteRenderer>();
                    _previousColor = sr.color;
                    sr.color = new Color(0.5f, 0.5f, 1);
                    InfoPanel.ShowConnectors(ShipBuildManager.GetConnectors(_previousModule));
                }
            }

            if (Input.GetMouseButtonDown(0) && _placeMode && _previousModule != null)
                PlaceModule();

            UpdatePlace();

            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButton(1))
            {
                if (_placeMode)
                {
                    CancelPlaceMode();
                    Menu.PlaceCancelled();
                }
                else
                    Application.Quit();
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Preform any initialization needed when this scene is loaded to make a new ship 
        /// (i.e., one that doesn't already exist)
        /// </summary>
        private void NewShipInitialization()
        {
        }

        #endregion

        #region Place Mode

        /// <summary>
        /// Handles moving the new module sprite around the screen, locking it to the 50*50 pixel grid,
        /// and handling the controls (Escape, R/LCTRL+R, T/LCTRL+T, LMB Click)
        /// </summary>
        private void UpdatePlace()
        {
            if (!_placeMode) return;

            for (int i = 0; i < _newModule.Components.Length; i++)
            {
                _newModule.Components[i].GameObject.transform.position = 
                    Camera.ScreenToWorldPoint(Input.mousePosition) + 
                    (_newModule.Components[i].LocalPosition) + new Vector3(0, 0, 10);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
                _newModule = ModuleVectorUtils.RotateModule(_newModule, ModuleVectorUtils.RotationDirection.CCW);
            else if (Input.GetKeyDown(KeyCode.R))
                _newModule = ModuleVectorUtils.RotateModule(_newModule, ModuleVectorUtils.RotationDirection.CW);
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
            {
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
            }
        }

        /// <summary>
        /// Handles the moving of the build sprite to the UI BG layer, and notifies the 
        /// ShipBuildManager to add a new module to the ship
        /// </summary>
        private void PlaceModule()
        {
            ShipBuildManager.AddModule(_previousModule, _newModule);
            Menu.ModulePlaced(_newModule.ModuleBlueprint);
            CancelPlaceMode();
        }

        /// <summary>
        /// An external event called by the DynamicBuildMenuManager when the build button of
        /// a module details description is clicked. Enabled the place mode, and creates a 
        /// new sprite of whatever module was clicked on.
        /// </summary>
        /// <param name="blueprint">The blueprint to build</param>
        public void Build(ModuleBlueprint blueprint)
        {
            _newModule = Module.Create(blueprint);

            //if (_newModuleRotation != 0)
            //{
            //    _newModule.GameObject.transform.RotateAround(_newModule.GameObject.transform.position, new Vector3(0, 0, 1),
            //        _newModuleRotation);

            //    for(int rotations = (int)Math.Abs(_newModuleRotation) / 90; rotations > 0; rotations--)
            //        _newModule.ModuleBlueprint.Connectors =
            //            ModuleVectorUtils.RotateConnectorPositions(_newModule.ModuleBlueprint.Connectors,
            //                ModuleVectorUtils.RotationDirection.CCW);
            //}

            foreach (var com in _newModule.Components)
            {
                var sprite = com.GameObject.GetComponent<SpriteRenderer>();
                sprite.sortingLayerName = "UI BG";
                sprite.sortingOrder = DeckManager.CurrentDeck;
                _placeMode = true;
            }
        }

        private void CancelPlaceMode()
        {
            foreach(var mod in _newModule.Components)
                Destroy(mod.GameObject);

            _placeMode = false;
            _newModule = null;
        }

        #endregion
    }
}
