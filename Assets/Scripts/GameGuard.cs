using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameGuard : MonoBehaviour
{
    public static GameGuard INSTANCE;

    public TextMeshProUGUI diceText;

    void Start()
    {
        INSTANCE = this;
        GameMaster.INSTANCE.OnReturnToGame();
    }
}
