using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ColorRunMaster : MinigameMaster
{
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
    private int freeSquares = ARENA_SIZE * ARENA_SIZE;
    [NonSerialized]
    public List<ColorRunPlayer> players;
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
        squareSize = (Math.Min(sizeX, sizeY) - (ARENA_SIZE - 1) * padding) / ARENA_SIZE;
        minX += squareSize / 2;
        minY += squareSize / 2;
        if (sizeX > sizeY)
            minX += (sizeX - sizeY) / 2;
        else
            minY += (sizeY - sizeX) / 2;

        players = new List<ColorRunPlayer>();
        arena = new int[ARENA_SIZE, ARENA_SIZE];
        arenaSquares = new GameObject[ARENA_SIZE, ARENA_SIZE];
        for (int i = 0; i < ARENA_SIZE; i++)
        {
            for (int j = 0; j < ARENA_SIZE; j++)
            {
                arena[i, j] = -1;
                Vector3 pos = getPosFromLineCol(i, j);
                arenaSquares[i, j] = Instantiate(squarePrefab, pos, Quaternion.identity);
                arenaSquares[i, j].GetComponent<RectTransform>().SetParent(canvasTransform, false);
                arenaSquares[i, j].GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize, squareSize);
            }
        }

        for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; i++)
        {
            PlayerData data = GameMaster.INSTANCE.minigamePlayers[i];

            arenaSquares[startPositions[i].x, startPositions[i].y].GetComponent<Image>().color = data.color;
            freeSquares--;
            arena[startPositions[i].x, startPositions[i].y] = data.id;
            Vector3 playerPos = getPosFromLineCol(startPositions[i].x, startPositions[i].y);

            GameObject obj = Instantiate(playerPrefab, playerPos, Quaternion.identity);
            obj.GetComponent<RectTransform>().SetParent(canvasTransform, false);
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);

            players.Add(obj.GetComponent<ColorRunPlayer>());
            players[i].pos = startPositions[i];
            players[i].data = data;
        }
    }

    public override void OnPlayerKeyDown(int playerId, KeyCode key)
    {
        // Find the player who pressed the key
        ColorRunPlayer player = players.Find((x) => x.data.id == playerId);

        if (!player)
        {
            return;
        }

        int dx = 0, dy = 0;
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
        if (newX >= ARENA_SIZE || newX < 0 || newY >= ARENA_SIZE || newY < 0)
            return;
        int col = arena[newX, newY];
        if (col != -1 && col != player.data.id)
            return;
        player.pos.x = newX;
        player.pos.y = newY;

        if (col == -1)
            freeSquares--;
        player.GetComponent<RectTransform>().localPosition = getPosFromLineCol(newX, newY);
        arena[newX, newY] = player.data.id;
        arenaSquares[newX, newY].GetComponent<Image>().color = player.data.color;
        Debug.Log(freeSquares + " squares left");

        if (freeSquares == 0)
        {
            Dictionary<int, int> playerScores = new Dictionary<int, int>();
            foreach (ColorRunPlayer p in players)
                playerScores.Add(p.data.id, 0);
            for (int i = 0; i < ARENA_SIZE; i++)
                for (int j = 0; j < ARENA_SIZE; j++)
                    playerScores[arena[i, j]]++;
            GameMaster.INSTANCE.minigameScoreboard = new List<PlayerData>();
            foreach (ColorRunPlayer p in players)
                GameMaster.INSTANCE.minigameScoreboard.Add(p.data);

            GameMaster.INSTANCE.minigameScoreboard.Sort((p1, p2) =>
            {
                if (playerScores[p1.id] == playerScores[p2.id])
                    return p2.id - p1.id;
                return playerScores[p2.id] - playerScores[p1.id];
            });

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

    public override void OnPlayerKeyHold(int playerId, KeyCode key)
    {
        return;
    }
}
