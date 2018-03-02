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
        PowerText.text = Manager.PowerUsed + " / " + Manager.PersonnelAvailable;
        PeopleText.text = Manager.PersonnelUsed + " / " + Manager.PersonnelAvailable;

        if (_placeMode)
        {
            //Constrain to n-px increments
            const int n = 50;
            _newModule.GameObject.transform.position = Camera.ScreenToWorldPoint(Input.mousePosition);
            _newModule.GameObject.transform.position = new Vector3(
                Mathf.Floor(_newModule.GameObject.transform.position.x / n) * n,
                Mathf.Floor(_newModule.GameObject.transform.position.y / n) * n,
                _currentDeck);

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                _placeMode = false;

                foreach (var obj in _activeObjects)
                    obj.SetActive(true);
                _newModule = null;
            }
        }
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

    private void SetDetailsViewActive(GameObject view, bool active)
    {
        view.transform.parent.parent.parent.gameObject.SetActive(active);
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
