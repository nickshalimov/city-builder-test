using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField]
    Game game;

    Camera gameCamera;
    RawImage rawImage;

    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
    bool isDraggingCube;

    void Awake()
    {
        gameCamera = Camera.main;
    }

    void Start()
    {
        UpdateActiveCell();
    }

    public void HandleClick(BaseEventData eventData)
    {
        if (game.state == Game.State.Construction)
        {
            game.state = Game.State.Initial;
        }
        else if (game.state == Game.State.Initial || game.state == Game.State.Selected)
        {
            var pointer = eventData as PointerEventData;
            Vector3 groundPoint;
            if (!GetGroundPoint(pointer.position, out groundPoint)) { return; }

            var building = game.field.SelectBuilding(groundPoint);
            game.SelectBuilding(building);
        }
    }

    public void HandleBeginDrag(BaseEventData eventData)
    {
        var pointer = eventData as PointerEventData;

        Vector3 groundPoint;
        isDraggingCube = GetGroundPoint(pointer.position, out groundPoint)
            && game.field.IsPointOverBuilding(groundPoint);
    }

    public void HandleDrag(BaseEventData eventData)
    {
        var pointer = eventData as PointerEventData;

        Vector3 prevGroundPoint;
        if (!GetGroundPoint(pointer.position - pointer.delta, out prevGroundPoint)) { return; }

        Vector3 groundPoint;
        if (!GetGroundPoint(pointer.position, out groundPoint)) { return; }

        if (!isDraggingCube)
        {
            gameCamera.transform.parent.position -= (groundPoint - prevGroundPoint);
            UpdateActiveCell();
        }
        else
        {
            game.field.MoveBuildingTo(groundPoint);
        }
    }

    bool GetGroundPoint(Vector2 screenPoint, out Vector3 point)
    {
        var ray = gameCamera.ScreenPointToRay(screenPoint);

        float distance = 0.0f;
        if (groundPlane.Raycast(ray, out distance))
        {
            point = ray.GetPoint(distance);
            return true;
        }

        point = Vector3.zero;
        return false;
    }

    void UpdateActiveCell()
    {
        var screenCenter = new Vector2(gameCamera.pixelWidth * 0.5f, gameCamera.pixelHeight * 0.5f);
        Vector3 point;
        if (!GetGroundPoint(screenCenter, out point)) { return; }

        game.field.SetActiveCell(point);
    }
}
