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
    public List<Spot> graveyardSpots;

    public Material normalSpotMaterial;
    public Material crownSpotMaterial;
    public Material reverseSpotMaterial;
    public Material hpplusSpotMaterial;
    public Material hpminusSpotMaterial;
    public Material coinsSpotMaterial;
    public Material teleportSpotMaterial;
    public Material respawnSpotMaterial;
    public Material itemSpotMaterial;
    public Material startSpotMaterial;

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

    public bool IsGraveyard(Spot spot)
    {
        return graveyardSpots.Contains(spot);
    }

    public Spot GetRandomGraveyard()
    {
        return graveyardSpots[UnityEngine.Random.Range(0, graveyardSpots.Count)];
    }

    public Spot GetRandomSpot(bool includeCrown = false)
    {
        Spot spot = boardSpots[UnityEngine.Random.Range(0, boardSpots.Count)];
        while (IsGraveyard(spot) || (spot == GameMaster.INSTANCE.crownSpot && includeCrown))
        {
            spot = boardSpots[UnityEngine.Random.Range(0, boardSpots.Count)];
        }
        return spot;
    }
}
