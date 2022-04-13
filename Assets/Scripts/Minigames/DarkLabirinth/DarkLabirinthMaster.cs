using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DarkLabirinthMaster : MinigameMaster
{
    private const int ARENA_SIZE_X = 17;

    private const int ARENA_SIZE_Y = 30;

    private const int padding = 1;

    private const int LIGHT_DISTANCE = 2;

    private int playersThatFinished = 0;

    private bool gameDone = false;

    private readonly Vector2Int[]
        startPositions =
        {
            new Vector2Int(0, 0),
            new Vector2Int(2, 0),
            new Vector2Int(6, 0),
            new Vector2Int(8, 0),
            new Vector2Int(10, 0),
            new Vector2Int(12, 0),
            new Vector2Int(14, 0),
            new Vector2Int(16, 0)
        };

    private float elapsedTime = 0f;

    private float timeBeforeSwap = 5f;

    private float totalTimePassed = 0f;

    private float ROUND_MAX_TIME = 90f;

    private bool makeBlack = true;

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

    private GameTimer gameTimer;

    public TextMeshProUGUI timerText;

    private Vector3 GetPosFromLineCol(int i, int j)
    {
        return new Vector3(minX + j * (squareSize + padding),
            minY + i * (squareSize + padding),
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
                Vector3 pos = GetPosFromLineCol(i, j);
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
                GetPosFromLineCol(startPositions[i].x, startPositions[i].y);

            GameObject obj =
                Instantiate(playerPrefab, playerPos, Quaternion.identity);
            obj.GetComponent<RectTransform>().SetParent(canvasTransform, false);
            obj.GetComponent<RectTransform>().sizeDelta =
                new Vector2(squareSize, squareSize); //20 20
            obj.GetComponent<Image>().color = data.superColor.color;
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
                    if (UnityEngine.Random.Range(0, 4) % 4 == 0)
                    {
                        arena[i, j] = 0;
                        arenaSquares[i, j].GetComponent<Image>().color =
                            Color.gray;
                        if (
                            UnityEngine.Random.Range(0, 3) % 3 == 0 &&
                            i > LIGHT_DISTANCE &&
                            i < ARENA_SIZE_X - LIGHT_DISTANCE &&
                            j > LIGHT_DISTANCE &&
                            j < ARENA_SIZE_Y - LIGHT_DISTANCE && !LightNearby(i, j)
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

        gameTimer = new GameTimer(ROUND_MAX_TIME, timerText, finishGameTime);
    }

    public override void OnPlayerKeyHold(int playerId, KeyCode key)
    {
        // Find the player who pressed the key
        DarkLabirinthPlayer player = players.Find((x) => x.data.id == playerId);

        if (!player)
        {
            return;
        }

        float
            dx = 0f,
            dy = 0f;

        // Handle the key
        switch (key)
        {
            case KeyCode.W:
                dx = 0.025f;
                break;
            case KeyCode.S:
                dx = -0.025f;
                break;
            case KeyCode.A:
                dy = -0.025f;
                break;
            case KeyCode.D:
                dy = 0.025f;
                break;
        }
        float auxX=player.pos.x + dx;
        float auxY=player.pos.y + dy;
        int newX = (int)(auxX);
        int newY = (int)(auxY);
        if (newX >= ARENA_SIZE_X || newX < 0 || newY >= ARENA_SIZE_Y || newY < 0
        ) return;
        int col = arena[newX, newY];
        if (col != 0)
        {
            player.pos.x = startPositions[playerId].x;
            player.pos.y = startPositions[playerId].y;
            player.GetComponent<RectTransform>().localPosition =
                GetPosFromLineCol(startPositions[playerId].x,
                startPositions[playerId].y);
            return;
        }

        player.pos.x = auxX;
        player.pos.y = auxY;
        player.GetComponent<RectTransform>().localPosition =
            GetPosFromLineCol(newX, newY);

        if (player.pos.y >= ARENA_SIZE_Y - 2 && player.finishedGame == false)
        {
            playersThatFinished += 1;
            player.finishedGame = true;
        }
    }

    private void finishGameTime()
    {
        Dictionary<int, int> playerScores = new Dictionary<int, int>();
        Debug.Log("Time is up");

        foreach (DarkLabirinthPlayer p in players)
        {
            if (p.finishedGame == false)
            {
                playerScores
                    .Add(p.data.id, 10 + ARENA_SIZE_Y - (int)(p.pos.y));
            }
            else
            {
                playerScores.Add(p.data.id, p.position);
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

    void Update()
    {
        if (gameDone) 
            return;
        gameTimer.Update();
        elapsedTime += Time.deltaTime;
        Debug.Log("Elapsed time: " + elapsedTime);
        if (elapsedTime > timeBeforeSwap)
        {
            totalTimePassed += elapsedTime;
            elapsedTime = 0f;
            if (makeBlack == true)
            {
                Debug.Log("Making black");
                MakeAllBlack();
                makeBlack = false;
                timeBeforeSwap = UnityEngine.Random.Range(5f, 10f);
                Debug.Log("Time to wait: " + timeBeforeSwap);
            }
            else
            {
                Debug.Log("Making normal");
                MakeNormal();
                makeBlack = true;
                timeBeforeSwap = UnityEngine.Random.Range(2f, 5f);
                Debug.Log("Time to wait: " + timeBeforeSwap);
            }
        }
        if (playersThatFinished == players.Count)
        {
            Dictionary<int, int> playerScores = new Dictionary<int, int>();
            Debug.Log("All players finished");
            foreach (DarkLabirinthPlayer p in players)
            {
                playerScores.Add(p.data.id, p.position);
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

    public override void OnPlayerMouseClick(int playerId, bool rightClick)
    {
        return;
    }

    public override void OnPlayerMouseMove(int playerId, Vector2 pos)
    {
        return;
    }

    public override void OnPlayerKeyDown(int playerId, KeyCode key)
    {
        return;
    }

    private void LightNeighbours(int x, int y)
    {
        int startX = x - LIGHT_DISTANCE;
        int startY = y - LIGHT_DISTANCE;

        int stopX = x + LIGHT_DISTANCE;
        int stopY = y + LIGHT_DISTANCE;
        for (int i = startX; i < stopX-1; i++)
        {
            for (int j = startY; j < stopY-1; j++)
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

    public bool LightNearby(int x,int y){
        int startX = x - LIGHT_DISTANCE;
        int startY = y - LIGHT_DISTANCE;

        int stopX = x + LIGHT_DISTANCE;
        int stopY = y + LIGHT_DISTANCE;
        for (int i = startX; i < stopX; i++)
        {
            for (int j = startY; j < stopY; j++)
            {
                if (i >= 0 && i < ARENA_SIZE_X && j >= 0 && j < ARENA_SIZE_Y && arena[i,j]==1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void MakeAllBlack()
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
            LightNeighbours(x, y);
        }
    }

    void MakeNormal()
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
}
