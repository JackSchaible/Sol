﻿using System.Collections.Generic;
using System.Linq;
using Assets.Data;
using Assets.Scenes.ShipBuild.MenuManager;
using Assets.Scenes.ShipBuild.UI;
using Assets.Ships;
using Assets.Ships.Modules;
using Assets.Utils;
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
        public CurrentModuleInfoPanel ModuleInfoPanel;

        //Internal state-tracking variables
        private bool _placeMode;
        private Module _newModule;
        private ModuleBlueprint _blueprint;
        private int _currentModuleComponent;
        private Color _previousColor;
        private GameObject _previousModule;
        private int _newModuleRotations;
        private int[] _newModuleFlips = new int[2];
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
            ModuleInfoPanel.gameObject.SetActive(false);
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

            if (_newModuleRotations == 4 || _newModuleRotations == -4)
                _newModuleRotations = 0;

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),//new Vector3(Input.mousePosition.x, Input.mousePosition.y, -DeckManager.CurrentDeck)),
                    out hit) && hit.transform.tag == "GridCell")
            {
                DebugText.text = hit.transform.name;
                if (_previousModule != hit.transform.gameObject)
                {
                    foreach (var cell in ShipBuildManager.Cells)
                    {
                        var gc = cell.GetComponent<GridCell>();
                        if (gc.Connectors.Length > 0)
                            Debug.Log(gc.Position);
                    }

                    if (_previousModule != null)
                        _previousModule.GetComponent<SpriteRenderer>().color = _previousColor;

                    _previousModule = hit.transform.gameObject;
                    var sr = _previousModule.GetComponent<SpriteRenderer>();
                    _previousColor = sr.color;
                    sr.color = new Color(0.5f, 0.5f, 1);
                    InfoPanel.ShowConnectors(ShipBuildManager.GetConnectors(_previousModule).ToList(), ShipBuildManager.Modules.Count == 0);
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

            foreach (ModuleComponent com in _newModule.Components)
                com.GameObject.transform.position = 
                    Camera.ScreenToWorldPoint(Input.mousePosition) + 
                    com.LocalPosition + new Vector3(0, 0, 10);

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
            {
                //Don't use module.blueprint.connectors, use the component's connectors!
                _newModule = ModuleVectorUtils.RotateModule(_newModule, ModuleVectorUtils.RotationDirection.CCW);
                _newModuleRotations--;
                ModuleInfoPanel.Initialize(_newModule.ModuleBlueprint.Name, _newModule.ModuleBlueprint.Connectors, _newModule.Components[_currentModuleComponent].LocalPosition, _newModuleRotations, _newModuleFlips);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                _newModule = ModuleVectorUtils.RotateModule(_newModule, ModuleVectorUtils.RotationDirection.CW);
                _newModuleRotations++;
                ModuleInfoPanel.Initialize(_newModule.ModuleBlueprint.Name, _newModule.ModuleBlueprint.Connectors, _newModule.Components[_currentModuleComponent].LocalPosition, _newModuleRotations, _newModuleFlips);
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
            {
                _newModule = ModuleVectorUtils.FlipModule(_newModule, ModuleVectorUtils.FlipDirection.Horizontal);
                _newModuleFlips[0] = MathHelper.Wrap(_newModuleFlips[0] + 1, 0, 1);
                ModuleInfoPanel.Initialize(_newModule.ModuleBlueprint.Name, _newModule.ModuleBlueprint.Connectors, _newModule.Components[_currentModuleComponent].LocalPosition, _newModuleRotations, _newModuleFlips);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                _newModule = ModuleVectorUtils.FlipModule(_newModule, ModuleVectorUtils.FlipDirection.Vertical);
                _newModuleFlips[1] = MathHelper.Wrap(_newModuleFlips[1] + 1, 0, 1);
                ModuleInfoPanel.Initialize(_newModule.ModuleBlueprint.Name, _newModule.ModuleBlueprint.Connectors, _newModule.Components[_currentModuleComponent].LocalPosition, _newModuleRotations, _newModuleFlips);
            }

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                var newModuleComponent = Input.GetKeyDown(KeyCode.Q) ? 
                    MathHelper.Wrap(_currentModuleComponent - 1, 0, _newModule.Components.Length - 1) :
                    MathHelper.Wrap(_currentModuleComponent + 1, 0, _newModule.Components.Length - 1);

                _newModule.Components[_currentModuleComponent].GameObject.GetComponent<SpriteRenderer>().color = Color.white;
                _newModule.Components[newModuleComponent].GameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                ModuleInfoPanel.Show(_newModule.Components[newModuleComponent].LocalPosition);

                _currentModuleComponent = newModuleComponent;
            }
        }

        /// <summary>
        /// Handles the moving of the build sprite to the UI BG layer, and notifies the 
        /// ShipBuildManager to add a new module to the ship
        /// </summary>
        private void PlaceModule()
        {
            if (_previousColor == new Color(0.6f, 0.14f, 0.14f))
                return;

            foreach (var com in _newModule.Components)
                com.GameObject.GetComponent<SpriteRenderer>().color = Color.white;

            ShipBuildManager.AddModule(_previousModule, _blueprint, _newModuleRotations, _newModuleFlips);
            Menu.ModulePlaced(_newModule.ModuleBlueprint);
            _previousModule = null;
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
            _blueprint = blueprint.Copy();
            _newModule = Module.Create(blueprint);
            _newModule = ModuleVectorUtils.RotateModule(_newModule, _newModuleRotations);
            _newModule = ModuleVectorUtils.FlipModule(_newModule, _newModuleFlips);

            for (var i = 0; i < _newModule.Components.Length; i++)
            {
                var com = _newModule.Components[i];
                var sprite = com.GameObject.GetComponent<SpriteRenderer>();
                sprite.sortingLayerName = "UI BG";
                sprite.sortingOrder = DeckManager.CurrentDeck;

                if (i != 0)
                    sprite.color = new Color(1, 1, 1, 0.5f);

                _placeMode = true;
            }

            _currentModuleComponent = 0;
            ModuleInfoPanel.Initialize(_newModule.ModuleBlueprint.Name, _newModule.ModuleBlueprint.Connectors, _newModule.Components[_currentModuleComponent].LocalPosition, _newModuleRotations, _newModuleFlips);
        }

        private void CancelPlaceMode()
        {
            foreach(var mod in _newModule.Components)
                Destroy(mod.GameObject);

            _placeMode = false;
            _newModule = null;
            _blueprint = null;

            ModuleInfoPanel.Disable();
        }

        #endregion
    }
}
