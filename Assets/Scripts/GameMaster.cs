using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class GameMaster : MonoBehaviour
{
    public enum State
    {
        IDLE,
        PLAYER_MOVING,
        MINIGAME_RESULTS
    }

    public const float MINIGAME_RESULTS_DURATION = 2f;

    public GameObject board;
    public List<Player> players;
    public TextMeshProUGUI diceText;

    public GameObject minigameCanvas;
    public TextMeshProUGUI minigameResultText;
    
    [NonSerialized]
    public List<Transform> boardSpots;

    int currentPlayer;
    State state;
    float clock;

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
        clock = 0f;
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
                    OnPlayerMovingEnd();
                }
                break;
            case State.MINIGAME_RESULTS:
                clock += Time.deltaTime;

                if(clock >= MINIGAME_RESULTS_DURATION)
                {
                    minigameResultText.text = "";
                    minigameCanvas.SetActive(false);
                    state = State.IDLE;
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

    void OnPlayerMovingEnd()
    {
        for(int i = 0; i < boardSpots.Count; i++)
        {
            List<Player> spotPlayers = new List<Player>();

            for(int j = 0; j < players.Count; ++j)
            {
                if(players[j].spot == i)
                {
                    spotPlayers.Add(players[j]);
                }
            }

            if(spotPlayers.Count > 1)
            {
                List<int> rolls = new List<int>();
                int maxIndex = -1;

                StringBuilder sb = new StringBuilder();

                foreach(Player p in spotPlayers)
                {
                    int roll = UnityEngine.Random.Range(1, 7);
                    rolls.Add(roll);

                    if(maxIndex == -1 || rolls[maxIndex] < roll)
                    {
                        maxIndex = rolls.Count - 1;
                    }

                    if(sb.Length > 0)
                    {
                        sb.Append("\n");
                    }

                    sb.AppendFormat("Player <color=#880000>{0}</color> rolled <color=#880000>{1}</color>", rolls.Count, roll);
                }

                sb.Append("\n");

                for(int j = 0; j < players.Count; ++j)
                {
                    if(rolls[j] == rolls[maxIndex])
                    {
                        if(sb.Length > 0)
                        {
                            sb.Append("\n");
                        }
                        sb.AppendFormat("Player {0} won!", j + 1);
                    }
                }

                minigameCanvas.SetActive(true);
                minigameResultText.text = sb.ToString();

                state = State.MINIGAME_RESULTS;
                clock = 0f;
                break;
            }
        }
    }
}
