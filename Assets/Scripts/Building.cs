using UnityEngine;

public class Building : MonoBehaviour
{
    public enum State { Allowed, Forbidden, Placed }

    [SerializeField]
    Renderer renderer;

    [SerializeField]
    Material allowedStateMaterial;

    [SerializeField]
    Material forbiddenStateMaterial;

    Material normalStateMaterial;

    int cubeSize;
    State state;

    int x, y;

    public bool isAllowed { get { return state == State.Allowed; } }

    public bool isPlaced { get { return state == State.Placed; } }

    void Awake()
    {
        normalStateMaterial = renderer.sharedMaterial;
    }

    public void SetSize(int size, float cellSize)
    {
        cubeSize = size;
    }

    public int GetSize()
    {
        return cubeSize;
    }

    public FieldPoint GetPoint()
    {
        return new FieldPoint { x = x, y = y };
    }

    public void SetAllowed(bool allowed)
    {
        SetState(allowed ? State.Allowed : State.Forbidden);
    }

    public void Place(FieldPoint point)
    {
        SetState(State.Placed);
        x = point.x;
        y = point.y;
    }

    void SetState(State newState)
    {
        state = newState;

        if (state == State.Placed)
        {
            renderer.sharedMaterial = normalStateMaterial;
        }
        else if (state == State.Allowed)
        {
            renderer.sharedMaterial = allowedStateMaterial;
        }
        else if (state == State.Forbidden)
        {
            renderer.sharedMaterial = forbiddenStateMaterial;
        }
    }
}
