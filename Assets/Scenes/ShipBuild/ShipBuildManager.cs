using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Common.Utils;
using Assets.Data;
using Assets.Scenes.ShipBuild;
using Assets.Scenes.ShipBuild.UI;
using Assets.Ships;
using Assets.Ships.Modules;
using Assets.Ships.Modules.Command;
using Assets.Utils.Extensions;
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
    public GameObject[,,] Cells { get; set; }
    public ModuleComponent[,,] Grid { get; set; }
    public List<Module> Modules { get; private set; }

    public bool HasCommandModule { get; private set; }

    public Color InvalidCellColor = new Color(155, 35, 35);
    public Color OpenCellColor = new Color(55, 170, 55);

    private readonly Dictionary<string, Vector3Int> _shipSizes = new Dictionary<string, Vector3Int>
    {
        {ShipTypes.StrikeCraft, new Vector3Int(10, 10, 3)},
        {ShipTypes.Frigate, new Vector3Int(25, 25, 10)},
        {ShipTypes.Cruiser, new Vector3Int(40, 40, 20)},
        {ShipTypes.CapitalShip, new Vector3Int(80, 80, 40)},
    };
    private Vector3Int _gridSize;

    void Start()
    {
        InitializeGrid();
    }

    void Update()
    {
    }

    public void InitializeGrid()
    {
        _gridSize = new Vector3Int(10, 10, 1);

        Cells = new GameObject[_gridSize.x, _gridSize.y, _gridSize.z];

        for (var z = 0; z < _gridSize.z; z++)
        {
            if (z > 0)
                DeckManager.AddLowerDeck();

            for (var x = 0; x < _gridSize.x; x++)
                for (var y = 0; y < _gridSize.y; y++)
                {
                    Cells[x, y, z] = Instantiate(ComponentPrefab);
                    Cells[x, y, z].transform.position = new Vector3Int(x, y, z);
                    Cells[x, y, z].GetComponent<SpriteRenderer>().color = OpenCellColor;
                    Cells[x, y, z].name = "Grid Cell (" + x + ", " + y + ", " + z + ")";

                    var com = Cells[x, y, z].GetComponent<GridCell>();
                    com.Position = new Vector3Int(x, y, z);
                    com.Connectors = new Connector[0];
                    com.ExclusionVectors = new ExclusionVector[0];
                }
        }

        DeckManager.SelectDeck(0);
    }

    public void AddModule(GameObject selectedComponent, Module module)
    {
        //TODO: Add to modules list

        PowerUsed += module.ModuleBlueprint.PowerConumption;
        ControlUsed += module.ModuleBlueprint.CommandRequirement;
        PersonnelUsed += module.ModuleBlueprint.CrewRequirement;

        var commandBlueprint = module.ModuleBlueprint as CommandModuleBlueprint;
        if (commandBlueprint != null)
        {
            ControlAvailable += commandBlueprint.CommandSupplied;
            HasCommandModule = true;
        }

        var cockpitBlueprint = module.ModuleBlueprint as CockpitModuleBlueprint;
        if (cockpitBlueprint != null)
            PersonnelAvailable += cockpitBlueprint.PersonnelHoused;

        Vector3Int pos = selectedComponent.GetComponent<GridCell>().Position;
        var newModule = Module.Create(module.ModuleBlueprint.Copy());
        //TODO: adjust the newmodule's rotation/flip orientation? Maybe? Does copy copy that stuff for me?
        foreach (var com in newModule.Components)
        {
            var aPos = pos + com.LocalPosition;
            var cell = Cells[aPos.x, aPos.y, aPos.z];

            cell.GetComponent<SpriteRenderer>().color = InvalidCellColor;
            com.GameObject.transform.position = cell.gameObject.transform.position;

            //Modify surrounding components to have receiving connectors
            foreach (var con in com.Connectors)
            {
                var conPos = aPos + con.Direction;
                if (!_gridSize.Contains(conPos))
                    continue;

                var gridCell = Cells[conPos.x, conPos.y, conPos.z].GetComponent<GridCell>();
                gridCell.Connectors = gridCell.Connectors.Concat(Enumerable.Repeat(new Connector(con.Direction.Times(-1), con.MaterialsConveyed), 1)).ToArray();
            }
        }

        //Disable decks if module has an x/y plane or space exclusion vector
        //if (module.ModuleBlueprint.ExclusionVectors.Length > 0)
        //{
        //    foreach (var vectors in module.ModuleBlueprint.ExclusionVectors)
        //    {
        //        foreach (var vector in vectors.Direction)
        //        {
        //            //Disable whatever it is
        //            switch (vector)
        //            {
        //                case ExclusionVectorDirections.Plane:
        //                    DeckManager.DisableDeck(DeckManager.CurrentDeck);
        //                    break;

        //                case ExclusionVectorDirections.PlaneAndForward:
        //                    DeckManager.DisableDeck(DeckManager.CurrentDeck);
        //                    break;

        //                case ExclusionVectorDirections.PlaneAndBackward:
        //                    DeckManager.DisableDeck(DeckManager.CurrentDeck);
        //                    break;
        //            }
        //        }
        //    }
        //}

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

        if (Modules.Count == 0) return true;

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
