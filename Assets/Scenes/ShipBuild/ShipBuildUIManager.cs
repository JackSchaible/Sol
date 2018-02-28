using System.Collections.Generic;
using System.Linq;
using Assets.Ships;
using Assets.Utils;
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

    private ModuleStatsManager _statsManager;
    private ModuleStats[] _stats;
    private Dictionary<string, GameObject> _detailsViews;

    void Start()
    {
        _statsManager = new ModuleStatsManager();
        _stats = _statsManager.Get();

        _detailsViews = new Dictionary<string, GameObject>();
        foreach (var view in GameObject.FindGameObjectsWithTag("Details View"))
            _detailsViews.Add(view.name.Replace(" Details Content", ""), view);
    }

    public void ModuleSelected(Toggle toggle)
    {
        if (toggle.isOn)
        {
            var toggleName = toggle.name.Replace(" Toggle", "");
            var stats = _stats.First(x => x.Name == toggleName);
            var view = _detailsViews[stats.ModuleSubtype];
            
            ConfigureModuleDetailsView(stats, view);
        }
    }

    void Update()
    {
        ModulesText.text = Manager.ControlUsed + " / " + Manager.ControlAvailable;
        PowerText.text = Manager.PowerUsed + " / " + Manager.PersonnelAvailable;
        PeopleText.text = Manager.PersonnelUsed + " / " + Manager.PersonnelAvailable;
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
        view.GetComponentInChildren<Text>().text = stats.Name;
        view.GetComponentInChildren<Image>().sprite = GraphicsUtils.GetSpriteFromPath(stats.BuildSprite);
    }
}
