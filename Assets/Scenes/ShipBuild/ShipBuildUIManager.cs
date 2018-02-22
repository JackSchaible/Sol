using Assets.Ships;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuildUIManager : MonoBehaviour
{
    public ShipBuildManager Manager;

    public Text ModulesText;
    public Text PowerText;
    public Text PeopleText;

    public Grid BuildGrid;

    private ModuleStatsManager _statsManager;
    private ModuleStats[] _stats;

    void Start()
    {
        _statsManager = new ModuleStatsManager();
        _stats = _statsManager.Get();
    }

    public void ModuleSelected(Toggle toggle)
    {
        if (toggle.isOn)
        {
            //TODO: get name of toggle, display values
        }
    }

    void Update()
    {
        return; //todo: reimplement, was lost somehow
        ModulesText.text = Manager.ControlUsed + " / " + Manager.ControlAvailable;
        PowerText.text = Manager.PowerUsed + " / " + Manager.PersonnelAvailable;
        PeopleText.text = Manager.PersonnelUsed + " / " + Manager.PersonnelAvailable;
    }
}
