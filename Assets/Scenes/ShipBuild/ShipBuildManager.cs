using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Common.Utils;
using Assets.Data;
using Assets.Scenes.ShipBuild;
using Assets.Ships;
using Assets.Ships.Modules;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuildManager : MonoBehaviour
{
    public DeckManager DeckManager;

    public float PowerAvailable;
    public float PowerUsed;
    public int ControlAvailable;
    public int ControlUsed;
    public int PersonnelAvailable;
    public int PersonnelUsed;

    public GameObject ComponentPrefab;
    public ModuleComponent[,,] Grid { get; set; }

    public List<Module> Modules
    {
        get;
        private set;
    }
    public bool HasCommandModule { get; private set; }
    public Module FirstModule { get; private set; }

    private readonly Dictionary<string, Vector3Int> _shipSizes = new Dictionary<string, Vector3Int>
    {
        {ShipTypes.StrikeCraft, new Vector3Int(10, 10, 3)},
        {ShipTypes.Frigate, new Vector3Int(25, 25, 10)},
        {ShipTypes.Cruiser, new Vector3Int(40, 40, 20)},
        {ShipTypes.CapitalShip, new Vector3Int(80, 80, 40)},
    };

    void Awake()
    {
        Modules = new List<Module>();
        HasCommandModule = false;
    }

    void Start()
    {

    }

    void Update()
    {
    }

    public void InitializeGrid(string shipType)
    {
        var size = _shipSizes[shipType];

        Grid = new ModuleComponent[size.x, size.y, size.z];

        for (var z = 0; z < size.z; z++)
        {
            if (z > 0)
                DeckManager.AddLowerDeck();

            for (var x = 0; x < size.x; x++)
                for (var y = 0; y < size.y; y++)
                {
                    Grid[x, y, z] = new ModuleComponent(Vector3Int.zero, new Connector[0], new ExclusionVector[0]);
                    Grid[x, y, z].GameObject = Instantiate(ComponentPrefab);
                    Grid[x, y, z].GameObject.transform.position = new Vector3Int(x, y, z);
                    Grid[x, y, z].GameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
        }

        DeckManager.SelectDeck(0);
    }

    public void AddModule(Module module)
    {
        if (Modules.Count == 0)
        {
            module.Position = new Vector3Int(0, 0, 1);
            FirstModule = module;
        }
        else
        {
            //TODO: Redo, maybe?
            //module.Position = IntVector.GetRelativeVector(FirstModule.GameObject.transform.position, module.GameObject.transform.position)
            //    .SetZ(DeckManager.CurrentDeck);
        }

        //var slotsToRemove =
        //    _availableSlots.Where(x => module.ModuleBlueprint.Space.Any(y => (y + module.Position).Equals(x.Position))).ToList();

        //foreach (var remove in slotsToRemove)
        //    _availableSlots.Remove(remove);

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
                            break;

                        case ExclusionVectorDirections.PlaneAndBackward:
                            DeckManager.DisableDeck(DeckManager.CurrentDeck);
                            break;
                    }
                }
            }
        }

        //AddConnectedSlots(module);
    }
    //private void AddConnectedSlots(Module module)
    //{
    //    foreach (var connector in module.ModuleBlueprint.Connectors)
    //    {
    //        //var pos = module.Position + connector.Position;
    //        var newPos = pos.Adjust(connector.Direction);

    //        if (!IsPositionOutsideOfExclusionSpaces(newPos))
    //            continue;

    //        if (DoesModuleOverlap(newPos))
    //            continue;

    //        ConnectorDirections newD = Connector.GetOpposite(connector.Direction);
    //        //_availableSlots.Add(new Connector(newD, connector.MaterialsConveyed, newPos));
    //    }
    //}

    public void DeleteModule(Module module)
    {
        //TODO:Enforce connector rules before deleting
    }

    #region Rules

    public bool IsPlacementValid(Module newModule)
    {
        var spaceValid = true;
        var conCount = newModule.ModuleBlueprint.AreConnectorsMandatory ? newModule.ModuleBlueprint.Connectors.Length : 1;

        if (FirstModule == null) return true;

        //foreach (var slot in _availableSlots)
        //    foreach (var modCon in newModule.ModuleBlueprint.Connectors)
        //        if (modCon.Equals(newModule.Position, slot))
        //            conCount--;

        //foreach (var space in newModule.ModuleBlueprint.Space)
        //{
        //    var spacePos = newModule.Position + space;

        //    if (IsPositionOutsideOfExclusionSpaces(spacePos))
        //    {
        //        foreach (var module in Modules)
        //        foreach (var mSpace in module.ModuleBlueprint.Space)
        //            if ((module.Position + mSpace).Equals(spacePos))
        //                spaceValid = false;
        //    }
        //    else
        //        spaceValid = false;
        //}

        return conCount <= 0 && spaceValid;
    }

    #endregion

    private static class ShipTypes
    {
        public static string StrikeCraft = "StrikeCraft";
        public static string Frigate = "Frigate";
        public static string Cruiser = "Cruiser";
        public static string CapitalShip = "CapitalShip";
    }
}
