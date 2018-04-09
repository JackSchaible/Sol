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

    private List<Slot> _availableSlots;

    public List<Module> Modules
    {
        get;
        private set;
    }
    public bool HasCommandModule { get; private set; }
    public Module FirstModule { get; private set; }

    void Awake()
    {
        Modules = new List<Module>();
        HasCommandModule = false;
        _availableSlots = new List<Slot>
        {
            new Slot(IntVector.Forward, new List<Connector>())
        };
    }

    void Start()
    {

    }

    void Update()
    {}

    public void AddModule(Module module)
    {
        if (Modules.Count == 0)
        {
            module.Position = new IntVector(0, 0, 1);
            FirstModule = module;

            DeckManager.EnableNewDeckButtons(DeckManager.NewDeckButtons.Lower);
            DeckManager.EnableNewDeckButtons(DeckManager.NewDeckButtons.Upper);
        }
        else
        {
            module.Position = IntVector.GetRelativeVector(FirstModule.GameObject.transform.position, module.GameObject.transform.position)
                .SetZ(DeckManager.CurrentDeck);
        }

        var slotsToRemove =
            _availableSlots.Where(x => module.ModuleBlueprint.Space.Any(y => (y + module.Position).Equals(x.Position))).ToList();

        foreach(var remove in slotsToRemove)
            _availableSlots.Remove(remove);

        Modules.Add(module);

        PowerUsed = Modules.Sum(x => x.ModuleBlueprint.PowerConumption);
        ControlUsed = Modules.Sum(x => x.ModuleBlueprint.CommandRequirement);
        PersonnelUsed = Modules.Sum(x => x.ModuleBlueprint.CrewRequirement);

        ControlAvailable = Modules.Where(x => x.ModuleBlueprint is CommandModuleBlueprint)
            .Sum(x => ((CommandModuleBlueprint)x.ModuleBlueprint).CommandSupplied);

        PersonnelAvailable = Modules.Where(x => x.ModuleBlueprint is CockpitModuleBlueprint)
            .Sum(x => ((CockpitModuleBlueprint)x.ModuleBlueprint).PersonnelHoused);

        if (!HasCommandModule && module.ModuleBlueprint is CommandModuleBlueprint)
            HasCommandModule = true;

        //Disable decks if module has an x/y plane or space exclusion vector
        if (module.ModuleBlueprint.ExclusionVectors.Length > 0)
        {
            foreach (var vectors in module.ModuleBlueprint.ExclusionVectors)
            {
                foreach (var vector in vectors.Direction)
                {
                    //Disable whatever it is
                    switch (vector)
                    {
                        case ExclusionVectorDirections.Plane:
                            DeckManager.DisableDeck(DeckManager.CurrentDeck);
                            break;

                        case ExclusionVectorDirections.PlaneAndForward:
                            DeckManager.DisableDeck(DeckManager.CurrentDeck);
                            DeckManager.DisableNewDeckButtons(DeckManager.NewDeckButtons.Upper);
                            DeckManager.AddLowerDeck();
                            break;

                        case ExclusionVectorDirections.PlaneAndBackward:
                            DeckManager.DisableDeck(DeckManager.CurrentDeck);
                            DeckManager.DisableNewDeckButtons(DeckManager.NewDeckButtons.Lower);
                            DeckManager.AddUpperDeck();
                            break;
                    }
                }
            }
        }

        AddConnectedSlots(module);
    }
    private void AddConnectedSlots(Module module)
    {
        foreach (var connector in module.ModuleBlueprint.Connectors)
        {
            var pos = module.Position + connector.Position;
            var newPos = pos.Adjust(connector.Direction);

            if (!IsPositionOutsideOfExclusionSpaces(newPos))
                continue;

            if (DoesModuleOverlap(newPos))
                continue;

            ConnectorPositions newD = ConnectorPosition.GetOpposite(connector.Direction);
            var existingSlot = _availableSlots.FirstOrDefault(x => x.Position.Equals(newPos));

            if (existingSlot == null)
                _availableSlots.Add(new Slot(newPos,
                    new List<Connector> { new Connector(newD, connector.CanConveyAtmosphere) }));
            else
                existingSlot.RequiredConnector.Add(new Connector(newD, connector.CanConveyAtmosphere));
        }
    }

    #region Rules

    public bool IsPlacementValid(Module newModule)
    {
        var hasConnector = false;
        var spaceValid = true;

        foreach (var slot in _availableSlots)
            foreach (var con in slot.RequiredConnector)
                foreach (var modCon in newModule.ModuleBlueprint.Connectors)
                    if (con.Position == modCon.Direction && slot.Position.Equals(modCon.Position + newModule.Position))
                    {
                        if ((con.SupportsAtmosphere && modCon.CanConveyAtmosphere) || 
                            (!con.SupportsAtmosphere && !modCon.CanConveyAtmosphere))
                            hasConnector = true;
                    }

        foreach (var space in newModule.ModuleBlueprint.Space)
        {
            var spacePos = newModule.Position + space;

            if (IsPositionOutsideOfExclusionSpaces(spacePos))
            {
                foreach (var module in Modules)
                foreach (var mSpace in module.ModuleBlueprint.Space)
                    if ((module.Position + mSpace).Equals(spacePos))
                        spaceValid = false;
            }
            else
                spaceValid = false;

        }

        return hasConnector && spaceValid;
    }

    #region 1. Exclusion Vectors and Connectors

    //1.a. Modules may not be placed in the space defined by the exclusion vector of other modules
    private bool IsPositionOutsideOfExclusionSpaces(IntVector pos)
    {
        bool valid = true;

        foreach (var module in Modules)
        {
            //Exclusion Vector Stuff
            foreach (var ev in module.ModuleBlueprint.ExclusionVectors)
            {
                foreach (var e in ev.Direction)
                {
                    var ePos = module.Position + ev.Position;
                    switch (e)
                    {
                        case ExclusionVectorDirections.ForwardLine:
                            if (ePos.X == pos.X &&
                                ePos.Y == pos.Y &&
                                ePos.Z < pos.Z)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.BackwardLine:
                            if (ePos.X == pos.X &&
                                ePos.Y == pos.Y &&
                                ePos.Z > pos.Z)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.UpwardLine:
                            if (ePos.X == pos.X &&
                                ePos.Y < pos.Y &&
                                ePos.Z == pos.Z)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.DownwardLine:
                            if (ePos.X == pos.X &&
                                ePos.Y > pos.Y &&
                                ePos.Z == pos.Z)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.RightLine:
                            if (ePos.X < pos.X &&
                                ePos.Y == pos.Y &&
                                ePos.Z == pos.Z)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.LeftLine:
                            if (ePos.X > pos.X &&
                                ePos.Y == pos.Y &&
                                ePos.Z == pos.Z)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.Plane:
                            if (ePos.Z == pos.Z)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.PlaneAndAbove:
                            if (ePos.Y <= pos.Y)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.PlaneAndBelow:
                            if (ePos.Y >= pos.Y)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.PlaneAndForward:
                            if (ePos.Z <= pos.Z)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.PlaneAndBackward:
                            if (ePos.Z >= pos.Z)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.PlaneAndRight:
                            if (ePos.X <= pos.X)
                                valid = false;
                            break;
                        case ExclusionVectorDirections.PlaneAndLeft:
                            if (ePos.X >= pos.X)
                                valid = false;
                            break;
                    }
                }
            }
        }

        return valid;
    }
    //1.c A module may not exist in the same space as another module
    private bool DoesModuleOverlap(IntVector pos)
    {
        return Modules.SelectMany(a => a.ModuleBlueprint.Space.Select(b => a.Position + b)).Any(c => c.Equals(pos));
    }

    #endregion

    #endregion

    private class Slot
    {
        public IntVector Position { get; set; }
        public List<Connector> RequiredConnector { get; set; }

        public Slot()
        {

        }

        public Slot(IntVector position, List<Connector> requiredConnector) : this()
        {
            Position = position;
            RequiredConnector = requiredConnector;
        }
    }

    private struct Connector
    {
        public ConnectorPositions Position;
        public bool SupportsAtmosphere;

        public Connector(ConnectorPositions position, bool supportsAtmosphere)
        {
            Position = position;
            SupportsAtmosphere = supportsAtmosphere;
        }
    }
}
