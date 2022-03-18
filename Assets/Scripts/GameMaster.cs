using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class GameMaster : MonoBehaviour
{
    public enum State
    {
        IDLE,
        PLAYER_MOVING,
        MINIGAME,
        SPOT_MINIGAME_FINISHED
    }

    public enum Minigame
    {
        DUMMY
    }

    public static bool INPUT_ENABLED = true;

    public static GameMaster INSTANCE = null;

    public List<PlayerData> playerData;

    [NonSerialized]
    public GameGuard guard = null; // Holds stuff specific to the Game scene

    [NonSerialized]
    public int minigameTriggerSpot; // The spot where the minigame was triggered
    [NonSerialized]
    public List<PlayerData> minigameTriggerPlayers; // The players on that spot;
    [NonSerialized]
    public int rigDice = -1; // If it's between 1 and 6 then the dice will always yield this value

    int currentPlayer;
    State state;

    void Awake()
    {
        if(INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }

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
                if(guard.players[currentPlayer].state == Player.State.IDLE)
                {
                    state = State.IDLE;
                    currentPlayer = (currentPlayer + 1) % guard.players.Count;
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

    public static bool InputEnabled()
    {
        return INPUT_ENABLED && !Backport.IsOpen();
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
        if (InputEnabled() && Input.GetKeyDown("space"))
        {
            OnPlayerRoll();
        }
    }

    void OnPlayerRoll()
    {
        int dice = (rigDice >= 1 && rigDice <= 6) ? rigDice : UnityEngine.Random.Range(1, 7);

        guard.diceText.text = string.Format("You rolled <color=#880000>{0}</color>!", dice.ToString());

        //int targetSpot = (playerData[currentPlayer].spot + dice) % guard.boardSpots.Count;

        guard.players[currentPlayer].SetInMotion(dice);

        state = State.PLAYER_MOVING;
    }

    void OnPlayerMovingEnd()
    {
        minigameTriggerSpot = 0;
        TriggerMinigameOnSpot();
    }

    void TriggerMinigameOnSpot()
    {
        for (; minigameTriggerSpot < guard.boardSpots.Count; minigameTriggerSpot++)
        {
            minigameTriggerPlayers = new List<PlayerData>();

            for (int j = 0; j < playerData.Count; ++j)
            {
                if (playerData[j].spot == minigameTriggerSpot)
                {
                    minigameTriggerPlayers.Add(playerData[j]);
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
