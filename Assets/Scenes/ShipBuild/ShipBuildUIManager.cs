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

    //UI Elements needing to be updates
    public Text ModulesText;
    public Text PowerText;
    public Text PeopleText;
    public Modal Modal;

    //Internal state-tracking variables
    private bool _placeMode;
    private Module _newModule;
    private IntVector _previousPos;
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

        UpdatePlace();
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

        _newModule.Position = IntVector.GetRelativeVector(_newModule.GameObject.transform.position);

        //Module pos has changed, recalculate the placement viability
        if (ShipBuildManager.FirstModule != null && !_newModule.Position.Equals(_previousPos))
            IsPlacementValid();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Destroy(_newModule.GameObject);
            _placeMode = false;
            Menu.PlaceCancelled();
        }

        #region Rotate/Flip Controls

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            _newModule.GameObject.transform.RotateAround(_newModule.GameObject.transform.position, new Vector3(0, 0, 1), 90);
            _newModule.ModuleBlueprint.ExclusionVectors =
                ModuleVectorUtils.RotateExclusionVectors(_newModule.ModuleBlueprint.ExclusionVectors, ModuleVectorUtils.RotationDirection.CW);
            _newModule.ModuleBlueprint.Connectors =
                ModuleVectorUtils.RotateConnectorPositions(_newModule.ModuleBlueprint.Connectors, ModuleVectorUtils.RotationDirection.CW);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            _newModule.GameObject.transform.RotateAround(_newModule.GameObject.transform.position, new Vector3(0, 0, 1), -90);
            _newModule.ModuleBlueprint.ExclusionVectors =
                ModuleVectorUtils.RotateExclusionVectors(_newModule.ModuleBlueprint.ExclusionVectors, ModuleVectorUtils.RotationDirection.CCW);
            _newModule.ModuleBlueprint.Connectors =
                ModuleVectorUtils.RotateConnectorPositions(_newModule.ModuleBlueprint.Connectors, ModuleVectorUtils.RotationDirection.CCW);
        } else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
        {
            _newModule.GameObject.transform.localScale = new Vector3(
                _newModule.GameObject.transform.localScale.x,
                _newModule.GameObject.transform.localScale.y * -1,
                _newModule.GameObject.transform.localScale.z);
            _newModule.ModuleBlueprint.ExclusionVectors =
                ModuleVectorUtils.FlipExclusionVectors(_newModule.ModuleBlueprint.ExclusionVectors, ModuleVectorUtils.FlipDirection.Horizontal);
            _newModule.ModuleBlueprint.Connectors =
                ModuleVectorUtils.FlipConnectorPositions(_newModule.ModuleBlueprint.Connectors, ModuleVectorUtils.FlipDirection.Horizontal);
        } else if (Input.GetKeyDown(KeyCode.T))
        {
            _newModule.GameObject.transform.localScale = new Vector3(
                _newModule.GameObject.transform.localScale.x * -1,
                _newModule.GameObject.transform.localScale.y,
                _newModule.GameObject.transform.localScale.z);
            _newModule.ModuleBlueprint.ExclusionVectors =
                ModuleVectorUtils.FlipExclusionVectors(_newModule.ModuleBlueprint.ExclusionVectors, ModuleVectorUtils.FlipDirection.Vertical);
            _newModule.ModuleBlueprint.Connectors =
                ModuleVectorUtils.FlipConnectorPositions(_newModule.ModuleBlueprint.Connectors, ModuleVectorUtils.FlipDirection.Vertical);
        }

        #endregion

        if (Input.GetMouseButtonDown(0) && IsPlacementValid())
            PlaceModule();

        if (ShipBuildManager.FirstModule != null)
            _previousPos = IntVector.GetRelativeVector(_newModule.GameObject.transform.position,
                ShipBuildManager.FirstModule.GameObject.transform.position);
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
        _placeMode = false;
        Menu.ModulePlaced(_newModule.ModuleBlueprint);
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

    #endregion
}
