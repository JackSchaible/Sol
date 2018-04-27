using System.Linq;
using Assets.Common.Utils;
using Assets.Data;
using Assets.Scenes.ShipBuild;
using Assets.Scenes.ShipBuild.MenuManager;
using Assets.Ships;
using Assets.Utils.ModuleUtils;
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

    //Internal state-tracking variables
    private bool _placeMode;
    private Module _newModule;
    private IntVector _previousPos;
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

        UpdatePlace();

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            var pos = Camera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, -100f);
            Ray r = new Ray(pos, Vector3.forward);
            RaycastHit rh;
            if (Physics.Raycast(r, out rh))
            {
                var obj = ShipBuildManager.Modules.FirstOrDefault(x => x.GameObject == rh.transform.gameObject);

                if (obj != null)
                    ShipBuildManager.DeleteModule(obj);
            }
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
        DeckManager.DisableNewDeckButtons(DeckManager.NewDeckButtons.Lower);
        DeckManager.DisableNewDeckButtons(DeckManager.NewDeckButtons.Upper);
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

        //Constrain to n-px increments
        const int n = 50;
        _newModule.GameObject.transform.position = Camera.ScreenToWorldPoint(Input.mousePosition);

        _newModule.GameObject.transform.position = new Vector3(
            Mathf.Floor(_newModule.GameObject.transform.position.x / n) * n,
            Mathf.Floor(_newModule.GameObject.transform.position.y / n) * n,
            1);

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

        _newModule.Position = IntVector.GetRelativeVector(_newModule.GameObject.transform.position);
        _newModule.Position.SetZ(DeckManager.CurrentDeck);

        //Module pos has changed, recalculate the placement viability
        if (ShipBuildManager.FirstModule != null && _newModule.Position != _previousPos)
            IsPlacementValid();

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            _newModule.GameObject.transform.RotateAround(_newModule.GameObject.transform.position,
                new Vector3(0, 0, 1), -90);

            _newModule.ModuleBlueprint.Connectors =
                ModuleVectorUtils.RotateConnectorPositions(_newModule.ModuleBlueprint.Connectors,
                    ModuleVectorUtils.RotationDirection.CW);

            _newModule.ModuleBlueprint.ExclusionVectors =
                ModuleVectorUtils.RotateExclusionVectors(_newModule.ModuleBlueprint.ExclusionVectors, ModuleVectorUtils.RotationDirection.CW);

            _newModule.ModuleBlueprint.Space =
                ModuleVectorUtils.RotateSpace(_newModule.ModuleBlueprint.Space,
                    ModuleVectorUtils.RotationDirection.CW);

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

        if (ShipBuildManager.FirstModule != null)
            _previousPos = IntVector.GetRelativeVector(_newModule.GameObject.transform.position,
                ShipBuildManager.FirstModule.GameObject.transform.position).SetZ(DeckManager.CurrentDeck);

        //Don't call the IsPlacementValid function unless you have to, it's expensive to run
        if (Input.GetMouseButton(0) && _newModule.GameObject.GetComponent<SpriteRenderer>().color == new Color(1, 1, 1))
            PlaceModule();

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButton(1))
        {
            if (_placeMode)
            {
                Destroy(_newModule.GameObject);
                CancelPlaceMode();
                Menu.PlaceCancelled();
            }
            else
                Application.Quit();
        }

        var collider = _newModule.GameObject.GetComponent<BoxCollider>();

        DebugText.text = string.Format("x1: {0} y1: {1} x2: {2} y2: {3}", collider.bounds.min.x, collider.bounds.min.y, collider.bounds.max.x, collider.bounds.max.y);
        //this.DebugText.text = _newModule.GameObject.GetComponent<BoxCollider>().bounds.center.ToString();
        //_newModule.GameObject.transform.RotateAround(_newModule.GameObject.GetComponent<BoxCollider>().bounds.center, Vector3.back, 1f);
    }

    /// <summary>
    /// Updates the color of the module and disables placement if the ShipBuildManager says the
    /// placement isn't valid
    /// </summary>
    /// <returns></returns>
    private bool IsPlacementValid()
    {
        bool valid = ShipBuildManager.IsPlacementValid(_newModule);

        _newModule.GameObject.GetComponent<SpriteRenderer>().color =
            valid ? new Color(1, 1, 1) : new Color(1, 0, 0);

        return valid;
    }

    /// <summary>
    /// Handles the moving of the build sprite to the UI BG layer, and notifies the 
    /// ShipBuildManager to add a new module to the ship
    /// </summary>
    private void PlaceModule()
    {
        ShipBuildManager.AddModule(_newModule);
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
        _newModule.GameObject.SetActive(false);
        _newModule.GameObject.SetActive(true);

        //if (_newModuleRotation != 0)
        //{
        //    _newModule.GameObject.transform.RotateAround(_newModule.GameObject.transform.position, new Vector3(0, 0, 1),
        //        _newModuleRotation);

        //    for(int rotations = (int)Math.Abs(_newModuleRotation) / 90; rotations > 0; rotations--)
        //        _newModule.ModuleBlueprint.Connectors =
        //            ModuleVectorUtils.RotateConnectorPositions(_newModule.ModuleBlueprint.Connectors,
        //                ModuleVectorUtils.RotationDirection.CCW);
        //}

        var sprite = _newModule.GameObject.GetComponent<SpriteRenderer>();
        sprite.sortingLayerName = "UI BG";
        sprite.sortingOrder = DeckManager.CurrentDeck;

        if (ShipBuildManager.FirstModule == null)
        {
            //Autoplace first modules at {0, 0}
            _newModule.GameObject.transform.position = Vector3.zero;
            PlaceModule();
        }
        else
            _placeMode = true;
    }

    private void CancelPlaceMode()
    {
        _placeMode = false;
        _newModule = null;
    }

    #endregion
}
