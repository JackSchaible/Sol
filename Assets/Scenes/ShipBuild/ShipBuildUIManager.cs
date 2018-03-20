using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Data;
using Assets.Scenes.ShipBuild;
using Assets.Scenes.ShipBuild.UI.DetailsViews;
using Assets.Ships;
using Assets.Ships.Modules;
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
    private ModuleBlueprints[] _blueprints;
    private Dictionary<string, GameObject> _detailsViews;

    private ModuleBlueprints _selected;
    private List<GameObject> _activeObjects = new List<GameObject>();
    private bool _placeMode;
    private Module _newModule;
    private bool _placementValid;

    void Start()
    {
        _blueprintsManager = new ModuleBlueprintsManager();
        _blueprints = _blueprintsManager.Get();

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
            var stats = _blueprints.First(x => x.Name == toggleName);
            var view = _detailsViews[stats.ModuleSubtype];
            
            ConfigureModuleDetailsView(stats, view);
            SetDetailsViewActive(view, toggle.isOn);
            _selected = stats;
        }
    }

    void Update()
    {
        _placementValid = true;
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
            
        //Todo: replace with raycast when you can figure it out
        if (Manager.Modules.Any(
            x => x.GameObject.transform.position.x == _newModule.GameObject.transform.position.x &&
                 x.GameObject.transform.position.y == _newModule.GameObject.transform.position.y))
        {
            //Make newmodule sprite red, disable placement, handle for seperate decks
            _newModule.GameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            _placementValid = false;
        }
        else
            _newModule.GameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

        if (Input.GetKeyUp(KeyCode.Escape))
            CancelPlaceMode();

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

        if (Input.GetMouseButtonDown(0) && _placementValid)
        {
            _newModule.GameObject.GetComponent<SpriteRenderer>().sortingLayerName = "UI BG";

            if (Manager.Modules.Count == 0)
            {
                DeckManager.EnableNewDeckButtons(DeckManager.NewDeckButtons.Lower);
                DeckManager.EnableNewDeckButtons(DeckManager.NewDeckButtons.Upper);
            }

            Manager.AddModule(_newModule);

            if (_newModule.ModuleBlueprint is CockpitModuleBlueprints)
                ControlCentresToggle.isOn = false;

            if (_newModule.ModuleBlueprint.ExclusionVectors.Length > 0)
            {
                foreach (var vector in _newModule.ModuleBlueprint.ExclusionVectors)
                {
                    //Disable whatever it is
                    switch (vector)
                    {
                        case ExclusionVectors.ForwardLine:
                            break;

                        case ExclusionVectors.BackwardLine:
                            break;

                        case ExclusionVectors.UpwardLine:
                            break;

                        case ExclusionVectors.DownwardLine:
                            break;

                        case ExclusionVectors.RightLine:
                            break;

                        case ExclusionVectors.LeftLine:
                            break;

                        case ExclusionVectors.Plane:
                            DeckManager.DisableDeck(DeckManager.CurrentDeck);
                            break;

                        case ExclusionVectors.PlaneAndAbove:
                            DeckManager.DisableDeck(DeckManager.CurrentDeck);
                            DeckManager.DisableNewDeckButtons(DeckManager.NewDeckButtons.Upper);
                            DeckManager.AddLowerDeck();
                            break;

                        case ExclusionVectors.PlaneAndBelow:
                            DeckManager.DisableDeck(DeckManager.CurrentDeck);
                            DeckManager.DisableNewDeckButtons(DeckManager.NewDeckButtons.Lower);
                            DeckManager.AddUpperDeck();
                            break;

                        case ExclusionVectors.PlaneAndForward:
                            break;

                        case ExclusionVectors.PlaneAndBackward:
                            break;

                        case ExclusionVectors.PlaneAndRight:
                            break;

                        case ExclusionVectors.PlaneAndLeft:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            CancelPlaceMode();
        }
    }

    private void CancelPlaceMode()
    {
        _placeMode = false;

        foreach (var obj in _activeObjects)
            obj.SetActive(true);
    }

    private void ConfigureModuleDetailsView(ModuleBlueprints blueprints, GameObject view)
    {
        switch (blueprints.ModuleSubtype)
        {
            case ControlCentreTypes.SmallShip:
                BindSmallShipDetailsView((CommandModuleBlueprints)blueprints, view);
                break;
        }
    }

    private void BindSmallShipDetailsView(CommandModuleBlueprints blueprints, GameObject view)
    {
        var script = view.GetComponent<SmallShip>();

        script.Title.text = blueprints.Name;
        script.Image.sprite = GraphicsUtils.GetSpriteFromPath(blueprints.BuildSprite);
        script.Image.type = Image.Type.Simple;
        script.Image.preserveAspect = true;
        script.Description.text = blueprints.Description;
        script.Command.text = blueprints.CommandSupplied.ToString();
        script.Health.text = blueprints.Health.ToString();
        script.Crew.text = blueprints.CrewRequirement.ToString();
        script.Weight.text = blueprints.Weight.ToSiUnit("g");
        script.Power.text = blueprints.PowerConumption.ToSiUnit("W");
        script.Cost.text = blueprints.Cost.ToString();
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
        _activeObjects.Add(GameObject.FindGameObjectWithTag("Base Menu"));
        _activeObjects.AddRange(GameObject.FindGameObjectsWithTag("Submenu"));
        _activeObjects.AddRange(GameObject.FindGameObjectsWithTag("Details View"));

        foreach(var obj in _activeObjects)
            obj.SetActive(false);

        _placeMode = true;
        _newModule = Module.Create(_selected);
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
