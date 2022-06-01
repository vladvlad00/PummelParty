using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StackingBoxesMaster : MinigameMaster
{
    private const int ARENA_SIZE = 5;
    private const int padding = 2;
    private const float SPEED = 0.3f;
    private const float ACCELERATION = 0.85f;

    private float minX;
    private float minY;
    private float squareSize;
    [NonSerialized]
    public List<StackingBoxesPlayer> players;
    [SerializeField]
    RectTransform canvasTransform;
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject squarePrefab;

    bool finished = false;

    int[,] arena;
    GameObject[,] arenaSquares;
    float[] speeds;
    float[] elapsedTimes;
    int[] directions;
    int[] falling;

    private Vector3 getPosFromLineCol(int i, int j)
    {
        return new Vector3(minX + j * (squareSize + padding), minY + i * (squareSize + padding), 0);
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
        int squareNum = (ARENA_SIZE + 1) * GameMaster.INSTANCE.minigamePlayers.Count - 1;
        squareSize = Math.Min((sizeX - (squareNum - 1) * padding) / squareNum, (sizeY - (ARENA_SIZE - 1) * padding) / ARENA_SIZE);
        minX += squareSize / 2;
        minY += squareSize / 2;
        minY -= (sizeY - (squareSize - 1) * padding - squareSize * ARENA_SIZE) / 2;

        speeds = new float[GameMaster.INSTANCE.minigamePlayers.Count];
        elapsedTimes = new float[GameMaster.INSTANCE.minigamePlayers.Count];
        directions = new int[GameMaster.INSTANCE.minigamePlayers.Count];
        falling = new int[GameMaster.INSTANCE.minigamePlayers.Count];
        for (int i = 0; i < speeds.Length; i++)
        {
            speeds[i] = SPEED;
            elapsedTimes[i] = 0;
            directions[i] = 1;
            falling[i] = -1;
        }

        players = new List<StackingBoxesPlayer>();
        arena = new int[ARENA_SIZE, squareNum];
        arenaSquares = new GameObject[ARENA_SIZE, squareNum];
        for (int i = 0; i < ARENA_SIZE; i++)
        {
            for (int j = 0; j < squareNum; j++)
            {
                Vector3 pos = getPosFromLineCol(i, j);
                arenaSquares[i, j] = Instantiate(squarePrefab, pos, Quaternion.identity);
                arenaSquares[i, j].GetComponent<RectTransform>().SetParent(canvasTransform, false);
                arenaSquares[i, j].GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize, squareSize);
                if (j % ARENA_SIZE + 1 == j / ARENA_SIZE)
                {
                    arenaSquares[i, j].GetComponent<Image>().color = Color.grey;
                    arena[i, j] = -2;
                }
                else
                    arena[i, j] = -1;
            }
        }

        for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; i++)
        {
            PlayerData data = GameMaster.INSTANCE.minigamePlayers[i];

            int line = ARENA_SIZE - 1;
            int col = i * (ARENA_SIZE + 1);
            arenaSquares[line, col].GetComponent<Image>().color = data.superColor.color;
            arena[line, col] = data.id;
            GameObject obj = Instantiate(playerPrefab);
            players.Add(obj.GetComponent<StackingBoxesPlayer>());
            players[i].data = data;
        }
    }

    void moveBox(int i, int x, int y)
    {
        int next = y + directions[i];
        if (next < 0 || next >= arena.GetLength(1) || arena[x, next] == -2)
            directions[i] = -directions[i];
        next = y + directions[i];
        arena[x, next] = arena[x, y];
        arena[x, y] = -1;
        arenaSquares[x, next].GetComponent<Image>().color = arenaSquares[x, y].GetComponent<Image>().color;
        arenaSquares[x,y].GetComponent<Image>().color = Color.white;
    }

    void gameDone(int i)
    {
        finished = true;
        GameMaster.INSTANCE.minigameScoreboard = new List<PlayerData>();
        GameMaster.INSTANCE.minigameScoreboard.Add(players[i].data);
        for (int j = 0; j < players.Count; j++)
            if (i != j)
                GameMaster.INSTANCE.minigameScoreboard.Add(players[j].data);

        TaleExtra.MinigameScoreboard();
    }

    void fallBox(int i, int x, int y)
    {
        int next = x - 1;
        if (next < 0 || arena[next, y] != -1)
        {
            if (x == ARENA_SIZE - 1)
            {
                gameDone(i);
                return;
            }
            speeds[i] *= ACCELERATION;
            falling[i] = -1;
            int line = ARENA_SIZE - 1;
            int col = i * (ARENA_SIZE + 1);
            arenaSquares[line, col].GetComponent<Image>().color = players[i].data.superColor.color;
            arena[line, col] = players[i].data.id;
            return;
        }
        arena[next, y] = arena[x, y];
        arena[x, y] = -1;
        arenaSquares[next, y].GetComponent<Image>().color = arenaSquares[x, y].GetComponent<Image>().color;
        arenaSquares[x, y].GetComponent<Image>().color = Color.white;
    }

    private void Update()
    {
        if (finished)
            return;
        for (int i = 0; i < elapsedTimes.Length; i++)
        {
            elapsedTimes[i] += Time.deltaTime;
            if (elapsedTimes[i] > speeds[i])
            {
                int left = i * (ARENA_SIZE + 1);
                int right = left + ARENA_SIZE - 1;
                if (falling[i] == -1)
                {
                    for (int j = left; j <= right; j++)
                        if (arena[ARENA_SIZE - 1, j] != -1)
                        {
                            moveBox(i, ARENA_SIZE - 1, j);
                            break;
                        }
                }
                else
                {
                    for (int j = ARENA_SIZE - 1; j >= 0; j--)
                    {
                        if (arena[j, falling[i]] != -1)
                        {
                            fallBox(i, j, falling[i]);
                            break;
                        }
                    }
                }
                elapsedTimes[i] = 0;
            }
        }
    }

    public override void OnPlayerKeyDown(int playerId, KeyCode key)
    {
        // Find the player who pressed the key
        StackingBoxesPlayer player = players.Find((x) => x.data.id == playerId);

        if (player == null)
        {
            return;
        }

        int left = player.data.id * (ARENA_SIZE + 1);
        int right = left + ARENA_SIZE - 1;
        for (int j = left; j <= right; j++)
            if (arena[ARENA_SIZE - 1, j] != -1)
            {
                falling[player.data.id] = j;
                break;
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

    public override void OnPlayerKeyHold(int playerId, KeyCode key)
    {
        return;
    }
}
