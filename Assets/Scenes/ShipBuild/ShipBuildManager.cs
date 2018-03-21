using System.Collections.Generic;
using System.Linq;
using Assets.Common.Utils;
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

    public List<Module> Modules
    {
        get;
        private set;
    }
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
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void AddModule(Module module)
    {
        if (Modules.Count == 0)
        {
            module.Position = new IntVector(0, 0, 1);
        }
        else
        {
            //Calculate position based off of distance from first module
        }

        Modules.Add(module);

        PowerUsed = Modules.Sum(x => x.ModuleBlueprint.PowerConumption);
        ControlUsed = Modules.Sum(x => x.ModuleBlueprint.CommandRequirement);
        PersonnelUsed = Modules.Sum(x => x.ModuleBlueprint.CrewRequirement);

        ControlAvailable = Modules.Where(x => x.ModuleBlueprint is CommandModuleBlueprints)
            .Sum(x => ((CommandModuleBlueprints)x.ModuleBlueprint).CommandSupplied);

        PersonnelAvailable = Modules.Where(x => x.ModuleBlueprint is CockpitModuleBlueprint)
            .Sum(x => ((CockpitModuleBlueprint)x.ModuleBlueprint).PersonnelHoused);

        if (!HasCommandModule && module.ModuleBlueprint is CommandModuleBlueprints)
        {
            HasCommandModule = true;

            foreach (var go in _firstLevelToggles)
            {
                go.interactable = true;

                if (module.ModuleBlueprint is CockpitModuleBlueprint && go.name == "Control Centers Toggle")
                    go.interactable = false;
            }
        }
    }
}
