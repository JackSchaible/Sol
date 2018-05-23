﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Data;
using Assets.Scenes.ShipBuild;
using Assets.Scenes.ShipBuild.UI;
using Assets.Ships;
using Assets.Ships.Modules;
using Assets.Ships.Modules.Command;
using Assets.Utils.Extensions;
using Assets.Utils.ModuleUtils;
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

    private Vector3Int _gridSize;

    private void Awake()
    {
        Modules = new List<Module>();
        InitializeGrid();
    }

    private void Update()
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
                    InitializeCell(x, y, z, Cells);
        }

        DeckManager.SelectDeck(0);
    }

    public void AddModule(GameObject selectedComponent, ModuleBlueprint blueprint, int rotations, int[] flips)
    {
        PowerUsed += blueprint.PowerConumption;
        ControlUsed += blueprint.CommandRequirement;
        PersonnelUsed += blueprint.CrewRequirement;

        var commandBlueprint = blueprint as CommandModuleBlueprint;
        if (commandBlueprint != null)
        {
            ControlAvailable += commandBlueprint.CommandSupplied;
            HasCommandModule = true;
        }

        var cockpitBlueprint = blueprint as CockpitModuleBlueprint;
        if (cockpitBlueprint != null)
            PersonnelAvailable += cockpitBlueprint.PersonnelHoused;

        Vector3Int pos = selectedComponent.GetComponent<GridCell>().Position;

        var newModule = Module.Create(blueprint);
        newModule = ModuleVectorUtils.RotateModule(newModule, rotations);
        newModule = ModuleVectorUtils.FlipModule(newModule, flips);

        if (Modules.Count == 0)
            foreach (var cell in Cells)
                cell.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.14f, 0.14f);

        Modules.Add(newModule);
        
        foreach (var com in newModule.Components)
        {
            var aPos = pos + com.LocalPosition;
            var cell = Cells[aPos.x, aPos.y, aPos.z];
            cell.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.14f, 0.14f);
            com.GameObject.GetComponent<SpriteRenderer>().color = Color.white;
            com.GameObject.transform.position = cell.gameObject.transform.position;
            Grid[aPos.x, aPos.y, aPos.z] = com;

            //Modify surrounding components to have receiving connectors
            foreach (var con in com.Connectors)
            {
                var conPos = aPos + con.Direction;

                var shiftDirection = Vector3Int.zero;

                if (conPos.x < 0)
                    shiftDirection += Vector3Int.right * 10;
                else if (conPos.y < 0)
                    shiftDirection += Vector3Int.up * 10;
                else if (conPos.z < 0)
                    shiftDirection += new Vector3Int(0, 0, 1);

                aPos += shiftDirection;

                if (!_gridSize.Contains(conPos))
                {
                    Resize(new Vector3Int(con.Direction.x * 10, con.Direction.y * 10, Math.Abs(con.Direction.z)),
                        shiftDirection);

                    if (con.Direction.z != 0)
                    {
                        if (con.Direction.z > 0)
                            DeckManager.AddUpperDeck();
                        else if (con.Direction.z < 0)
                            DeckManager.AddLowerDeck();

                        DeckManager.SelectDeck(DeckManager.CurrentDeck + shiftDirection.z);
                    }

                    pos += shiftDirection;
                    conPos += shiftDirection;
                }

                if (!CanAddConnector(conPos))
                    continue;

                var cCell = Cells[conPos.x, conPos.y, conPos.z];
                var gridCell = cCell.GetComponent<GridCell>();
                gridCell.Connectors = gridCell.Connectors.Concat(Enumerable.Repeat(new Connector(con.Direction * -1, con.MaterialsConveyed), 1)).ToArray();
                cCell.GetComponent<SpriteRenderer>().color = new Color(0.22f, 0.66f, 0.22f);
            }
        }

        //Disable decks if module has an x / y plane or space exclusion vector
        if (blueprint.ExclusionVectors.Length > 0)
        {
            foreach (var vectors in blueprint.ExclusionVectors)
            {
                foreach (var vector in vectors.Directions)
                {
                    //Disable whatever it is
                    switch (vector)
                    {
                        case ExclusionVectorDirections.Plane:
                            break;

                        case ExclusionVectorDirections.PlaneAndForward:
                            break;

                        case ExclusionVectorDirections.PlaneAndBackward:
                            break;
                    }
                }
            }
        }
    }

    public void DeleteModule(Module module)
    {
        //TODO:Enforce connector rules before deleting
    }

    #region Rules

    public Connector[] GetConnectors(GameObject selectedCell)
    {
        var cell = selectedCell.GetComponent<GridCell>();
        return cell.Connectors;
    }

    private bool CanAddConnector(Vector3Int position)
    {
        var canAddConnector = true;

        //Can't add an open connector to a space populated by another object
        if (Grid[position.x, position.y, position.z].GameObject != null)
            canAddConnector = false;

        //Can't add a module to an exlcusion vector-space
        foreach (var module in Modules)
        foreach (var component in module.Components)
        foreach (var ev in component.ExclusionVectors)
        foreach (var direction in ev.Directions)
            switch (direction)
            {
                case ExclusionVectorDirections.ForwardLine:
                    if (ev.Position.x == position.x &&
                        ev.Position.y == position.y &&
                        ev.Position.z <= position.z)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.BackwardLine:
                    if (ev.Position.x == position.x &&
                        ev.Position.y == position.y &&
                        ev.Position.z >= position.z)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.UpwardLine:
                    if (ev.Position.x == position.x &&
                        ev.Position.y <= position.y &&
                        ev.Position.z == position.z)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.DownwardLine:
                    if (ev.Position.x == position.x &&
                        ev.Position.y >= position.y &&
                        ev.Position.z == position.z)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.RightLine:
                    if (ev.Position.x <= position.x &&
                        ev.Position.y == position.y &&
                        ev.Position.z == position.z)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.LeftLine:
                    if (ev.Position.x >= position.x &&
                        ev.Position.y == position.y &&
                        ev.Position.z == position.z)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.Plane:
                    if (ev.Position.z == position.z)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.PlaneAndAbove:
                    if (ev.Position.z == position.z &&
                        ev.Position.y >= position.y)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.PlaneAndBelow:
                    if (ev.Position.z >= position.z &&
                        ev.Position.y <= position.y)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.PlaneAndForward:
                    if (ev.Position.z >= position.z)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.PlaneAndBackward:
                    if (ev.Position.z <= position.z)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.PlaneAndRight:
                    if (ev.Position.z >= position.z &&
                        ev.Position.x <= position.x)
                        canAddConnector = false;
                    break;

                case ExclusionVectorDirections.PlaneAndLeft:
                    if (ev.Position.z >= position.z &&
                        ev.Position.x >= position.x)
                        canAddConnector = false;
                    break;
        }

        return canAddConnector;
    }

    #endregion

    private void Resize(Vector3Int add, Vector3Int shift)
    {
        var newCells = Resize(add, Cells);
        Cells = ReinitCells(newCells, Cells);
        var newGrid = Resize(add, Grid);
        Grid = ReinitComponents(newGrid, Grid, shift);
        _gridSize = new Vector3Int(Cells.GetLength(0), Cells.GetLength(1), Cells.GetLength(2));
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
        var oldXSize = oldArr.GetLength(0);
        var oldYSize = oldArr.GetLength(1);
        var oldZSize = oldArr.GetLength(2);

        var xSize = newArr.GetLength(0);
        var ySize = newArr.GetLength(1);
        var zSize = newArr.GetLength(2);

        for (int x = 0; x < xSize; x++)
            for (int y = 0; y < ySize; y++)
                for (int z = 0; z < zSize; z++)
                    if (x < oldXSize && y < oldYSize && z < oldZSize)
                        newArr[x, y, z] = oldArr[x, y, z];
                    else
                        InitializeCell(x, y, z, newArr);

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
                    {
                        newArr[x + shift.x, y + shift.y, z + shift.z] = oldArr[x, y, z];
                        newArr[x + shift.x, y + shift.y, z + shift.z].GameObject.transform.position =
                            Cells[x + shift.x, y + shift.y, z + shift.z].transform.position;
                    }

        return newArr;
    }
    private void InitializeCell(int x, int y, int z, GameObject[,,] cells)
    {
        cells[x, y, z] = Instantiate(ComponentPrefab);
        cells[x, y, z].transform.position = new Vector3Int(x, y, z);
        cells[x, y, z].GetComponent<SpriteRenderer>().color = (Modules.Count == 0 ? new Color(0.22f, 0.66f, 0.22f) : new Color(0.6f, 0.14f, 0.14f));
        cells[x, y, z].name = "Grid Cell (" + x + ", " + y + ", " + z + ")";

        var com = cells[x, y, z].GetComponent<GridCell>();
        com.Position = new Vector3Int(x, y, z);
        com.Connectors = new Connector[0];
        com.ExclusionVectors = new ExclusionVector[0];
    }
}
