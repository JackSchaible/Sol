using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Common.Utils;
using Assets.Scenes.ShipBuild;
using Assets.Ships;
using Assets.Ships.Modules;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuildManager : MonoBehaviour
{
    public DeckManager DeckManager; 

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

    private Module _firstModule;
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
            module.Position = new IntVector(0, 0, 0);
            _firstModule = module;
        }
        else
        {
            module.Position = GetRelativeVector(module.GameObject.transform.position,
                _firstModule.GameObject.transform.position);
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

    public bool IsPlacementValid(Module newModule)
    {
        var valid = true;
        var pos = IntVector.GetRelativeVector(newModule.GameObject.transform.position,
            _firstModule.GameObject.transform.position);

        #region If is overlapping

        if (!Modules.Any(
            x => x.GameObject.transform.position.x == newModule.GameObject.transform.position.x &&
                 x.GameObject.transform.position.y == newModule.GameObject.transform.position.y &&
                 x.Position.Z == DeckManager.CurrentDeck))
            return false;

        #endregion

        #region Exclusion Vectors

        foreach (var module in Modules)
        {
            //Exclusion Vector Stuff
            foreach (var ev in module.ModuleBlueprint.ExclusionVectors)
            {
                switch (ev)
                {
                    case ExclusionVectors.ForwardLine:
                        if (module.Position.X == pos.X &&
                            module.Position.Y == pos.Y &&
                            module.Position.Z < pos.Z)
                            valid = false;
                        break;
                    case ExclusionVectors.BackwardLine:
                        if (module.Position.X == pos.X &&
                            module.Position.Y == pos.Y &&
                            module.Position.Z > pos.Z)
                            valid = false;
                        break;
                    case ExclusionVectors.UpwardLine:
                        if (module.Position.X == pos.X &&
                            module.Position.Y < pos.Y &&
                            module.Position.Z == pos.Z)
                            valid = false;
                        break;
                    case ExclusionVectors.DownwardLine:
                        if (module.Position.X == pos.X &&
                            module.Position.Y > pos.Y &&
                            module.Position.Z == pos.Z)
                            valid = false;
                        break;
                    case ExclusionVectors.RightLine:
                        if (module.Position.X < pos.X &&
                            module.Position.Y == pos.Y &&
                            module.Position.Z == pos.Z)
                            valid = false;
                        break;
                    case ExclusionVectors.LeftLine:
                        if (module.Position.X > pos.X &&
                            module.Position.Y == pos.Y &&
                            module.Position.Z == pos.Z)
                            valid = false;
                        break;
                    case ExclusionVectors.Plane:
                        if (module.Position.Z == pos.Z)
                            valid = false;
                        break;
                    case ExclusionVectors.PlaneAndAbove:
                        if (module.Position.Y <= pos.Y)
                            valid = false;
                        break;
                    case ExclusionVectors.PlaneAndBelow:
                        if (module.Position.Y >= pos.Y)
                            valid = false;
                        break;
                    case ExclusionVectors.PlaneAndForward:
                        if (module.Position.Z <= pos.Z)
                            valid = false;
                        break;
                    case ExclusionVectors.PlaneAndBackward:
                        if (module.Position.Z >= pos.Z)
                            valid = false;
                        break;
                    case ExclusionVectors.PlaneAndRight:
                        if (module.Position.X <= pos.X)
                            valid = false;
                        break;
                    case ExclusionVectors.PlaneAndLeft:
                        if (module.Position.X >= pos.X)
                            valid = false;
                        break;
                }
            }
        }
        if (!valid) return false;

        #endregion

        #region Connectors & Exclusion Vectors

        foreach (var connector in newModule.ModuleBlueprint.Connectors)
        {
            switch (connector.Position)
            {
                case ConnectorPositions.Up:
                    if (!Modules.Any(x => x.Position.Y == pos.Y - 1 &&
                                          x.Position.X == pos.X &&
                                          x.Position.Z == pos.Z))
                        valid = false;
                    break;
                case ConnectorPositions.Right:
                    if (!Modules.Any(x => x.Position.Y == pos.Y &&
                                             x.Position.X == pos.X + 1 &&
                                             x.Position.Z == pos.Z))
                        valid = false;
                    break;
                case ConnectorPositions.Down:
                    if (!Modules.Any(x => x.Position.Y == pos.Y + 1 &&
                                             x.Position.X == pos.X &&
                                             x.Position.Z == pos.Z))
                        valid = false;
                    break;
                case ConnectorPositions.Left:
                    if (!Modules.Any(x => x.Position.Y == pos.Y &&
                                             x.Position.X == pos.X - 1 &&
                                             x.Position.Z == pos.Z))
                        valid = false;
                    break;
                case ConnectorPositions.Forward:
                    if (!Modules.Any(x => x.Position.Y == pos.Y &&
                                             x.Position.X == pos.X &&
                                             x.Position.Z == pos.Z + 1))
                        valid = false;
                    break;
                case ConnectorPositions.Backward:
                    if (!Modules.Any(x => x.Position.Y == pos.Y &&
                                             x.Position.X == pos.X &&
                                             x.Position.Z == pos.Z - 1))
                        valid = false;
                    break;
            }
        }
        if (!valid) return false;

        #endregion

        //TODO: Other invalid conditions?

        return valid;
    }
}
