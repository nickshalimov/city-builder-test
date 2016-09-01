using UnityEngine;

public class GameMenuState : MonoBehaviour
{
    [SerializeField]
    Game.State gameState;

    public Game.State GetState()
    {
        return gameState;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
