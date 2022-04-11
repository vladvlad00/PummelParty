using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Play : MonoBehaviour, IPointerClickHandler
{
    public ColorPicker playerColor1;
    public ColorPicker playerColor2;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameMaster.INSTANCE.playerData = new List<PlayerData>();
        GameMaster.INSTANCE.playerData.Add(new PlayerData("Player1", GameMaster.playerColors[playerColor1.colorIndex]));
        GameMaster.INSTANCE.playerData.Add(new PlayerData("Player2", GameMaster.playerColors[playerColor2.colorIndex]));
        TaleExtra.DisableInput();
        TaleExtra.RipOut();
        Tale.Scene("Scenes/Stranded");
        TaleExtra.RipIn();
        TaleExtra.EnableInput();
    }
}
