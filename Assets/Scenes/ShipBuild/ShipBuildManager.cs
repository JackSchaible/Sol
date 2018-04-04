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
    public Module FirstModule { get; private set; }

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
            FirstModule = module;

            DeckManager.EnableNewDeckButtons(DeckManager.NewDeckButtons.Lower);
            DeckManager.EnableNewDeckButtons(DeckManager.NewDeckButtons.Upper);
        }
        else
        {
            module.Position = IntVector.GetRelativeVector(module.GameObject.transform.position,
                FirstModule.GameObject.transform.position);
        }

        Modules.Add(module);

        PowerUsed = Modules.Sum(x => x.ModuleBlueprint.PowerConumption);
        ControlUsed = Modules.Sum(x => x.ModuleBlueprint.CommandRequirement);
        PersonnelUsed = Modules.Sum(x => x.ModuleBlueprint.CrewRequirement);

        ControlAvailable = Modules.Where(x => x.ModuleBlueprint is CommandModuleBlueprint)
            .Sum(x => ((CommandModuleBlueprint)x.ModuleBlueprint).CommandSupplied);

        PersonnelAvailable = Modules.Where(x => x.ModuleBlueprint is CockpitModuleBlueprint)
            .Sum(x => ((CockpitModuleBlueprint)x.ModuleBlueprint).PersonnelHoused);

        if (!HasCommandModule && module.ModuleBlueprint is CommandModuleBlueprint)
        {
            HasCommandModule = true;

            foreach (var go in _firstLevelToggles)
            {
                go.interactable = true;

                if (module.ModuleBlueprint is CockpitModuleBlueprint && go.name == "Control Centers Toggle")
                    go.interactable = false;
            }
        }

        //Disable decks if module has an x/y plane or space exclusion vector
        if (module.ModuleBlueprint.ExclusionVectors.Length > 0)
        {
            foreach (var vector in module.ModuleBlueprint.ExclusionVectors)
            {
                //Disable whatever it is
                switch (vector)
                {
                    case ExclusionVectors.Plane:
                        DeckManager.DisableDeck(DeckManager.CurrentDeck);
                        break;

                    case ExclusionVectors.PlaneAndAbove:
                        DeckManager.DisableDeck(DeckManager.CurrentDeck);
                        DeckManager.DisableNewDeckButtons(DeckManager.NewDeckButtons.Upper);
                        DeckManager.AddLowerDeck();
                        break;

                    case ExclusionVectors.PlaneAndBelow:
                        DeckManager.DisableDeck(DeckManager.CurrentDeck);
                        DeckManager.DisableNewDeckButtons(DeckManager.NewDeckButtons.Lower);
                        DeckManager.AddUpperDeck();
                        break;

                    default:
                        break;
                }
            }
        }

    }

    #region Rules

    public bool IsPlacementValid(Module newModule)
    {
        if (!Modules.Any()) return true;

        var valid = true;
        var pos = newModule.Position;

        if (valid)
            valid = DoesModuleOverlap(newModule);

        if (valid)
            valid = DoesModuleExistInExclusionSpace(pos);

        if (valid)
            valid = DoesModuleConnect(newModule, pos);

        return valid;
    }

    #region 1. Exclusion Vectors and Connectors
    
    //1.a. Modules may not be placed in the space defined by the exclusion vector of other modules
    private bool DoesModuleExistInExclusionSpace(IntVector pos)
    {
        bool valid = true;

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

        return valid;
    }
    //1.b. A new module must connect to a previously-placed module where the connector of the new module matches the location of any existing module
    private bool DoesModuleConnect(Module newModule, IntVector pos)
    {
        bool valid = true;

        foreach (var connector in newModule.ModuleBlueprint.Connectors)
        {
            switch (connector.Direction)
            {
                case ConnectorPositions.Up:
                    if (!Modules.Any(x =>
                    {
                        var conPos = x.Position + connector.Position.SetZ(DeckManager.CurrentDeck);

                        return
                            conPos.X == pos.X &&
                            conPos.Y == pos.Y - 1 &&
                            conPos.Z == pos.Z;
                    }))
                        valid = false;
                    break;
                case ConnectorPositions.Right:
                    if (!Modules.Any(x =>
                    {
                        var conPos = x.Position + connector.Position.SetZ(DeckManager.CurrentDeck);

                        return
                            conPos.X == pos.X + 1 &&
                            conPos.Y == pos.Y &&
                            conPos.Z == pos.Z;
                    }))
                        valid = false;
                    break;
                case ConnectorPositions.Down:
                    if (!Modules.Any(x =>
                    {
                        var conPos = x.Position + connector.Position.SetZ(DeckManager.CurrentDeck);

                        return
                            conPos.X == pos.X &&
                            conPos.Y == pos.Y + 1 &&
                            conPos.Z == pos.Z;
                    }))
                        valid = false;
                    break;
                case ConnectorPositions.Left:
                    if (!Modules.Any(x =>
                    {
                        var conPos = x.Position + connector.Position.SetZ(DeckManager.CurrentDeck);

                        return
                            conPos.X == pos.X - 1 &&
                            conPos.Y == pos.Y &&
                            conPos.Z == pos.Z;
                    }))
                        valid = false;
                    break;
                case ConnectorPositions.Forward:
                    if (!Modules.Any(x =>
                    {
                        var conPos = x.Position + connector.Position.SetZ(DeckManager.CurrentDeck);

                        return
                            conPos.X == pos.X &&
                            conPos.Y == pos.Y &&
                            conPos.Z == pos.Z + 1;
                    }))
                        valid = false;
                    break;
                case ConnectorPositions.Backward:
                    if (!Modules.Any(x =>
                    {
                        var conPos = x.Position + connector.Position.SetZ(DeckManager.CurrentDeck);

                        return
                            conPos.X == pos.X &&
                            conPos.Y == pos.Y &&
                            conPos.Z == pos.Z - 1;
                    }))
                        valid = false;
                    break;
            }
        }

        return valid;
    }
    //A module may not exist in the same space as another module
    private bool DoesModuleOverlap(Module newModule)
    {
        //TODO: Replace with raycast
        return !Modules.Any(
            x => x.GameObject.transform.position.x == newModule.GameObject.transform.position.x &&
                 x.GameObject.transform.position.y == newModule.GameObject.transform.position.y &&
                 x.Position.Z == DeckManager.CurrentDeck);
    }

    #endregion

    #endregion
}
