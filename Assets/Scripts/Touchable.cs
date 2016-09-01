using UnityEngine.UI;

public class Touchable : Graphic
{
    protected override void OnPopulateMesh(UnityEngine.Mesh m)
    {
        m.Clear();
    }

    public override bool Raycast(UnityEngine.Vector2 sp, UnityEngine.Camera eventCamera)
    {
        return raycastTarget;
    }
}
