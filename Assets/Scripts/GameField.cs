using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField]
    float cellSize = 1.0f;

    [SerializeField]
    GameObject groundProto;

    [SerializeField]
    GameObject blockProto;

    [SerializeField]
    Building[] buildingProtos = {};

    Building newBuilding;
    FieldPoint activeCell = new FieldPoint();
    FieldPoint placeCell = new FieldPoint();
    FieldData fieldData;
    Dictionary<int, Building> buildings = new Dictionary<int, Building>();

    public void Initialize(int width, int height, float lockedRatio)
    {
        fieldData = new FieldData(width, height, lockedRatio);
        transform.localPosition = new Vector3(-0.5f * width * cellSize, 0.0f, -0.5f * height * cellSize);

        for (int x = 0; x < fieldData.width; ++x)
        {
            for (int y = 0; y < fieldData.height; ++y)
            {
                if (fieldData.GetState(x, y) == FieldData.CellState.Free)
                {
                    AddGround(x, y);
                }
                else
                {
                    AddBlock(x, y);
                }
            }
        }
    }

    void AddGround(int x, int y)
    {
        var ground = Instantiate(groundProto);
        ground.transform.SetParent(transform, true);
        ground.transform.localPosition = new Vector3(x * cellSize, 0.0f, y * cellSize);
    }

    void AddBlock(int x, int y)
    {
        var block = Instantiate(blockProto);
        block.transform.SetParent(transform, true);
        block.transform.localPosition = new Vector3(x * cellSize, 0.0f, y * cellSize);
    }

    public void StartPlacing(int size)
    {
        newBuilding = Instantiate(buildingProtos[size - 1]);
        newBuilding.transform.SetParent(transform, false);
        newBuilding.gameObject.SetActive(true);

        newBuilding.SetSize(size, cellSize);

        SetBuildingPoint(activeCell);
    }

    public void StopPlacing()
    {
        Destroy(newBuilding.gameObject);
        newBuilding = null;
    }

    public void SetActiveCell(Vector3 worldPoint)
    {
        FieldPoint cell = GetPoint(worldPoint);
        if (IsPointValid(cell)) { activeCell = cell; }
    }

    public void MoveBuildingTo(Vector3 worldPoint)
    {
        if (newBuilding.isPlaced) { return; }
        var cell = GetPoint(worldPoint);
        SetBuildingPoint(cell);
    }

    public Building SelectBuilding(Vector3 worldPoint)
    {
        var cell = GetPoint(worldPoint);
        var buildingCell = fieldData.GetBuildingPoint(cell.x, cell.y);

        if (buildingCell == null) { return null; }

        int hash = GetPointHash(buildingCell);
        return buildings[hash];
    }

    public bool IsPointOverBuilding(Vector3 worldPoint)
    {
        if (newBuilding == null) { return false; }

        var cell = GetPoint(worldPoint);
        var cubeSize = newBuilding.GetSize();

        return cell.x >= placeCell.x && cell.x < placeCell.x + cubeSize
            && cell.y >= placeCell.y && cell.y < placeCell.y + cubeSize;
    }

    public bool TryToPlaceBuilding()
    {
        if (newBuilding.isAllowed)
        {
            var cubeSize = newBuilding.GetSize();
            for (int x = 0; x < cubeSize; ++x)
            {
                for (int y = 0; y < cubeSize; ++y)
                {
                    fieldData.SetState(placeCell.x + x, placeCell.y + y, FieldData.CellState.Locked);
                    fieldData.SetCellOwnerPosition(placeCell.x + x, placeCell.y + y, placeCell);
                }
            }

            buildings.Add(GetPointHash(placeCell), newBuilding);

            newBuilding.Place(placeCell);
            newBuilding = null;
            return true;
        }

        return false;
    }

    public void RemoveBuilding(Building building)
    {
        var cubeSize = building.GetSize();
        var cell = building.GetPoint();
        buildings.Remove(GetPointHash(cell));

        for (int x = 0; x < cubeSize; ++x)
        {
            for (int y = 0; y < cubeSize; ++y)
            {
                fieldData.SetState(cell.x + x, cell.y + y, FieldData.CellState.Free);
                fieldData.SetCellOwnerPosition(cell.x + x, cell.y + y, null);
            }
        }

        Destroy(building.gameObject);
    }

    void SetBuildingPoint(FieldPoint point)
    {
        var cubeSize = newBuilding.GetSize();
        placeCell.x = point.x - cubeSize / 2;
        placeCell.y = point.y - cubeSize / 2;

        while (placeCell.x < 0) { placeCell.x++; }
        while (placeCell.x + cubeSize > fieldData.width) { placeCell.x--; }
        while (placeCell.y < 0) { placeCell.y++; }
        while (placeCell.y + cubeSize > fieldData.height) { placeCell.y--; }

        newBuilding.transform.localPosition = new Vector3(placeCell.x * cellSize, 0.0f, placeCell.y * cellSize);
        newBuilding.SetAllowed(CanPlaceBuilding());
    }

    bool CanPlaceBuilding()
    {
        var cubeSize = newBuilding.GetSize();

        for (int x = 0; x < cubeSize; ++x)
        {
            for (int y = 0; y < cubeSize; ++y)
            {
                if (fieldData.GetState(placeCell.x + x, placeCell.y + y) != FieldData.CellState.Free)
                {
                    return false;
                }
            }
        }

        return true;
    }

    FieldPoint GetPoint(Vector3 worldPoint)
    {
        var position = transform.InverseTransformPoint(worldPoint);

        return new FieldPoint() {
            x = Mathf.FloorToInt(position.x / cellSize),
            y = Mathf.FloorToInt(position.z / cellSize)
        };
    }

    bool IsPointValid(FieldPoint point)
    {
        return point.x >= 0 && point.y >= 0 && point.x < fieldData.width && point.y < fieldData.height;
    }

    int GetPointHash(FieldPoint point)
    {
        return point.x * fieldData.width + point.y;
    }
}
