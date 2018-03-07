using System.Collections.Generic;
using System.Linq;
using Assets.Ships;
using Assets.Utils;
using Assets.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuildUIManager : MonoBehaviour
{
    public ShipBuildManager Manager;

    public Text ModulesText;
    public Text PowerText;
    public Text PeopleText;

    public Grid BuildGrid;

    public GameObject ControlCentresDetails;

    public Camera Camera;

    private ModuleStatsManager _statsManager;
    private ModuleStats[] _stats;
    private Dictionary<string, GameObject> _detailsViews;

    private ModuleStats _selected;
    private List<GameObject> _activeObjects = new List<GameObject>();
    private bool _placeMode;
    private Module _newModule;

    private int _currentDeck = 0;

    void Start()
    {
        _statsManager = new ModuleStatsManager();
        _stats = _statsManager.Get();

        _detailsViews = new Dictionary<string, GameObject>();
        foreach (var view in GameObject.FindGameObjectsWithTag("Details View"))
        {
            _detailsViews.Add(view.name.Replace(" Details Content", ""), view);
            SetDetailsViewActive(view, false);
        }
    }

    public void ModuleSelected(Toggle toggle)
    {
        if (toggle.isOn)
        {
            var toggleName = toggle.name.Replace(" Toggle", "");
            var stats = _stats.First(x => x.Name == toggleName);
            var view = _detailsViews[stats.ModuleSubtype];
            
            ConfigureModuleDetailsView(stats, view);
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
            if (Manager.Modules.Any(x => x.GameObject.transform.position.x == _newModule.GameObject.transform.position.x &&
                                         x.GameObject.transform.position.y == _newModule.GameObject.transform.position.y))
            {
                //Make newmodule sprite red, disable placement, handle for seperate decks
            }

            if (Input.GetKeyUp(KeyCode.Escape))
                CancelBuildMode();

            if (Input.GetMouseButtonDown(0))
            {
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


    private void ConfigureModuleDetailsView(ModuleStats stats, GameObject view)
    {
        switch (stats.ModuleSubtype)
        {
            case ControlCentreTypes.SmallShip:
                BindSmallShipDetailsView((CommandModuleStats)stats, view);
                break;
        }
    }

    private void BindSmallShipDetailsView(CommandModuleStats stats, GameObject view)
    {
        var textComponents = view.GetComponentsInChildren<Text>();
        var imageComponents = view.GetComponentsInChildren<Image>();

        textComponents.First(x => x.name == "Name").text = stats.Name;
        imageComponents.First(x => x.name == "Image").sprite = GraphicsUtils.GetSpriteFromPath(stats.BuildSprite);
        textComponents.First(x => x.name == "Description").text = stats.Description;
        textComponents.First(x => x.name == "Command").text = stats.CommandSupplied.ToString();
        textComponents.First(x => x.name == "Health").text = stats.Health.ToString();
        textComponents.First(x => x.name == "Crew").text = stats.CrewRequirement.ToString();
        textComponents.First(x => x.name == "Weight").text = stats.Weight.ToSiUnit("g");
        textComponents.First(x => x.name == "Power").text = stats.PowerConumption.ToSiUnit("W");
        textComponents.First(x => x.name == "Cost").text = stats.Cost.ToString();
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
}
