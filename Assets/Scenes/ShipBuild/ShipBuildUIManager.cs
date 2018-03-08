using System.Collections.Generic;
using System.Linq;
using Assets.Ships;
using Assets.Utils;
using Assets.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuildUIManager : MonoBehaviour
{
    public Modal Modal;

    public ShipBuildManager Manager;

    public Text ModulesText;
    public Text PowerText;
    public Text PeopleText;

    public GameObject ControlCentresDetails;

    public Camera Camera;

    private ModuleBlueprintsManager _blueprintsManager;
    private ModuleBlueprints[] _blueprints;
    private Dictionary<string, GameObject> _detailsViews;

    private ModuleBlueprints _selected;
    private List<GameObject> _activeObjects = new List<GameObject>();
    private bool _placeMode;
    private Module _newModule;
    private bool _placementValid;

    private int _currentDeck = 0;

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

        Modal.Initialize(Modal.ModalTypes.Info, "Command Modules", "Command modules must be placed first, before any other module.");
        Modal.ShowModal();
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

        if (_placeMode)
        {
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
                CancelBuildMode();

            if (Input.GetMouseButtonDown(0) && _placementValid)
            {
                _newModule.GameObject.GetComponent<SpriteRenderer>().sortingLayerName = "UI BG";
                Manager.AddModule(_newModule);
                CancelBuildMode();
            }
        }
    }

    private void CancelBuildMode()
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
        var textComponents = view.GetComponentsInChildren<Text>();
        var imageComponents = view.GetComponentsInChildren<Image>();

        textComponents.First(x => x.name == "Name").text = blueprints.Name;
        imageComponents.First(x => x.name == "Image").sprite = GraphicsUtils.GetSpriteFromPath(blueprints.BuildSprite);
        textComponents.First(x => x.name == "Description").text = blueprints.Description;
        textComponents.First(x => x.name == "Command").text = blueprints.CommandSupplied.ToString();
        textComponents.First(x => x.name == "Health").text = blueprints.Health.ToString();
        textComponents.First(x => x.name == "Crew").text = blueprints.CrewRequirement.ToString();
        textComponents.First(x => x.name == "Weight").text = blueprints.Weight.ToSiUnit("g");
        textComponents.First(x => x.name == "Power").text = blueprints.PowerConumption.ToSiUnit("W");
        textComponents.First(x => x.name == "Cost").text = blueprints.Cost.ToString();
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

    public void ShowCommandModulePopup(Toggle t)
    {
        if (t.isOn)
        {
            Modal.Initialize(Modal.ModalTypes.Info, "Cockpit Modules",
                "You may only place one Small Ship module. If you select a different type of command module, you will be unable to place a Small Ship module.");
            Modal.ShowModal();
        }
    }
}
