using System.Linq;
using Assets.Common.Utils;
using Assets.Data;
using Assets.Scenes.ShipBuild;
using Assets.Scenes.ShipBuild.MenuManager;
using Assets.Ships;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuildUIManager : MonoBehaviour
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
    public GameObject ShipSizeSelect;

    //Internal state-tracking variables
    private bool _placeMode;
    private Module _newModule;
    private GameObject _previousModule;
    private float _newModuleRotation;

    #endregion

    #region Main methods

    void Start()
    {
        Modal.Initialize(Modals.BuildMenu.CommandModulesModalData);
        Modal.ShowModal();

        //TODO: Only run this if ship is new
        NewShipInitialization();
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

        var rh = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rh) && rh.transform.tag == "GridCell")
        {
            if (_previousModule != rh.transform.gameObject)
            {
                if (_previousModule != null)
                    _previousModule.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

                _previousModule = rh.transform.gameObject;
                _previousModule.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1);
            }
        }

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

        //if (_newModule.ModuleBlueprint.Space.Length > 1)
        //{
        //    if (_newModule.ModuleBlueprint.Space.Any(a => a.X != 0))
        //        _newModule.GameObject.transform.position = new Vector3(
        //            _newModule.GameObject.transform.position.x + 25,
        //            _newModule.GameObject.transform.position.y,
        //            1);

        //    if (_newModule.ModuleBlueprint.Space.Any(a => a.Y != 0))
        //        _newModule.GameObject.transform.position = new Vector3(
        //            _newModule.GameObject.transform.position.x,
        //            _newModule.GameObject.transform.position.y + 25,
        //            1);
        //}

        //_newModule.Position = IntVector.GetRelativeVector(_newModule.GameObject.transform.position);
        //_newModule.Position.SetZ(DeckManager.CurrentDeck);

        //Module pos has changed, recalculate the placement viability

        //if (ShipBuildManager.FirstModule != null && _newModule.Position != _previousPos)
        //    ShipBuildManager.IsPlacementValid(_newModule);

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //_newModule.GameObject.transform.RotateAround(_newModule.GameObject.transform.position,
            //    new Vector3(0, 0, 1), -90);

            //_newModule.ModuleBlueprint.Connectors =
            //    ModuleVectorUtils.RotateConnectorPositions(_newModule.ModuleBlueprint.Connectors,
            //        ModuleVectorUtils.RotationDirection.CW);

            //_newModule.ModuleBlueprint.ExclusionVectors =
            //    ModuleVectorUtils.RotateExclusionVectors(_newModule.ModuleBlueprint.ExclusionVectors, ModuleVectorUtils.RotationDirection.CW);

            //_newModule.ModuleBlueprint.Space =
            //    ModuleVectorUtils.RotateSpace(_newModule.ModuleBlueprint.Space,
            //        ModuleVectorUtils.RotationDirection.CW);

            _newModuleRotation -= 90;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
        {
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            //_newModule.GameObject.transform.localScale = new Vector3(
            //    _newModule.GameObject.transform.localScale.x * -1,
            //    _newModule.GameObject.transform.localScale.y,
            //    _newModule.GameObject.transform.localScale.z);
            //_newModule.ModuleBlueprint.ExclusionVectors =
            //    ModuleVectorUtils.FlipExclusionVectors(_newModule.ModuleBlueprint.ExclusionVectors, ModuleVectorUtils.FlipDirection.Vertical);
            //_newModule.ModuleBlueprint.Connectors =
            //    ModuleVectorUtils.FlipConnectorPositions(_newModule.ModuleBlueprint.Connectors, ModuleVectorUtils.FlipDirection.Vertical);
        }

        //if (ShipBuildManager.FirstModule != null)
        //    _previousPos = IntVector.GetRelativeVector(_newModule.GameObject.transform.position,
        //        ShipBuildManager.FirstModule.GameObject.transform.position).SetZ(DeckManager.CurrentDeck);

        //Don't call the IsPlacementValid function unless you have to, it's expensive to run
        //TODO: Maintain last call to IsPlacementValid's state
        if (Input.GetMouseButton(0))// && _newModule.GameObject.GetComponent<SpriteRenderer>().color == new Color(1, 1, 1))
            PlaceModule();

        
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

        //TODO: Why did we need this?
        //_newModule.GameObject.SetActive(false);
        //_newModule.GameObject.SetActive(true);

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

    #region Event Handlers

    public void OnShipTypeSelected(string shipType)
    {
        ShipSizeSelect.SetActive(false);
        ShipBuildManager.InitializeGrid(shipType);
    }

    #endregion
}
