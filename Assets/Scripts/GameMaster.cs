using System;
using System.Collections.Generic;
using UnityEngine;

// Should execute before GameGuard
[DefaultExecutionOrder(0)]
public class GameMaster : MonoBehaviour
{
    public enum State
    {
        IDLE,
        PLAYER_MOVING,
        PLAYER_SELECTING_SPOT,
        MINIGAME,
        SPOT_MINIGAME,
        SPOT_MINIGAME_FINISHED
    }

    public enum Minigame
    {
        // Inconsistent with UPPER_SNAKE_CASE, but needed since ToString() has to yield a valid Scene name
        // (HopRace, because HOP_RACE is a weird scene name)
        HopRace,
        MeteorRain,
        Baseball
    }

    public static bool INPUT_ENABLED = true;

    public static GameMaster INSTANCE = null;

    public List<PlayerData> playerData;

    [NonSerialized]
    public GameGuard guard = null; // Holds stuff specific to the Game scene

    [NonSerialized]
    public int minigameTriggerSpot; // The spot where the minigame was triggered
    [NonSerialized]
    public List<PlayerData> minigamePlayers;
    [NonSerialized]
    public int rigDice; // If it's between 1 and 6 then the dice will always yield this value
    [NonSerialized]
    public string rigMinigame; // If it's set to a valid minigame, that minigame will always be selected
    [NonSerialized]
    public bool moveDirectionReversed;

    [NonSerialized]
    public List<PlayerData> minigameScoreboard;

    [NonSerialized]
    public Spot lastSelectedSpot;

    [NonSerialized]
    public State state;
    [NonSerialized]
    public Minigame minigame;

    int currentPlayer;

    void Awake()
    {
        if(INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }

        rigDice = -1;
        rigMinigame = null;
        moveDirectionReversed = false;

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
                    // Next turn
                    currentPlayer = (currentPlayer + 1) % guard.players.Count;
                    OnPlayerMovingEnd();
                }
                break;
            case State.MINIGAME:
            case State.SPOT_MINIGAME:
                break;
            case State.SPOT_MINIGAME_FINISHED:
                TriggerMinigameOnSpot(); // Check for additional spots with multiple players

                if(state == State.SPOT_MINIGAME_FINISHED)
                {
                    state = State.IDLE;
                }
                break;
            default:
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

        if(moveDirectionReversed)
        {
            ReverseMoveDirection(); // The board isn't reversed, while moveDirectionReserved is true, so fix that
            moveDirectionReversed = true;
        }

        if(state == State.SPOT_MINIGAME)
        {
            OnSpotMinigameEnd();
        }
        else
        {
            state = State.IDLE;
        }
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

        state = State.PLAYER_MOVING;

        guard.players[currentPlayer].SetInMotion(dice);
    }

    // The Player script will monitor the master state, so we don't have to do anything else.
    public void OnPlayerSelectingSpot()
    {
        state = State.PLAYER_SELECTING_SPOT;
    }

    public void OnPlayerSelectedSpot(Spot spot)
    {
        lastSelectedSpot = spot;
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
            minigamePlayers = new List<PlayerData>();

            for (int j = 0; j < playerData.Count; ++j)
            {
                if (playerData[j].spot == minigameTriggerSpot)
                {
                    minigamePlayers.Add(playerData[j]);
                }
            }

            if (minigamePlayers.Count > 1)
            {
                Minigame minigame = Minigame.HopRace;

                // If it's not rigged, choose a random minigame. Otherwise, TryParse will set the rigged minigame
                if(string.IsNullOrWhiteSpace(rigMinigame) || !Enum.TryParse<Minigame>(rigMinigame, true, out minigame))
                {
                    Array pool = Enum.GetValues(typeof(Minigame));
                    minigame = (Minigame)pool.GetValue(UnityEngine.Random.Range(0, pool.Length));
                }

                TriggerMinigame(minigame, minigamePlayers);
                return;
            }
        }

        guard.diceText.text = "Press <color=#880000>space</color> to roll!";
    }

    public void TriggerMinigame(Minigame minigame, List<PlayerData> players)
    {
        this.minigame = minigame;
        minigamePlayers = players;

        state = State.MINIGAME;

        TaleExtra.DisableInput();
        TaleExtra.RipOut();
        Tale.Scene(string.Format("Scenes/Minigames/{0}", minigame.ToString()));
        TaleExtra.RipIn();
        TaleExtra.EnableInput();
    }

    // Reverse the direction in which the players move
    public void ReverseMoveDirection()
    {
        Dictionary<int, HashSet<int>> processedSpots = new Dictionary<int, HashSet<int>>();
        Queue<Spot> queue = new Queue<Spot>();

        queue.Enqueue(guard.startingSpot);

        while (queue.Count > 0)
        {
            if(!processedSpots.ContainsKey(queue.Peek().index))
            {
                processedSpots[queue.Peek().index] = new HashSet<int>();
            }

            for(int i = 0; i < queue.Peek().next.Count;)
            {
                Spot spot = queue.Peek().next[i];

                if(!processedSpots[queue.Peek().index].Contains(spot.index))
                {
                    queue.Enqueue(spot);

                    Debug.Assert(!spot.next.Contains(queue.Peek()), "2-node cycle detected; two spots cannot have each other as the next");
                    spot.next.Add(queue.Peek());

                    queue.Peek().next.RemoveAt(i);

                    if (!processedSpots.ContainsKey(spot.index))
                    {
                        processedSpots[spot.index] = new HashSet<int>();
                    }

                    processedSpots[spot.index].Add(queue.Peek().index);
                }
                else
                {
                    ++i;
                }
            }

            queue.Dequeue();
        }

        moveDirectionReversed = !moveDirectionReversed;
    }
}
