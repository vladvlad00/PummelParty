using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour, IPointerClickHandler
{
    public int colorIndex;

    private static bool[] selected = new bool[GameMaster.playerColors.Length];

    void Start()
    {
        GetComponent<Image>().color = GameMaster.playerColors[colorIndex].color;
        selected[colorIndex] = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        do
        {
            selected[colorIndex] = false;
            colorIndex = (colorIndex + 1) % GameMaster.playerColors.Length;
        } while (selected[colorIndex]);
        GetComponent<Image>().color = GameMaster.playerColors[colorIndex].color;
        selected[colorIndex] = true;
    }
}
