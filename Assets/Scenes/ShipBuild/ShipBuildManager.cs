using System.Collections.Generic;
using System.Linq;
using Assets.Ships;
using UnityEngine;

public class ShipBuildManager : MonoBehaviour
{
    public int PowerAvailable;
    public int PowerUsed;
    public int ControlAvailable;
    public int ControlUsed;
    public int PersonnelAvailable;
    public int PersonnelUsed;

    public List<Module> Modules = new List<Module>();

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();

        PowerUsed = Modules.Sum(x => x.ModuleStats.PowerConumption);
        ControlUsed = Modules.Sum(x => x.ModuleStats.CommandRequirement);
        PersonnelUsed = Modules.Sum(x => x.ModuleStats.CrewRequirement);

        ControlAvailable = Modules.Where(x => x.ModuleStats is CommandModuleStats)
            .Sum(x => ((CommandModuleStats) x.ModuleStats).CommandSupplied);
    }
}
