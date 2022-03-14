using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMaster : MonoBehaviour
{
    public enum State
    {
        IDLE,
        PLAYER_MOVING
    }

    public GameObject board;
    public List<Player> players;
    public TextMeshProUGUI diceText;
    
    [NonSerialized]
    public List<Transform> boardSpots;

    int currentPlayer;
    State state;

    void Awake()
    {
        boardSpots = new List<Transform>();

        foreach(Transform child in board.transform)
        {
            if(child.name.StartsWith("Spot"))
            {
                boardSpots.Add(child);
            }
        }

        boardSpots.Sort((x, y) => x.name.CompareTo(y.name));

        currentPlayer = 0;

        state = State.IDLE;
    }

    void Update()
    {
        switch(state)
        {
            case State.IDLE:
                WaitForPlayerMove();
                break;
            case State.PLAYER_MOVING:
                if(players[currentPlayer].state == Player.State.IDLE)
                {
                    state = State.IDLE;
                    currentPlayer = (currentPlayer + 1) % players.Count;
                }
                break;
        }
    }

    void WaitForPlayerMove()
    {
        if (Input.GetKeyDown("space"))
        {
            OnPlayerRoll();
        }
    }

    void OnPlayerRoll()
    {
        int dice = UnityEngine.Random.Range(1, 7);
        diceText.text = dice.ToString();

        int targetSpot = (players[currentPlayer].spot + dice) % boardSpots.Count;

        players[currentPlayer].SetTargetSpot(targetSpot);

        state = State.PLAYER_MOVING;
    }
}
