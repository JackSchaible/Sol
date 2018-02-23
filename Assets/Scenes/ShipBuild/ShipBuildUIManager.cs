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

    public GameObject ControlCentresDetails;

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
            var toggleName = toggle.name.Replace(" Toggle", "");

        }
    }

    void Update()
    {
        ModulesText.text = Manager.ControlUsed + " / " + Manager.ControlAvailable;
        PowerText.text = Manager.PowerUsed + " / " + Manager.PersonnelAvailable;
        PeopleText.text = Manager.PersonnelUsed + " / " + Manager.PersonnelAvailable;
    }
}
