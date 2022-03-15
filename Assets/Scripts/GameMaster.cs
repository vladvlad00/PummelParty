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
        MINIGAME,
        SPOT_MINIGAME_FINISHED
    }

    public const float MINIGAME_RESULTS_DURATION = 2f;
    public static bool INPUT_ENABLED = true;

    public static GameMaster INSTANCE = null;

    public GameObject board;
    public List<Player> players;

    [NonSerialized]
    public GameGuard guard = null; // Holds stuff specific to the Game scene
    [NonSerialized]
    public List<Transform> boardSpots;

    [NonSerialized]
    public int minigameTriggerSpot; // The spot where the minigame was triggered
    [NonSerialized]
    public List<Player> minigameTriggerPlayers; // The players on that spot;

    int currentPlayer;
    State state;

    void Awake()
    {
        if(INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }

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

        OnReturnToGame();

        DontDestroyOnLoad(gameObject);
        INSTANCE = this;
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
            case State.MINIGAME:
                break;
            case State.SPOT_MINIGAME_FINISHED:
                TriggerMinigameOnSpot(); // Check for additional spots with multiple players

                if(state == State.SPOT_MINIGAME_FINISHED)
                {
                    state = State.IDLE;
                }
                break;
        }
    }

    public void OnSpotMinigameEnd()
    {
        state = State.SPOT_MINIGAME_FINISHED;
        ++minigameTriggerSpot;
    }

    // Minigame -> Game scene
    public void OnReturnToGame()
    {
        guard = GameGuard.INSTANCE;
    }

    void WaitForPlayerMove()
    {
        if (INPUT_ENABLED && Input.GetKeyDown("space"))
        {
            OnPlayerRoll();
        }
    }

    void OnPlayerRoll()
    {
        int dice = UnityEngine.Random.Range(1, 7);

        guard.diceText.text = string.Format("You rolled <color=#880000>{0}</color>!", dice.ToString());

        int targetSpot = (players[currentPlayer].spot + dice) % boardSpots.Count;

        players[currentPlayer].SetTargetSpot(targetSpot);

        state = State.PLAYER_MOVING;
    }

    void OnPlayerMovingEnd()
    {
        minigameTriggerSpot = 0;
        TriggerMinigameOnSpot();
    }

    void TriggerMinigameOnSpot()
    {
        for (; minigameTriggerSpot < boardSpots.Count; minigameTriggerSpot++)
        {
            minigameTriggerPlayers = new List<Player>();

            for (int j = 0; j < players.Count; ++j)
            {
                if (players[j].spot == minigameTriggerSpot)
                {
                    minigameTriggerPlayers.Add(players[j]);
                }
            }

            if (minigameTriggerPlayers.Count > 1)
            {
                state = State.MINIGAME;

                TaleExtra.DisableInput();
                TaleExtra.RipOut();
                Tale.Scene("Barbut");
                TaleExtra.RipIn();
                TaleExtra.EnableInput();

                return;
            }
        }

        guard.diceText.text = "Press <color=#880000>space</color> to roll!";
    }
}
