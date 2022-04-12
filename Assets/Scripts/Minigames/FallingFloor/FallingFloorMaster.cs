using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FallingFloorMaster : MinigameMaster
{
    private const float ROUND_TIME = 90f;
    private const float ACCELERATION = 0.9f;

    private float remainingTime = 0f;
    private float fallInterval = 7f;
    private float fallDuration = 4f;
    private float elapsedTime = 0f;
    private bool gameDone = false;
    private bool falling = false;
    private int round = 0;
    private int activePlayers;

    private List<GameObject> selectedSquares = new List<GameObject>();
    private List<PlayerData> scoreboard = new List<PlayerData>();

    private const int ARENA_SIZE = 10;
    private const int padding = 2;
    private readonly Vector2Int[] startPositions =
    {
        new Vector2Int(0, 0),
        new Vector2Int(ARENA_SIZE - 1, ARENA_SIZE - 1),
        new Vector2Int(ARENA_SIZE - 1, 0),
        new Vector2Int(0, ARENA_SIZE - 1),
        new Vector2Int(0, ARENA_SIZE / 2),
        new Vector2Int(ARENA_SIZE - 1, ARENA_SIZE / 2),
        new Vector2Int(ARENA_SIZE / 2, 0),
        new Vector2Int(ARENA_SIZE / 2, ARENA_SIZE - 1)
    };

    private float minX;
    private float minY;
    private float squareSize;

    [NonSerialized]
    public List<FallingFloorPlayer> players;
    [SerializeField]
    RectTransform canvasTransform;
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject squarePrefab;
    [SerializeField]
    GameObject wallPrefab;

    int[,] arena;
    GameObject[,] arenaSquares;

    private Vector3 getPosFromLineCol(int i, int j, bool isWall = false)
    {
        if (!isWall)
            return new Vector3(minX + (j + 1) * (squareSize + padding), minY + (i + 1) * (squareSize + padding), 0);
        return new Vector3(minX + j * (squareSize + padding), minY + i * (squareSize + padding), 0);
    }

    void Awake()
    {
        InitInputMaster();

        remainingTime = ROUND_TIME;

        Vector3[] corners = new Vector3[4];
        canvasTransform.GetLocalCorners(corners);
        minX = corners[0].x;
        minY = corners[0].y;
        float maxX = corners[2].x;
        float maxY = corners[2].y;
        float sizeX = maxX - minX;
        float sizeY = maxY - minY;
        squareSize = (Math.Min(sizeX, sizeY) - (ARENA_SIZE + 1) * padding) / (ARENA_SIZE + 2);
        minX += squareSize / 2;
        minY += squareSize / 2;
        if (sizeX > sizeY)
            minX += (sizeX - sizeY) / 2;
        else
            minY += (sizeY - sizeX) / 2;

        players = new List<FallingFloorPlayer>();
        arena = new int[ARENA_SIZE, ARENA_SIZE];
        arenaSquares = new GameObject[ARENA_SIZE, ARENA_SIZE];
        for (int i = 0; i < ARENA_SIZE; i++)
        {
            for (int j = 0; j < ARENA_SIZE; j++)
            {
                arena[i, j] = 0;
                Vector3 pos = getPosFromLineCol(i, j);
                arenaSquares[i, j] = Instantiate(squarePrefab, pos, Quaternion.identity);
                arenaSquares[i, j].GetComponent<RectTransform>().SetParent(canvasTransform, false);
                arenaSquares[i, j].GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize, squareSize);
                arenaSquares[i, j].GetComponent<BoxCollider2D>().size = new Vector2(squareSize / 4, squareSize / 4);
            }
        }

        for (int i = 0; i < ARENA_SIZE + 2; i++)
        {
            Vector3 pos = getPosFromLineCol(i, 0, true);
            GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity);
            wall.GetComponent<RectTransform>().SetParent(canvasTransform, false);
            wall.GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize, squareSize);
            wall.GetComponent<BoxCollider2D>().size = new Vector2(squareSize, squareSize);

            pos = getPosFromLineCol(i, ARENA_SIZE + 1, true);
            wall = Instantiate(wallPrefab, pos, Quaternion.identity);
            wall.GetComponent<RectTransform>().SetParent(canvasTransform, false);
            wall.GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize, squareSize);
            wall.GetComponent<BoxCollider2D>().size = new Vector2(squareSize, squareSize);

            pos = getPosFromLineCol(0, i, true);
            wall = Instantiate(wallPrefab, pos, Quaternion.identity);
            wall.GetComponent<RectTransform>().SetParent(canvasTransform, false);
            wall.GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize, squareSize);
            wall.GetComponent<BoxCollider2D>().size = new Vector2(squareSize, squareSize);

            pos = getPosFromLineCol(ARENA_SIZE + 1, i, true);
            wall = Instantiate(wallPrefab, pos, Quaternion.identity);
            wall.GetComponent<RectTransform>().SetParent(canvasTransform, false);
            wall.GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize, squareSize);
            wall.GetComponent<BoxCollider2D>().size = new Vector2(squareSize, squareSize);
        }

        for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; i++)
        {
            PlayerData data = GameMaster.INSTANCE.minigamePlayers[i];

            Vector3 playerPos = getPosFromLineCol(startPositions[i].x, startPositions[i].y);

            GameObject obj = Instantiate(playerPrefab, playerPos, Quaternion.identity);
            obj.GetComponent<RectTransform>().SetParent(canvasTransform, false);
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize * 0.8f, squareSize * 0.8f);
            obj.GetComponent<BoxCollider2D>().size = new Vector2(squareSize * 0.8f, squareSize * 0.8f);
            obj.GetComponent<Image>().color = data.superColor.color;

            players.Add(obj.GetComponent<FallingFloorPlayer>());
            players[i].data = data;
        }
        activePlayers = GameMaster.INSTANCE.minigamePlayers.Count;
    }

    private void selectParity(int round)
    {
        int parity = UnityEngine.Random.Range(0, 2);
        for (int i = 0; i < ARENA_SIZE; i++)
            for (int j = 0; j < ARENA_SIZE; j++)
                if ((i + j) % 2 == parity)
                    selectedSquares.Add(arenaSquares[i, j]);
    }
    private void selectLines(int round)
    {
        int selected = (1 + round) / 2;
        bool[,] lines = new bool[2,ARENA_SIZE];
        while (selected > 0)
        {
            int dir = UnityEngine.Random.Range(0, 2);
            int pos = UnityEngine.Random.Range(0, ARENA_SIZE);
            if (!lines[dir, pos])
            {
                lines[dir, pos] = true;
                selected--;
            }
        }
        for (int i = 0; i < ARENA_SIZE; i++)
            for (int j = 0; j < ARENA_SIZE; j++)
            {
                if (lines[0, i] || lines[1, j])
                    selectedSquares.Add(arenaSquares[i, j]);
            }
    }
    private void selectQuarters(int round)
    {
        int selected = 1;
        if (round > 5)
            selected++;
        if (round > 8)
            selected++;
        bool[] quarters = new bool[4];
        while (selected > 0)
        {
            int pos = UnityEngine.Random.Range(0, 4);
            if (!quarters[pos])
            {
                quarters[pos] = true;
                selected--;
            }
        }
        if (quarters[0])
        {
            for (int i = 0; i < ARENA_SIZE / 2; i++)
                for (int j = 0; j < ARENA_SIZE / 2; j++)
                    selectedSquares.Add(arenaSquares[i, j]);
        }
        if (quarters[1])
        {
            for (int i = 0; i < ARENA_SIZE / 2; i++)
                for (int j = ARENA_SIZE / 2; j < ARENA_SIZE; j++)
                    selectedSquares.Add(arenaSquares[i, j]);
        }
        if (quarters[2])
        {
            for (int i = ARENA_SIZE / 2; i < ARENA_SIZE; i++)
                for (int j = 0; j < ARENA_SIZE / 2; j++)
                    selectedSquares.Add(arenaSquares[i, j]);
        }
        if (quarters[3])
        {
            for (int i = ARENA_SIZE / 2; i < ARENA_SIZE; i++)
                for (int j = ARENA_SIZE / 2; j < ARENA_SIZE; j++)
                    selectedSquares.Add(arenaSquares[i, j]);
        }
    }

    private void selectPattern(int round)
    {
        Action<int>[] patterns =
        {
            selectLines,
            selectQuarters,
            selectParity // always last
        };

        int right = patterns.Length - 1;
        if (round < 5)
            right--;
        int pos = UnityEngine.Random.Range(0, right);
        patterns[pos](round);
    }

    public void destroyPlayer(GameObject obj)
    {
        FallingFloorPlayer player = obj.GetComponent<FallingFloorPlayer>();
        scoreboard.Add(player.data);
        activePlayers--;
        obj.SetActive(false);
        if (activePlayers == 1)
        {
            gameDone = true;
            for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; ++i)
                if (players[i].isActiveAndEnabled)
                {
                    scoreboard.Add(players[i].data);
                    break;
                }
            scoreboard.Reverse();
            GameMaster.INSTANCE.minigameScoreboard = scoreboard;
            TaleExtra.MinigameScoreboard();
        }
    }

    private void checkPlayers()
    {
        for (int i = 0; i < selectedSquares.Count; i++)
        {
            selectedSquares[i].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (gameDone)
        {
            return;
        }
        float deltaTime = Time.deltaTime;
        remainingTime -= deltaTime;
        elapsedTime += deltaTime;
        if (remainingTime <= 0)
        {
            gameDone = true;
            return;
        }

        if (!falling)
        {
            if (elapsedTime <= fallInterval / 5)
            {
                float color = Math.Min(1, elapsedTime / (fallInterval / 5));
                for (int i = 0; i < selectedSquares.Count; i++)
                {
                    selectedSquares[i].GetComponent<BoxCollider2D>().enabled = false;
                    selectedSquares[i].GetComponent<Image>().color = new Color(color, color, color, 1);
                }
            }
            else
                selectedSquares.Clear();
            if (elapsedTime >= fallInterval)
            {
                round++;
                falling = true;
                elapsedTime = 0;
                selectPattern(round);
            }
        }
        else
        {
            if (elapsedTime >= fallDuration)
            {
                falling = false;
                elapsedTime = 0;
                fallInterval *= ACCELERATION;
                fallDuration *= ACCELERATION;
                checkPlayers();
            }
            else
            {
                float color = Math.Max(0, 1 - elapsedTime / fallDuration);
                for (int i = 0; i < selectedSquares.Count; i++)
                {
                    selectedSquares[i].GetComponent<Image>().color = new Color(color, color, color);
                }
            }
        }
    }

    public override void OnPlayerKeyDown(int playerId, KeyCode key)
    {
        return;
    }

    public override void OnPlayerMouseClick(int playerId)
    {
        return;
    }

    public override void OnPlayerMouseMove(int playerId, Vector2 pos)
    {
        return;
    }

    public override void OnPlayerKeyHold(int playerId, KeyCode key)
    {
        // Find the player who pressed the key
        FallingFloorPlayer player = players.Find((x) => x.data.id == playerId);

        if (!player)
        {
            return;
        }

        int dx = 0, dy = 0;
        // Handle the key
        switch (key)
        {
            case KeyCode.W:
                dy = 1;
                break;
            case KeyCode.S:
                dy = -1;
                break;
            case KeyCode.A:
                dx = -1;
                break;
            case KeyCode.D:
                dx = 1;
                break;
        }
        player.rigidBody.AddForce(new Vector2(dx, dy), ForceMode2D.Impulse);
    }
}
