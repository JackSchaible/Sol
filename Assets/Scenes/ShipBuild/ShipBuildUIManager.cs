using System.Collections.Generic;
using System.Linq;
using Assets.Common.Utils;
using Assets.Data;
using Assets.Scenes.ShipBuild;
using Assets.Ships;
using Assets.Utils;
using Assets.Utils.Extensions;
using Assets.Utils.ModuleUtils;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class ShipBuildUIManager : MonoBehaviour
{
    public Modal Modal;

    public ShipBuildManager Manager;

    public Text ModulesText;
    public Text PowerText;
    public Text PeopleText;

    public GameObject ControlCentresDetails;
    public Toggle ControlCentresToggle;

    public Camera Camera;

    public DeckManager DeckManager;

    private ModuleBlueprintsManager _blueprintsManager;
    private ModuleBlueprint[] _blueprint;
    private Dictionary<string, GameObject> _detailsViews;

    private ModuleBlueprint _selected;
    /// <summary>
    /// A list of the active toggles and menu game objects in the tree, used to hide the menus when placing a module
    /// </summary>
    private readonly List<GameObject> _activeObjects = new List<GameObject>();
    private bool _placeMode;
    private Module _newModule;
    private IntVector _previousPos;

    void Start()
    {
        _blueprintsManager = new ModuleBlueprintsManager();
        _blueprint = _blueprintsManager.Get();

        _detailsViews = new Dictionary<string, GameObject>();
        foreach (var view in GameObject.FindGameObjectsWithTag("Details View"))
        {
            _detailsViews.Add(view.name.Replace(" Details Content", ""), view);
            SetDetailsViewActive(view, false);
        }

        Modal.Initialize(Modals.BuildMenu.CommandModulesModalData);
        Modal.ShowModal();

        //TODO: Only run this if ship is new
        NewShipInitialization();
    }

    private void NewShipInitialization()
    {
        DeckManager.DisableNewDeckButtons(DeckManager.NewDeckButtons.Lower);
        DeckManager.DisableNewDeckButtons(DeckManager.NewDeckButtons.Upper);
    }

    public void ModuleSelected(Toggle toggle)
    {
        if (toggle.isOn)
        {
            var toggleName = toggle.name.Replace(" Toggle", "");
            var stats = _blueprint.First(x => x.Name == toggleName);
            var view = _detailsViews[stats.ModuleSubtype];
            
            SetDetailsViewActive(view, toggle.isOn);
            _selected = stats;
        }
    }

    void Update()
    {
        ModulesText.text = Manager.ControlUsed + " / " + Manager.ControlAvailable;
        PowerText.text = Manager.PowerUsed + " / " + Manager.PowerAvailable;
        PeopleText.text = Manager.PersonnelUsed + " / " + Manager.PersonnelAvailable;

        ModulesText.color = Manager.ControlUsed > Manager.ControlAvailable ?
            new Color(1, 0, 0) : new Color(1, 1, 1);

        PowerText.color = Manager.PowerUsed > Manager.PowerAvailable ?
            new Color(1, 0, 0) : new Color(1, 1, 1);

        PeopleText.color = Manager.PersonnelUsed > Manager.PersonnelAvailable ?
            new Color(1, 0, 0) : new Color(1, 1, 1);

        UpdatePlace();
    }

    #region Place Mode

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

        if (Manager.FirstModule != null && !_newModule.Position.Equals(_previousPos))
        {
            //Module pos has changed, recalculate the placement viability
            IsPlacementValid();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Destroy(_newModule.GameObject);
            CancelPlaceMode();
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

        if (Manager.FirstModule != null)
            _previousPos = IntVector.GetRelativeVector(_newModule.GameObject.transform.position,
                Manager.FirstModule.GameObject.transform.position);
    }

    private bool IsPlacementValid()
    {
        bool valid = Manager.IsPlacementValid(_newModule);

        _newModule.GameObject.GetComponent<SpriteRenderer>().color =
            valid ? new Color(1, 1, 1) : new Color(1, 0, 0);

        return valid;
    }

    private void PlaceModule()
    {
        _newModule.GameObject.GetComponent<SpriteRenderer>().sortingLayerName = "UI BG";
        Manager.AddModule(_newModule);
        CancelPlaceMode();
    }

    private void CancelPlaceMode()
    {
        _placeMode = false;

        foreach (var obj in _activeObjects)
            obj.SetActive(true);
    }

    private static void SetDetailsViewActive(GameObject view, bool active)
    {
        var p1 = view.transform.parent;
        if (p1 == null) return;
        var p2 = p1.parent;
        if (p2 == null) return;
        var p3 = p2.parent;
        if (p3 == null) return;
        p3.gameObject.SetActive(active);
    }

    public void Build()
    {
        if (Manager.FirstModule == null)
        {
            _activeObjects.Add(GameObject.FindGameObjectWithTag("Base Menu"));
            _activeObjects.AddRange(GameObject.FindGameObjectsWithTag("Submenu"));
            _activeObjects.Add(GameObject.FindGameObjectsWithTag("Details View").First().transform.parent.parent.parent.gameObject);

            foreach (var obj in _activeObjects)
                obj.SetActive(false);

            _newModule = Module.Create(_selected);
            _newModule.GameObject.transform.position = Vector3.zero;

            if (_newModule.ModuleBlueprint is CockpitModuleBlueprint)
                ControlCentresToggle.isOn = false;

            PlaceModule();
            CancelPlaceMode();
        }
        else
        {
            _activeObjects.Add(GameObject.FindGameObjectWithTag("Base Menu"));
            _activeObjects.AddRange(GameObject.FindGameObjectsWithTag("Submenu"));
            _activeObjects.Add(GameObject.FindGameObjectsWithTag("Details View").First().transform.parent.parent.parent.gameObject);

            foreach (var obj in _activeObjects)
                obj.SetActive(false);

            _placeMode = true;
            _newModule = Module.Create(_selected);
        }
    }

    #endregion

    #region Modals
    public void ShowCommandModulePopup(Toggle t)
    {
        if (t.isOn)
        {
            Modal.Initialize(Modals.BuildMenu.CommandModulesModalData);
            Modal.ShowModal();
        }
    }

    public void ShowCockpitModulePopup(Toggle t)
    {
        if (t.isOn)
        {
            Modal.Initialize(Modals.BuildMenu.CockpitModalData);
            Modal.ShowModal();
        }
    }

    #endregion
}
