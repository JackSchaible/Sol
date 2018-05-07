using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scenes.ShipBuild;
using Assets.Scenes.ShipBuild.UI;
using Assets.Ships;
using Assets.Ships.Modules;
using Assets.Ships.Modules.Command;
using Assets.Utils.Extensions;
using UnityEngine;

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

    public Color InvalidCellColor = new Color(0.6f, 0.14f, 0.14f);
    public Color OpenCellColor = new Color(0.22f, 0.66f, 0.22f);

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
        Grid = new ModuleComponent[_gridSize.x, _gridSize.y, _gridSize.z];
        for (var z = 0; z < _gridSize.z; z++)
        {
            if (z > 0)
                DeckManager.AddLowerDeck();

            for (var x = 0; x < _gridSize.x; x++)
                for (var y = 0; y < _gridSize.y; y++)
                    InitializeCell(x, y, z);
        }

        DeckManager.SelectDeck(0);
    }

    public void AddModule(GameObject selectedComponent, Module module)
    {
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
            Grid[aPos.x + com.LocalPosition.x, aPos.y + com.LocalPosition.y, aPos.z + com.LocalPosition.z] = com;

            //Modify surrounding components to have receiving connectors
            foreach (var con in com.Connectors)
            {
                var conPos = aPos + con.Direction;

                var shiftDirection = Vector3Int.zero;

                if (con.Direction.x < 0)
                    shiftDirection = Vector3Int.right * 10;
                else if (con.Direction.y < 0)
                    shiftDirection = Vector3Int.up * 10;
                else if (con.Direction.z < 0)
                    shiftDirection = new Vector3Int(0, 0, 1);

                if (!_gridSize.Contains(conPos))
                    Resize(new Vector3Int(con.Direction.x * 10, con.Direction.y * 10, Math.Abs(con.Direction.z)), shiftDirection);

                if (con.Direction.z > 0)
                    DeckManager.AddUpperDeck();
                else if (con.Direction.z < 0)
                    DeckManager.AddLowerDeck();

                var gridCell = Cells[conPos.x + shiftDirection.x, conPos.y + shiftDirection.y, conPos.z + shiftDirection.z].GetComponent<GridCell>();
                gridCell.Connectors = gridCell.Connectors.Concat(Enumerable.Repeat(new Connector(con.Direction * -1, con.MaterialsConveyed), 1)).ToArray();
            }
        }

        //Disable decks if module has an x / y plane or space exclusion vector
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

        Modules.Add(newModule);
    }

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

    private void Resize(Vector3Int add, Vector3Int shift)
    {
        var newCells = Resize(add, Cells);
        Cells = ReinitCells(newCells, Cells);
        var newGrid = Resize(add, Grid);
        Grid = ReinitComponents(newGrid, Grid, shift);
    }
    private T[,,] Resize<T>(Vector3Int newSpace, T[,,] arr)
    {
        var xSize = arr.GetLength(0);
        var ySize = arr.GetLength(1);
        var zSize = arr.GetLength(2);

        var newArr = new T[xSize + newSpace.x, ySize + newSpace.y, zSize + newSpace.z];

        return newArr;
    }
    private GameObject[,,] ReinitCells(GameObject[,,] newArr, GameObject[,,] oldArr)
    {
        var xSize = oldArr.GetLength(0);
        var ySize = oldArr.GetLength(1);
        var zSize = oldArr.GetLength(2);

        for (int x = 0; x < xSize; x++)
            for (int y = 0; y < ySize; y++)
                for (int z = 0; z < zSize; z++)
                {
                    newArr[x, y, z] = oldArr[x, y, z];
                    InitializeCell(x, y, z);
                }

        return newArr;
    }
    private ModuleComponent[,,] ReinitComponents(ModuleComponent[,,] newArr, ModuleComponent[,,] oldArr, Vector3Int shift)
    {
        var xSize = oldArr.GetLength(0);
        var ySize = oldArr.GetLength(1);
        var zSize = oldArr.GetLength(2);

        for (int x = 0; x < xSize; x++)
            for (int y = 0; y < ySize; y++)
                for (int z = 0; z < zSize; z++)
                    if (oldArr[x, y, z].GameObject != null)
                        newArr[x + shift.x, y + shift.y, z + shift.z] = oldArr[x, y, z];

        return newArr;
    }

    private void InitializeCell(int x, int y, int z)
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

    private static class ShipTypes
    {
        public static string StrikeCraft = "StrikeCraft";
        public static string Frigate = "Frigate";
        public static string Cruiser = "Cruiser";
        public static string CapitalShip = "CapitalShip";
    }
}
