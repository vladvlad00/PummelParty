using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkLabirinthMaster : MinigameMaster
{
    private const int ARENA_SIZE_X = 25;

    private const int ARENA_SIZE_Y = 45;

    private const int padding = 1;

    private const int light_distance = 2;

    private int players_that_finished = 0;

    private bool gameDone = false;

    private readonly Vector2Int[]
        startPositions =
        {
            new Vector2Int(0, 0),
            new Vector2Int(4, 0),
            new Vector2Int(8, 0),
            new Vector2Int(12, 0),
            new Vector2Int(16, 0),
            new Vector2Int(20, 0),
            new Vector2Int(24, 0),
            new Vector2Int(28, 0)
        };

    private float elapsedTime = 0f;

    private float time_before_swap = 5f;

    private float total_time_passed = 0f;

    private float round_max_time = 10f;

    private bool make_black = true;

    private float minX;

    private float minY;

    private float squareSize;

    [NonSerialized]
    public List<DarkLabirinthPlayer> players;

    [SerializeField]
    RectTransform canvasTransform;

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    GameObject squarePrefab;

    int[,] arena;

    GameObject[,] arenaSquares;

    private Vector3 getPosFromLineCol(int i, int j)
    {
        return new Vector3(minX + j * (squareSize + padding),
            minY + i * (squareSize + padding),
            0);
    }

    void Awake()
    {
        InitInputMaster();

        Vector3[] corners = new Vector3[4];
        canvasTransform.GetLocalCorners(corners);
        minX = corners[0].x;
        minY = corners[0].y;
        float maxX = corners[2].x;
        float maxY = corners[2].y;

        float sizeX = maxX - minX;
        float sizeY = maxY - minY;
        squareSize =
            (Math.Min(sizeX, sizeY) - (ARENA_SIZE_X - 1) * padding) /
            ARENA_SIZE_X;
        minX += squareSize / 2;
        minY += squareSize / 2;

        players = new List<DarkLabirinthPlayer>();
        arena = new int[ARENA_SIZE_X, ARENA_SIZE_Y];
        arenaSquares = new GameObject[ARENA_SIZE_X, ARENA_SIZE_Y];

        for (int i = 0; i < ARENA_SIZE_X; i++)
        {
            for (int j = 0; j < ARENA_SIZE_Y; j++)
            {
                arena[i, j] = -1;
                Vector3 pos = getPosFromLineCol(i, j);
                arenaSquares[i, j] =
                    Instantiate(squarePrefab, pos, Quaternion.identity);
                arenaSquares[i, j]
                    .GetComponent<RectTransform>()
                    .SetParent(canvasTransform, false);
                arenaSquares[i, j].GetComponent<RectTransform>().sizeDelta =
                    new Vector2(squareSize, squareSize);
                if (j < 2 || j > ARENA_SIZE_Y - 3)
                {
                    arena[i, j] = 0;
                    arenaSquares[i, j].GetComponent<Image>().color = Color.gray;
                }
                else
                {
                    arenaSquares[i, j].GetComponent<Image>().color =
                        Color.black;
                }
            }
        }

        for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; i++)
        {
            PlayerData data = GameMaster.INSTANCE.minigamePlayers[i];

            Vector3 playerPos =
                getPosFromLineCol(startPositions[i].x, startPositions[i].y);

            GameObject obj =
                Instantiate(playerPrefab, playerPos, Quaternion.identity);
            obj.GetComponent<RectTransform>().SetParent(canvasTransform, false);
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize, squareSize); //20 20

            players.Add(obj.GetComponent<DarkLabirinthPlayer>());
            players[i].pos = startPositions[i];
            players[i].data = data;
        }

        var currentX = UnityEngine.Random.Range(0, ARENA_SIZE_X);
        var currentY = 0;
        var prevX = currentX;
        var prevY = currentY;
        while (currentY != ARENA_SIZE_Y - 1)
        {
            prevY = currentY;
            currentY = UnityEngine.Random.Range(prevY + 1, ARENA_SIZE_Y);
            while (currentY - prevY > 7)
            {
                currentY = UnityEngine.Random.Range(prevY + 1, ARENA_SIZE_Y);
            }

            for (int i = prevY; i <= currentY; i++)
            {
                arena[currentX, i] = 0;
                arenaSquares[currentX, i].GetComponent<Image>().color =
                    Color.gray;
            }

            prevX = currentX;
            currentX = UnityEngine.Random.Range(0, ARENA_SIZE_X);
            if (prevX < currentX)
            {
                for (int i = prevX; i <= currentX; i++)
                {
                    arena[i, currentY] = 0;
                    arenaSquares[i, currentY].GetComponent<Image>().color =
                        Color.gray;
                }
            }
            else
            {
                for (int i = currentX; i <= prevX; i++)
                {
                    arena[i, currentY] = 0;
                    arenaSquares[i, currentY].GetComponent<Image>().color =
                        Color.gray;
                }
            }
        }

        for (int i = 0; i < ARENA_SIZE_X; i++)
        {
            for (int j = 0; j < ARENA_SIZE_Y; j++)
            {
                if (arena[i, j] == -1)
                {
                    if (UnityEngine.Random.Range(0, 5) % 5 == 0)
                    {
                        arena[i, j] = 0;
                        arenaSquares[i, j].GetComponent<Image>().color =
                            Color.gray;
                        if (
                            UnityEngine.Random.Range(0, 5) % 5 == 0 &&
                            i > light_distance &&
                            i < ARENA_SIZE_X - light_distance &&
                            j > light_distance &&
                            j < ARENA_SIZE_Y - light_distance
                        )
                        {
                            arena[i, j] = 1;
                            arenaSquares[i, j].GetComponent<Image>().color =
                                Color.yellow;
                        }
                    }
                }
            }
        }
    }

    public override void OnPlayerKeyDown(int playerId, KeyCode key)
    {
        // Find the player who pressed the key
        DarkLabirinthPlayer player = players.Find((x) => x.data.id == playerId);

        if (!player)
        {
            return;
        }

        int
            dx = 0,
            dy = 0;

        // Handle the key
        switch (key)
        {
            case KeyCode.W:
                dx = 1;
                break;
            case KeyCode.S:
                dx = -1;
                break;
            case KeyCode.A:
                dy = -1;
                break;
            case KeyCode.D:
                dy = 1;
                break;
        }
        int newX = player.pos.x + dx;
        int newY = player.pos.y + dy;
        if (newX >= ARENA_SIZE_X || newX < 0 || newY >= ARENA_SIZE_Y || newY < 0
        ) return;
        int col = arena[newX, newY];
        if (col != 0)
        {
            player.pos.x = startPositions[playerId].x;
            player.pos.y = startPositions[playerId].y;
            player.GetComponent<RectTransform>().localPosition =
                getPosFromLineCol(startPositions[playerId].x, startPositions[playerId].y);
            return;
        }

        player.pos.x = newX;
        player.pos.y = newY;
        player.GetComponent<RectTransform>().localPosition =
            getPosFromLineCol(newX, newY);

        if (player.pos.y >= ARENA_SIZE_Y - 2 && player.finishedGame == false)
        {
            players_that_finished += 1;
            player.finishedGame = true;
            player.position = players_that_finished;
        }
    }

    private void light_neighbours(int x, int y, int distance)
    {
        int startX = x - distance;
        int startY = y - distance;

        int stopX = x + distance;
        int stopY = y + distance;
        for (int i = startX; i <= stopX; i++)
        {
            for (int j = startY; j <= stopY; j++)
            {
                if (i >= 0 && i < ARENA_SIZE_X && j >= 0 && j < ARENA_SIZE_Y)
                {
                    if (arena[i, j] == 0)
                    {
                        arenaSquares[i, j].GetComponent<Image>().color =
                            Color.gray;
                    }
                }
            }
        }
    }

    void makeAllBlack()
    {
        List<Vector2> lights = new List<Vector2>();
        for (int i = 0; i < ARENA_SIZE_X; i++)
        {
            for (int j = 0; j < ARENA_SIZE_Y; j++)
            {
                if (arena[i, j] != 1)
                {
                    arenaSquares[i, j].GetComponent<Image>().color =
                        Color.black;
                }
                else
                {
                    lights.Add(new Vector2(i, j));
                }
            }
        }
        for (int i = 0; i < lights.Count; ++i)
        {
            int x = (int)(lights[i].x);
            int y = (int)(lights[i].y);
            light_neighbours(x, y, 3);
        }
    }

    void makeNormal()
    {
        for (int i = 0; i < ARENA_SIZE_X; i++)
        {
            for (int j = 0; j < ARENA_SIZE_Y; j++)
            {
                if (arena[i, j] == -1)
                {
                    arenaSquares[i, j].GetComponent<Image>().color =
                        Color.black;
                }
                else if (arena[i, j] == 1)
                {
                    arenaSquares[i, j].GetComponent<Image>().color =
                        Color.yellow;
                }
                else
                {
                    arenaSquares[i, j].GetComponent<Image>().color = Color.gray;
                }
            }
        }
    }

    void Update()
    {
        if (gameDone)
            return;
        elapsedTime += Time.deltaTime;
        Debug.Log("Elapsed time: " + elapsedTime);
        if (elapsedTime > time_before_swap)
        {
            total_time_passed += elapsedTime;
            elapsedTime = 0f;
            if (make_black == true)
            {
                Debug.Log("Making black");
                makeAllBlack();
                make_black = false;
                time_before_swap = UnityEngine.Random.Range(5f, 10f);
                Debug.Log("Time to wait: " + time_before_swap);
            }
            else
            {
                Debug.Log("Making normal");
                makeNormal();
                make_black = true;
                time_before_swap = UnityEngine.Random.Range(2f, 5f);
                Debug.Log("Time to wait: " + time_before_swap);
            }
        }
        if (
            players_that_finished == players.Count ||
            total_time_passed >= round_max_time
        )
        {
            Dictionary<int, int> playerScores = new Dictionary<int, int>();
            if (players_that_finished == players.Count)
            {
                Debug.Log("All players finished");
                foreach (DarkLabirinthPlayer p in players)
                {
                    playerScores.Add(p.data.id, p.position);
                }
            }
            else
            {
                Debug.Log("Time is up");

                foreach (DarkLabirinthPlayer p in players)
                {
                    if (p.finishedGame == false)
                    {
                        playerScores
                            .Add(p.data.id, 10 + ARENA_SIZE_Y - p.pos.y);
                    }
                    else
                    {
                        playerScores.Add(p.data.id, p.position);
                    }
                }
            }
            GameMaster.INSTANCE.minigameScoreboard = new List<PlayerData>();
            foreach (DarkLabirinthPlayer p in players)
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
    }

    public override void OnPlayerMouseClick(int playerId)
    {
        return;
    }

    public override void OnPlayerMouseMove(int playerId, Vector2 pos)
    {
        return;
    }
}
