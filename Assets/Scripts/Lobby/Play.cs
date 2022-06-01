using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Play : MonoBehaviour, IPointerClickHandler
{
    public ColorPicker playerColor1;
    public ColorPicker playerColor2;
    public TMP_InputField playerName1;
    public TMP_InputField playerName2;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameMaster.INSTANCE.playerData = new List<PlayerData>();
        GameMaster.INSTANCE.playerData.Add(new PlayerData(playerName1.text, GameMaster.playerColors[playerColor1.colorIndex]));
        GameMaster.INSTANCE.playerData.Add(new PlayerData(playerName2.text, GameMaster.playerColors[playerColor2.colorIndex]));
        TaleExtra.DisableInput();
        TaleExtra.RipOut();
        Tale.Scene("Scenes/Stranded");
        TaleExtra.RipIn();
        TaleExtra.EnableInput();
    }
}
