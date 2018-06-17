using System;
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
        if (Cells.GetLength(1) > 10)
        {
            var cell = Cells[3, 10, 1].GetComponent<GridCell>();
            Debug.Log(cell.Connectors);
        }
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

    /// <summary>
    /// Checks if a connector can be added to the specified position
    /// </summary>
    /// <param name="connectorPosition">The position to add the connector to</param>
    /// <param name="excludeNewModule">Whether or not to check against all module, or exclude the newest-added one (used for the AddModule method)</param>
    /// <returns>A bool specifying whether or not the connector may be added</returns>
    private bool CanAddConnector(Vector3Int connectorPosition, bool excludeNewModule)
    {
        var canAddConnector = true;

        #region  Can't add an open connector to a space populated by another object

        if (Grid[connectorPosition.x, connectorPosition.y, connectorPosition.z].GameObject != null)
            canAddConnector = false;

        #endregion

        #region Can't add a module to an exlcusion vector-space

        //Validate against the newly-placed module?
        var count = excludeNewModule ? Modules.Count - 1 : Modules.Count;

        for (var i = 0; i < count; i++)
        {

            var module = Modules[i];
            foreach (var component in module.Components)
            {
                foreach (var ev in component.ExclusionVectors)
                {
                    var exclusionVectorPosition = module.Position + ev.Position;

                    foreach (var direction in ev.Directions)
                    {
                        switch (direction)
                        {
                            case ExclusionVectorDirections.ForwardLine:
                                if (exclusionVectorPosition.x == connectorPosition.x &&
                                    exclusionVectorPosition.y == connectorPosition.y &&
                                    exclusionVectorPosition.z <= connectorPosition.z)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.BackwardLine:
                                if (exclusionVectorPosition.x == connectorPosition.x &&
                                    exclusionVectorPosition.y == connectorPosition.y &&
                                    exclusionVectorPosition.z >= connectorPosition.z)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.UpwardLine:
                                if (exclusionVectorPosition.x == connectorPosition.x &&
                                    exclusionVectorPosition.y <= connectorPosition.y &&
                                    exclusionVectorPosition.z == connectorPosition.z)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.DownwardLine:
                                if (exclusionVectorPosition.x == connectorPosition.x &&
                                    exclusionVectorPosition.y >= connectorPosition.y &&
                                    exclusionVectorPosition.z == connectorPosition.z)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.RightLine:
                                if (exclusionVectorPosition.x <= connectorPosition.x &&
                                    exclusionVectorPosition.y == connectorPosition.y &&
                                    exclusionVectorPosition.z == connectorPosition.z)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.LeftLine:
                                if (exclusionVectorPosition.x >= connectorPosition.x &&
                                    exclusionVectorPosition.y == connectorPosition.y &&
                                    exclusionVectorPosition.z == connectorPosition.z)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.Plane:
                                if (exclusionVectorPosition.z == connectorPosition.z)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.PlaneAndAbove:
                                if (exclusionVectorPosition.z == connectorPosition.z &&
                                    exclusionVectorPosition.y >= connectorPosition.y)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.PlaneAndBelow:
                                if (exclusionVectorPosition.z >= connectorPosition.z &&
                                    exclusionVectorPosition.y <= connectorPosition.y)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.PlaneAndForward:
                                if (exclusionVectorPosition.z <= connectorPosition.z)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.PlaneAndBackward:
                                if (exclusionVectorPosition.z >= connectorPosition.z)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.PlaneAndRight:
                                if (exclusionVectorPosition.z >= connectorPosition.z &&
                                    exclusionVectorPosition.x <= connectorPosition.x)
                                    canAddConnector = false;
                                break;

                            case ExclusionVectorDirections.PlaneAndLeft:
                                if (exclusionVectorPosition.z >= connectorPosition.z &&
                                    exclusionVectorPosition.x >= connectorPosition.x)
                                    canAddConnector = false;
                                break;
                        }
                    }
                }
            }
        }

        #endregion

        return canAddConnector;
    }

    #endregion

    #region Add Module

    public string AddModule(GameObject selectedComponent, ModuleBlueprint blueprint, int rotations, int[] flips)
    {
        var module = ConfigureModule(selectedComponent, blueprint, rotations, flips);
        string msg = null;

        //validate connectors
        if (Modules.Count > 0)
            msg = ValidateConnectors(module);

        if (msg == null)
        {
            UpdateAddedValues(blueprint);
            AddModuleToState(module);
            CheckConnectors();
        }

        return msg;
    }

    private string ValidateConnectors(Module module)
    {
        string msg = null;

        int numConnectors = 1;

        if (module.ModuleBlueprint.AreConnectorsMandatory)
        {
            foreach (var component in module.Components)
                numConnectors += component.Connectors.Length;
        }

        foreach (var component in module.Components)
        {
            foreach (var connector in component.Connectors)
            {
                var position = module.Position + component.LocalPosition + connector.Position;
                var cell = Cells[position.x, position.y, position.z].GetComponent<GridCell>();

                if (cell == null)
                    continue;

                List<Materials> matsConveyed = new List<Materials>();

                if (connector.MaterialsConveyed != null)
                    matsConveyed = connector.MaterialsConveyed.ToList();

                foreach (var cellConnector in cell.Connectors)
                {
                    if (cellConnector.Direction == connector.Direction)
                    {
                        numConnectors--;
                        foreach (var material in matsConveyed)
                        {
                            foreach (var cellMaterial in cellConnector.MaterialsConveyed)
                            {
                                if (cellMaterial == material)
                                    matsConveyed.Remove(material);
                            }
                        }
                    }
                }

                if (matsConveyed.Count > 0)
                    foreach(var mat in matsConveyed)
                        msg += "ShipBuild.ModulePlace.Errors.MaterialsNotConveyed." + mat + "\r\n";
            }
        }

        if (numConnectors > 0)
            msg += "ShipBuild.ModulePlace.Errors.NumConnectors";

        return msg;
    }
    private Module ConfigureModule(GameObject selectedComponent, ModuleBlueprint blueprint, int rotations, int[] flips)
    {
        var newModule = Module.Create(blueprint);
        newModule = ModuleVectorUtils.RotateModule(newModule, rotations);
        newModule = ModuleVectorUtils.FlipModule(newModule, flips);
        newModule.Position = selectedComponent.GetComponent<GridCell>().Position;

        return newModule;
    }
    private void UpdateAddedValues(ModuleBlueprint blueprint)
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

        if (Modules.Count == 0)
            foreach (var cell in Cells)
                cell.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.14f, 0.14f);
    }
    private void AddModuleToState(Module newModule)
    {
        Modules.Add(newModule);

        foreach (var com in newModule.Components)
        {
            var aPos = newModule.Position + com.LocalPosition;
            var cell = Cells[aPos.x, aPos.y, aPos.z];
            cell.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.14f, 0.14f);
            cell.GetComponent<GridCell>().Connectors = new Connector[0];
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

                if (!(_gridSize - Vector3Int.one).Contains(conPos))
                {
                    Resize(new Vector3Int(con.Direction.x * 10, con.Direction.y * 10, Math.Abs(con.Direction.z)),
                        shiftDirection);

                    if (con.Direction.z != 0)
                    {
                        if (con.Direction.z > 0)
                            DeckManager.AddUpperDeck();
                        else if (con.Direction.z < 0)
                            DeckManager.AddLowerDeck();
                    }

                    newModule.Position += shiftDirection;
                    conPos += shiftDirection;
                    DeckManager.SelectDeck(DeckManager.CurrentDeck + shiftDirection.z);
                }

                if (!CanAddConnector(conPos, true))
                    continue;

                var cCell = Cells[conPos.x, conPos.y, conPos.z];
                var gridCell = cCell.GetComponent<GridCell>();
                gridCell.Connectors = gridCell.Connectors.Concat(Enumerable.Repeat(new Connector(con.Direction * -1, con.MaterialsConveyed), 1)).ToArray();
                cCell.GetComponent<SpriteRenderer>().color = new Color(0.22f, 0.66f, 0.22f);
            }
        }
    }
    private void CheckConnectors()
    {
        //remove existing connectors if invalid
        foreach (var cell in Cells)
        {
            var gridCell = cell.GetComponent<GridCell>();
            if (gridCell.Connectors.Length == 0) continue;

            foreach (var connector in gridCell.Connectors)
            {
                if (!CanAddConnector(connector.Position + gridCell.Position, false))
                {
                    var temp = gridCell.Connectors.ToList();
                    temp.Remove(connector);
                    gridCell.Connectors = temp.ToArray();
                    cell.GetComponent<SpriteRenderer>().color = new Color((153f/255f), (36f/255f), (36f/255f));
                }
            }
        }
    }

    #endregion

    private void Resize(Vector3Int add, Vector3Int shift)
    {
        var newCells = Resize(add, Cells);
        Cells = ReinitCells(newCells, Cells);
        var newGrid = Resize(add, Grid);
        Grid = ReinitComponents(newGrid, Grid, shift);
        _gridSize = new Vector3Int(Cells.GetLength(0), Cells.GetLength(1), Cells.GetLength(2));

        if (shift == Vector3Int.zero) return;

        foreach (var mod in Modules)
            mod.Position += shift;
    }
    private T[,,] Resize<T>(Vector3Int newSpace, T[,,] arr)
    {
        var xSize = arr.GetLength(0);
        var ySize = arr.GetLength(1);
        var zSize = arr.GetLength(2);

        var newArr = new T[xSize + Mathf.Abs(newSpace.x), ySize + Mathf.Abs(newSpace.y), zSize + Mathf.Abs(newSpace.z)];

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
