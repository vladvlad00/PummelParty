using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FallingFloorMaster : MinigameMaster
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

            players.Add(obj.GetComponent<FallingFloorPlayer>());
            players[i].data = data;
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
