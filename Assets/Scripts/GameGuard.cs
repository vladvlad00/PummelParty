using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[DefaultExecutionOrder(10)]
public class GameGuard : MonoBehaviour
{
    public static GameGuard INSTANCE;

    public TextMeshProUGUI diceText;
    public GameObject board;

    public List<Player> players;

    [NonSerialized]
    public List<Spot> boardSpots;

    public Spot startingSpot;

    void Awake()
    {
        INSTANCE = this;

        boardSpots = new List<Spot>();

        foreach (Transform child in board.GetComponent<Transform>())
        {
            if (child.name.StartsWith("Spot"))
            {
                boardSpots.Add(child.GetComponent<Spot>());
            }
        }

        boardSpots.Sort((x, y) => x.name.CompareTo(y.name));

        for(int i = 0; i < boardSpots.Count; ++i)
        {
            boardSpots[i].index = i;
        }

        GameMaster.INSTANCE.OnReturnToGame();

        for(int i = 0; i < players.Count; ++i) {
            if(GameMaster.INSTANCE.playerData[i].spot == -1)
            {
                GameMaster.INSTANCE.playerData[i].spot = startingSpot.index;
            }

            players[i].data = GameMaster.INSTANCE.playerData[i];
            players[i].Reposition();
        }
    }
}
