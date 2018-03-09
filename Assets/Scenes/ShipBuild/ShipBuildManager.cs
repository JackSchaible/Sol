using System.Collections.Generic;
using System.Linq;
using Assets.Ships;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuildManager : MonoBehaviour
{
    public int PowerAvailable;
    public int PowerUsed;
    public int ControlAvailable;
    public int ControlUsed;
    public int PersonnelAvailable;
    public int PersonnelUsed;

    public List<Module> Modules { get; private set; }
    public bool HasCommandModule { get; private set; }

    private List<Toggle> _firstLevelToggles;

    void Start()
    {
        Modules = new List<Module>();
        HasCommandModule = false;

        _firstLevelToggles = new List<Toggle>();
        var gos = GameObject.FindGameObjectsWithTag("First-Level Menu Button");
        foreach (var go in gos)
        {
            var toggle = go.transform.gameObject.GetComponentInChildren<Toggle>();
            if (go.name != "Control Centers Toggle")
                toggle.interactable = false;

            _firstLevelToggles.Add(toggle);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
    }

    public void AddModule(Module module)
    {
        Modules.Add(module);

        PowerUsed = Modules.Sum(x => x.ModuleBlueprint.PowerConumption);
        ControlUsed = Modules.Sum(x => x.ModuleBlueprint.CommandRequirement);
        PersonnelUsed = Modules.Sum(x => x.ModuleBlueprint.CrewRequirement);

        ControlAvailable = Modules.Where(x => x.ModuleBlueprint is CommandModuleBlueprints)
            .Sum(x => ((CommandModuleBlueprints)x.ModuleBlueprint).CommandSupplied);

        PersonnelAvailable = Modules.Where(x => x.ModuleBlueprint is CockpitModuleBlueprints)
            .Sum(x => ((CockpitModuleBlueprints)x.ModuleBlueprint).PersonnelHoused);

        if (!HasCommandModule && module.ModuleBlueprint is CommandModuleBlueprints)
        {
            HasCommandModule = true;

            foreach (var go in _firstLevelToggles)
            {
                go.interactable = true;

                if (module.ModuleBlueprint is CockpitModuleBlueprints && go.name == "Control Centers Toggle")
                    go.interactable = false;
            }
        }
    }
}
