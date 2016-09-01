using UnityEngine;

public class Game : MonoBehaviour
{
    public enum State
    {
        Initial,
        Construction,
        Placing,
        Selected
    }

    [SerializeField]
    GameField gameField;

    [SerializeField]
    GameMenu gameMenu;

    State stateValue;
    Building selection;

    void Start()
    {
        GenerateField();
        state = State.Initial;
    }

    void GenerateField()
    {
        gameField.Initialize(100, 100, 0.1f);
    }

    public GameField field { get { return gameField; } }

    public State state
    {
        get { return stateValue; }
        set
        {
            stateValue = value;
            gameMenu.OnGameStateSet(stateValue);
        }
    }

    public void StartConstruction()
    {
        state = State.Construction;
    }

    public void ConstructBuilding(int size)
    {
        gameField.StartPlacing(size);
        state = State.Placing;
    }

    public void PlaceBuilding()
    {
        if (gameField.TryToPlaceBuilding())
        {
            state = State.Initial;
        }
    }

    public void CancelPlacing()
    {
        gameField.StopPlacing();
        state = State.Initial;
    }

    public void ShowBuildingInfo()
    {
        gameMenu.ShowBuildingInfo(selection);
    }

    public void RemoveBuilding()
    {
        field.RemoveBuilding(selection);
        state = State.Initial;
    }

    public void SelectBuilding(Building building)
    {
        selection = building;
        state = (selection != null) ? State.Selected : State.Initial;
    }
}
