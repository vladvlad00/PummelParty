using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HopRaceMaster : MinigameMaster
{
    private const float PLAYER_Y_DIFF = 150f;

    [SerializeField]
    RectTransform canvasTransform;
    [SerializeField]
    GameObject playerPrefab;

    [NonSerialized]
    public List<HopRacePlayer> players;

    void Awake()
    {
        InitInputMaster();

        // Get the player IDs from the master and instantiate the minigame player objects
        players = new List<HopRacePlayer>();

        Vector3 basePos = new Vector3(-860f, 460f, 0f);

        for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; ++i)
        {
            PlayerData data = GameMaster.INSTANCE.minigamePlayers[i];

            GameObject obj = Instantiate(playerPrefab, basePos, Quaternion.identity);

            RectTransform transform = obj.GetComponent<RectTransform>();
            transform.SetParent(canvasTransform, false);

            players.Add(obj.GetComponent<HopRacePlayer>());
            players[i].data = data;

            basePos.y -= PLAYER_Y_DIFF;
        }
    }

    void Update()
    {
        
    }

    public override void OnPlayerKeyDown(int playerId, KeyCode key)
    {
        // Find the player who pressed the key
        HopRacePlayer player = players.Find((x) => x.data.id == playerId);

        if(!player)
        {
            return;
        }

        // Handle the key
        switch(key)
        {
            case KeyCode.Space:
                player.rigidBody.velocity = new Vector2(HopRacePlayer.SPEED, 0f);
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
