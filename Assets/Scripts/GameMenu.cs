using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    GameMenuState[] states = {};

    [SerializeField]
    InfoWindow infoWindow;

    public void OnGameStateSet(Game.State state)
    {
        for (int i = 0; i < states.Length; ++i)
        {
            states[i].SetActive(states[i].GetState() == state);
        }
    }

    public void ShowBuildingInfo(Building building)
    {
        infoWindow.ShowInfo(building);
    }
}
