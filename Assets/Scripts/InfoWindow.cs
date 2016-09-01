using System;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour
{
    [SerializeField]
    Text sizeValue;

    public void ShowInfo(Building building)
    {
        gameObject.SetActive(true);

        sizeValue.text = string.Format("{0}x{0}", building.GetSize());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
