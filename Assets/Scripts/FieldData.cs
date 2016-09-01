using UnityEngine;

public class FieldData
{
    public enum CellState
    {
        Free,
        Locked
    }

    class CellInfo
    {
        public CellState state;
        public FieldPoint ownerPosition;
    }

    CellInfo[,] cells;

    public FieldData(int fieldWidth, int fieldHeight, float lockedRatio)
    {
        cells = new CellInfo[fieldWidth, fieldHeight];
        for (int x = 0; x < fieldWidth; ++x)
        {
            for (int y = 0; y < fieldHeight; ++y)
            {
                cells[x, y] = new CellInfo {
                    state = (Random.value < lockedRatio) ? CellState.Locked : CellState.Free
                };
            }
        }
    }

    public int width { get { return cells.GetLength(0); } }

    public int height { get { return cells.GetLength(1); } }

    public CellState GetState(int x, int y)
    {
        return cells[x, y].state;
    }

    public void SetState(int x, int y, CellState state)
    {
        cells[x, y].state = state;
    }

    public FieldPoint GetBuildingPoint(int x, int y)
    {
        return cells[x, y].ownerPosition;
    }

    public void SetCellOwnerPosition(int x, int y, FieldPoint point)
    {
        cells[x, y].ownerPosition = (point == null) ? null : new FieldPoint { x = point.x, y = point.y };
    }
}
