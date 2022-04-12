using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UwUBridgesMaster : MinigameMaster
{

    private int[] x_sc={1,7,13,1,7,13,1,7,13};
    private int[] y_sc={1,1,1,7,7,7,13,13,13};

    private int ROUNDS_REMAINING = 3;

    private float ROUND_MAX_TIME = 10f;

    bool roundPlaying = true;

    private float PAUSE_TIME = 3f;

    private float elapsedTime = 0f;

    private const int ARENA_SIZE = 15;

    private const int padding = 2;

    private const int LIGHT_DISTANCE = 2;

    private bool gameDone = false;

    private readonly Vector2Int[]
        startPositions =
        {
            new Vector2Int(6, 6),
            new Vector2Int(8, 8),
            new Vector2Int(6, 7),
            new Vector2Int(6, 8),
            new Vector2Int(7, 6),
            new Vector2Int(7, 8),
            new Vector2Int(8, 7),
            new Vector2Int(8, 6)
        };

    public enum Squares : int
    {
        NORD = 0,
        EST = 1,
        SUD = 2,
        WEST = 3,
        NORD_EST = 4,
        NORD_WEST = 5,
        SUD_EST = 6,
        SUD_WEST = 7
    }

    private float minX;

    private float minY;

    private float squareSize;

    [NonSerialized]
    public List<UwUBridgesPlayer> players;

    [SerializeField]
    RectTransform canvasTransform;

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    GameObject squarePrefab;


    int[,] arena;

    GameObject[,] arenaSquares;
    [NonSerialized]
    public GameObject[] squareScores;


    private Vector3 GetPosFromLineCol(int i, int j)
    {
        return new Vector3(minX + j * (squareSize + padding),
            minY + i * (squareSize + padding),
            0);
    }

        private Vector3 GetPosForScores(int i, int j)
    {
        return new Vector3(minX + j * (squareSize + padding)+93,
            minY + i * (squareSize + padding)-12,
            0);
    }

    void Awake()
    {
        InitInputMaster();

        Vector3[] corners = new Vector3[4];
        canvasTransform.GetLocalCorners (corners);
        minX = corners[0].x;
        minY = corners[0].y;
        float maxX = corners[2].x;
        float maxY = corners[2].y;

        float sizeX = maxX - minX;
        float sizeY = maxY - minY;
        squareSize =
            (Math.Min(sizeX, sizeY) - (ARENA_SIZE - 1) * padding) / ARENA_SIZE;
        minX += squareSize / 2;
        minY += squareSize / 2;
        if (sizeX > sizeY)
            minX += (sizeX - sizeY) / 2;
        else
            minY += (sizeY - sizeX) / 2;

        players = new List<UwUBridgesPlayer>();
        arena = new int[ARENA_SIZE, ARENA_SIZE];
        arenaSquares = new GameObject[ARENA_SIZE, ARENA_SIZE];
        squareScores = new GameObject[ARENA_SIZE];

        for (int i = 0; i < ARENA_SIZE; i++)
        {
            for (int j = 0; j < ARENA_SIZE; j++)
            {
                arena[i, j] = -1;
                Vector3 pos = GetPosFromLineCol(i, j);
                arenaSquares[i, j] =
                    Instantiate(squarePrefab, pos, Quaternion.identity);
                arenaSquares[i, j]
                    .GetComponent<RectTransform>()
                    .SetParent(canvasTransform, false);
                arenaSquares[i, j].GetComponent<RectTransform>().sizeDelta =
                    new Vector2(squareSize, squareSize);
                if (
                    ((i > 2 && i < 6) || (i > 8 && i < 12)) ||
                    ((j > 2 && j < 6) || (j > 8 && j < 12))
                )
                {
                    if (i == j)
                    {
                        arenaSquares[i, j].GetComponent<Image>().color =
                            Color.white;
                    }
                    else if (i + j == ARENA_SIZE - 1)
                    {
                        arenaSquares[i, j].GetComponent<Image>().color =
                            Color.white;
                    }
                    else if (
                        i == 1 ||
                        i == 7 ||
                        i == 13 ||
                        j == 1 ||
                        j == 7 ||
                        j == 13
                    )
                    {
                        arenaSquares[i, j].GetComponent<Image>().color =
                            Color.white;
                    }
                    else
                    {
                        arenaSquares[i, j].GetComponent<Image>().color =
                            Color.black;
                    }
                }
                else
                {
                    arenaSquares[i, j].GetComponent<Image>().color =
                        Color.white;
                }
            }
        }
        int score_display_counter=0;
        for (int i = 0; i < 9; i++)
        {
            arenaSquares[x_sc[i], y_sc[i]].GetComponent<Image>().color = Color.gray;
            arena[x_sc[i], y_sc[i]] = UnityEngine.Random.Range(1,8);
            if (x_sc[i]!=7 || y_sc[i]!=7)
            {
                createBridgeScore(x_sc[i], y_sc[i],score_display_counter);
                score_display_counter++;
            }

        }


        for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; i++)
        {
            PlayerData data = GameMaster.INSTANCE.minigamePlayers[i];

            Vector3 playerPos =
                GetPosFromLineCol(startPositions[i].x, startPositions[i].y);

            GameObject obj =
                Instantiate(playerPrefab, playerPos, Quaternion.identity);
            obj.GetComponent<RectTransform>().SetParent(canvasTransform, false);
            obj.GetComponent<RectTransform>().sizeDelta =
                new Vector2(squareSize * 0.9f, squareSize * 0.9f); //20 20
            obj.GetComponent<Image>().color = data.superColor.color;
            players.Add(obj.GetComponent<UwUBridgesPlayer>());
            players[i].pos = startPositions[i];
            players[i].data = data;
        }
    }

    public override void OnPlayerKeyDown(int playerId, KeyCode key)
    {
        // Find the player who pressed the key
        UwUBridgesPlayer player = players.Find((x) => x.data.id == playerId);

        if (!player)
        {
            return;
        }

        // Handle the key
        switch (key)
        {
            case KeyCode.W:
                player.xChosen = 6;
                player.yChosen = 0;
                break;
            case KeyCode.S:
                player.xChosen = -6;
                player.yChosen = 0;
                break;
            case KeyCode.A:
                player.xChosen = 0;
                player.yChosen = -6;
                break;
            case KeyCode.D:
                player.xChosen = 0;
                player.yChosen = 6;
                break;
        }
    }

    void Update()
    {
        if (roundPlaying)
        {
            Debug.Log("Round playing");
            if (gameDone) return;
            elapsedTime += Time.deltaTime;
            Debug.Log("Elapsed time: " + elapsedTime);
            if (elapsedTime > ROUND_MAX_TIME)
            {
                int[] number_of_players_per_bridge = new int[8];

                foreach (UwUBridgesPlayer player in players)
                {
                    if (player.xChosen == 0 && player.yChosen == 0)
                    {
                        // Choose a random direction if none was chosen
                        bool modifyX = UnityEngine.Random.Range(0, 2) == 0;
                        if (modifyX)
                        {
                            bool plusX = UnityEngine.Random.Range(0, 2) == 0;
                            if (plusX)
                            {
                                player.xChosen = 6;
                            }
                            else
                            {
                                player.xChosen = -6;
                            }
                        }
                        else
                        {
                            bool plusY = UnityEngine.Random.Range(0, 2) == 0;
                            if (plusY)
                            {
                                player.yChosen = 6;
                            }
                            else
                            {
                                player.yChosen = -6;
                            }
                        }
                    }

                    if (player.xChosen == 6 && player.yChosen == 0)
                    {
                        number_of_players_per_bridge[(int)(Squares.NORD)]++;
                    }
                    else if (player.xChosen == 0 && player.yChosen == 6)
                    {
                        number_of_players_per_bridge[(int)(Squares.EST)]++;
                    }
                    else if (player.xChosen == -6 && player.yChosen == 0)
                    {
                        number_of_players_per_bridge[(int)(Squares.SUD)]++;
                    }
                    else if (player.xChosen == 0 && player.yChosen == -6)
                    {
                        number_of_players_per_bridge[(int)(Squares.WEST)]++;
                    }

                    player.pos.x += player.xChosen;
                    player.pos.y += player.yChosen;
                    player.GetComponent<RectTransform>().localPosition =
                        GetPosFromLineCol(player.pos.x, player.pos.y);
                }

                foreach (UwUBridgesPlayer player in players)
                {
                    bool addScore = false;
                    if (
                        player.xChosen == 6 &&
                        player.yChosen == 0 &&
                        number_of_players_per_bridge[(int)(Squares.NORD)] == 1
                    )
                    {
                        addScore = true;
                    }
                    else if (
                        player.xChosen == 0 &&
                        player.yChosen == 6 &&
                        number_of_players_per_bridge[(int)(Squares.EST)] == 1
                    )
                    {
                        addScore = true;
                    }
                    else if (
                        player.xChosen == -6 &&
                        player.yChosen == 0 &&
                        number_of_players_per_bridge[(int)(Squares.SUD)] == 1
                    )
                    {
                        addScore = true;
                    }
                    else if (
                        player.xChosen == 0 &&
                        player.yChosen == -6 &&
                        number_of_players_per_bridge[(int)(Squares.WEST)] == 1
                    )
                    {
                        addScore = true;
                    }

                    if (addScore)
                    {
                        Debug
                            .Log("Player " +
                            player.data.id +
                            " scored " +
                            arena[7 + player.xChosen, 7 + player.yChosen]+ "on round " +ROUNDS_REMAINING);
                        player.score +=
                            arena[7 + player.xChosen, 7 + player.yChosen];
                    }
                    else
                    {
                        Debug.Log("Player " + player.data.id + " missed "+ "on round " +ROUNDS_REMAINING);
                    }
                }
                elapsedTime = 0;
                ROUNDS_REMAINING--;
                roundPlaying = false;
               
            }
        }else{
            Debug.Log("Round not playing"+elapsedTime+" "+PAUSE_TIME);
            elapsedTime += Time.deltaTime;
            Debug.Log("Elapsed time: " + elapsedTime);
            if (elapsedTime > PAUSE_TIME){
                int pos_counter=0;
                foreach (UwUBridgesPlayer player in players)
                {
                    player.xChosen = 0;
                    player.yChosen = 0;
                    player.pos = startPositions[pos_counter];
                    player.GetComponent<RectTransform>().localPosition =
                        GetPosFromLineCol(player.pos.x, player.pos.y);
                    ++pos_counter;
                }

                if (ROUNDS_REMAINING == 0)
                {
                    Dictionary<int, int> playerScores =
                        new Dictionary<int, int>();

                    foreach (UwUBridgesPlayer p in players)
                    {
                        playerScores.Add(p.data.id, p.score);
                        Debug
                            .Log("Total score for player " +
                            p.data.id +
                            ": " +
                            p.score);
                    }

                    GameMaster.INSTANCE.minigameScoreboard =
                        new List<PlayerData>();
                    foreach (UwUBridgesPlayer p in players)
                    GameMaster.INSTANCE.minigameScoreboard.Add(p.data);

                    GameMaster
                        .INSTANCE
                        .minigameScoreboard
                        .Sort((p1, p2) =>
                        {
                            if (playerScores[p1.id] == playerScores[p2.id])
                                return p2.id - p1.id;
                            return playerScores[p2.id] - playerScores[p1.id];
                        });
                    gameDone = true;
                    TaleExtra.MinigameScoreboard();
                }

                int score_display_counter=0;
                for (int i = 0; i < 9; i++)
                {
                    arena[x_sc[i], y_sc[i]] = UnityEngine.Random.Range(1,8);
                    if (x_sc[i]!=7 || y_sc[i]!=7)
                    {
                        displayBridgeScore(x_sc[i], y_sc[i],score_display_counter);
                        score_display_counter++;
                    }

                }
                elapsedTime = 0;
                roundPlaying = true;
            }

        }
    }
    public void createBridgeScore(int x,int y,int position){
        GameObject txt = new GameObject();
        txt.AddComponent<TextMeshProUGUI>();
        txt.GetComponent<TextMeshProUGUI>().SetText(arena[x, y].ToString());
        txt.GetComponent<TextMeshProUGUI>().color = Color.red;
        txt.GetComponent<TextMeshProUGUI>().fontSize = 24;
        txt.transform.position = GetPosForScores(x,y);
        txt.transform.SetParent(canvasTransform, false);
        squareScores[position] = txt;
    }

        public void displayBridgeScore(int x,int y,int position){
        squareScores[position].GetComponent<TextMeshProUGUI>().SetText(arena[x, y].ToString());
    }

    public override void OnPlayerMouseClick(int playerId, bool rightClick)
    {
        return;
    }

    public override void OnPlayerMouseMove(int playerId, Vector2 pos)
    {
        return;
    }

    public override void OnPlayerKeyHold(int playerId, KeyCode key)
    {
        return;
    }
}
