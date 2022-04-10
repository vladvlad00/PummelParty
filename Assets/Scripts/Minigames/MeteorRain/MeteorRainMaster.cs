using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MeteorRainMaster : MinigameMaster
{
    private const float PLAYER_Y = -220f;
    private const float METEOR_SPAW = 1f;

    private float elapsedTime = 0f;

    [SerializeField]
    RectTransform canvasTransform;
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject wallPrefab;
    [SerializeField]
    GameObject meteorPrefab;

    [NonSerialized]
    public List<MeteorRainPlayer> players;

    private float playerArenaSize = 0f;
    private float minX = 0f;
    private float maxX = 0f;

    private int activePlayers = 0;
    private List<PlayerData> scoreboard = null;

    void Awake()
    {
        InitInputMaster();

        players = new List<MeteorRainPlayer>();

        Vector3[] corners = new Vector3[4];
        canvasTransform.GetLocalCorners(corners);
        minX = corners[0].x;
        maxX = corners[2].x;
        playerArenaSize = (maxX - minX) / GameMaster.INSTANCE.minigamePlayers.Count;

        Vector3 wallPos = new Vector3(minX, 0f, 0f);
        GameObject obj = Instantiate(wallPrefab, wallPos, Quaternion.identity);
        RectTransform transform = obj.GetComponent<RectTransform>();
        transform.SetParent(canvasTransform, false);

        for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; ++i)
        {
            Vector3 nextWallPos = new Vector3(wallPos.x + playerArenaSize, 0f, 0f);

            obj = Instantiate(wallPrefab, nextWallPos, Quaternion.identity);
            transform = obj.GetComponent<RectTransform>();
            transform.SetParent(canvasTransform, false);

            PlayerData data = GameMaster.INSTANCE.minigamePlayers[i];

            Vector3 playerPos = new Vector3((wallPos.x + nextWallPos.x) / 2, PLAYER_Y, 0f);

            obj = Instantiate(playerPrefab, playerPos, Quaternion.identity);
            obj.GetComponent<Image>().color = data.color;
            transform = obj.GetComponent<RectTransform>();
            transform.SetParent(canvasTransform, false);

            players.Add(obj.GetComponent<MeteorRainPlayer>());
            players[i].data = data;

            wallPos = nextWallPos;
        }

        activePlayers = GameMaster.INSTANCE.minigamePlayers.Count;
        scoreboard = new List<PlayerData>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > METEOR_SPAW)
        {
            elapsedTime = 0;
            for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; ++i)
            {
                if (!players[i].isActiveAndEnabled)
                    continue;
                float x = minX + i * playerArenaSize;
                x += UnityEngine.Random.Range(20f, playerArenaSize - 20f);
                GameObject obj = Instantiate(meteorPrefab, new Vector3(x, 100f, 0f), Quaternion.identity);
                RectTransform transform = obj.GetComponent<RectTransform>();
                transform.SetParent(canvasTransform, false);
            }
        }
    }

    public void destroyPlayer(GameObject obj)
    { 
        MeteorRainPlayer player = obj.GetComponent<MeteorRainPlayer>();
        scoreboard.Add(player.data);
        activePlayers--;
        obj.SetActive(false);
        if (activePlayers == 1)
        {
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

    public override void OnPlayerKeyDown(int playerId, KeyCode key)
    {
        // Find the player who pressed the key
        MeteorRainPlayer player = players.Find((x) => x.data.id == playerId);

        if (!player)
        {
            return;
        }

        // Handle the key
        switch (key)
        {
            case KeyCode.A:
                player.rigidBody.velocity = new Vector2(-MeteorRainPlayer.SPEED, 0f);
                break;
            case KeyCode.D:
                player.rigidBody.velocity = new Vector2(MeteorRainPlayer.SPEED, 0f);
                break;
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
