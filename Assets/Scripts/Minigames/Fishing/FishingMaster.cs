using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class FishingMaster : MinigameMaster
{
    private const float PLAYER_Y = 135f;

    [SerializeField]
    RectTransform canvasTransform;
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject tacklePrefab;
    [SerializeField]
    GameObject fishPrefab;

    [NonSerialized]
    public List<FishingPlayer> players;
    private int activePlayers = 0;

    [NonSerialized]
    public List<GameObject> fish;

    public GameObject timeText;
    public DateTime startTime;
    [NonSerialized]
    public int maxTime = 45;

    [NonSerialized]
    private bool gameEnded = false;

    private GameTimer gameTimer;

    // Start is called before the first frame update
    void Start()
    {
        int noFish = 24;
        InitInputMaster();

        activePlayers = GameMaster.INSTANCE.minigamePlayers.Count;
        //activePlayers = 2;
        players = new List<FishingPlayer>();
        fish = new List<GameObject>();

        for (int i = 0; i < activePlayers; ++i)
        {
            //PlayerData data = new PlayerData("test " + i, GameMaster.playerColors[0]);
            PlayerData data = GameMaster.INSTANCE.minigamePlayers[i];

            Vector3 playerPos = new Vector3(-32 + i * 64, PLAYER_Y, 0f);

            GameObject obj = Instantiate(playerPrefab, playerPos, Quaternion.identity);
            obj.GetComponent<Image>().color = data.superColor.color;
            obj.GetComponent<RectTransform>().SetParent(canvasTransform, false);

            GameObject tkl = Instantiate(tacklePrefab, playerPos, Quaternion.identity);
            tkl.GetComponent<RectTransform>().SetParent(canvasTransform, false);

            obj.GetComponent<FishingPlayer>().tackle = tkl;

            players.Add(obj.GetComponent<FishingPlayer>());
            players[i].data = data;
        }

        for (int i = 0; i < noFish; ++i)
        {
            fish.Add(newFish());
        }

        startTime = DateTime.Now;
        timeText = new GameObject();
        timeText.AddComponent<TextMeshProUGUI>();
        timeText.GetComponent<TextMeshProUGUI>().horizontalAlignment = HorizontalAlignmentOptions.Center;
        timeText.transform.position = new Vector3(0f, 250f, 0f);
        timeText.transform.SetParent(canvasTransform, false);
        gameTimer = new GameTimer(maxTime, timeText.GetComponent<TextMeshProUGUI>(), finishGame);
    }

    GameObject newFish()
    {
        GameObject obj = Instantiate(fishPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        obj.GetComponent<FishingFish>().spawnFish(-480, 480, 80, -250);
        obj.GetComponent<RectTransform>().SetParent(canvasTransform, false);

        return obj;
    }

    int fishIn(float x, float y)
    {
        for (int i = 0; i < fish.Count; ++i)
        {
            if (
                fish[i].GetComponent<FishingFish>().player == -1
                &&
                x + 12 >= fish[i].GetComponent<FishingFish>().transform.localPosition.x && x <= fish[i].GetComponent<FishingFish>().transform.localPosition.x + (30 * fish[i].GetComponent<FishingFish>().size)
                && 
                y + 16 >= fish[i].GetComponent<FishingFish>().transform.localPosition.y && y <= fish[i].GetComponent<FishingFish>().transform.localPosition.y + (21 * fish[i].GetComponent<FishingFish>().size)
                )
            {
                return i;
            }
        }
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnded)
        {
            return;
        }
        gameTimer.Update();

        for (int i = 0; i < activePlayers; ++i)
        {
            if (players[i].transform.localPosition.x <= -465)
            {
                players[i].transform.localPosition = new Vector3(-465, players[i].transform.localPosition.y, players[i].transform.localPosition.z);
            }
            if (players[i].transform.localPosition.x >= 505)
            {
                players[i].transform.localPosition = new Vector3(505, players[i].transform.localPosition.y, players[i].transform.localPosition.z);
            }
            players[i].tackle.transform.localPosition = new Vector3(players[i].transform.localPosition.x, players[i].tackle.transform.localPosition.y, players[i].tackle.transform.localPosition.z);
            if (players[i].fishing == 1)
            {
                int fishy = fishIn(players[i].tackle.transform.localPosition.x, players[i].tackle.transform.localPosition.y);
                Console.WriteLine(fishy);
                if (fishy != -1)
                {
                    fish[fishy].GetComponent<FishingFish>().player = i;
                    fish[fishy].GetComponent<FishingFish>().caught = true;
                    players[i].fishing = 2;
                }
            }
        }
        for (int i = 0; i < fish.Count; ++i)
        {
            int res = fish[i].GetComponent<FishingFish>().moveFish();
            if (res == 1)
            {
                players[fish[i].GetComponent<FishingFish>().player].score += 10 / fish[i].GetComponent<FishingFish>().size;
                players[fish[i].GetComponent<FishingFish>().player].tackle.GetComponent<RectTransform>().localPosition = new Vector3(players[fish[i].GetComponent<FishingFish>().player].tackle.GetComponent<RectTransform>().localPosition.x, 135, players[fish[i].GetComponent<FishingFish>().player].tackle.GetComponent<RectTransform>().localPosition.z);
                players[fish[i].GetComponent<FishingFish>().player].fishing = 0;
                Destroy(fish[i]);
                fish[i] = newFish();
            }
        }
    }

    public override void OnPlayerKeyDown(int playerId, KeyCode key)
    {
        FishingPlayer player = players.Find((x) => x.data.id == playerId);

        if (!player)
        {
            return;
        }

        // Handle the key
        switch (key)
        {
            case KeyCode.Space:
                player.fishing = 1;
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
        // Find the player who pressed the key
        FishingPlayer player = players.Find((x) => x.data.id == playerId);

        if (!player)
        {
            return;
        }

        // Handle the key
        switch (key)
        {
            case KeyCode.A:
                player.rigidBody.velocity = new Vector2(-100, 0f);
                break;
            case KeyCode.D:
                player.rigidBody.velocity = new Vector2(100, 0f);
                break;
        }
    }

    public void finishGame()
    {
        gameEnded = true;
        players.Sort((p1, p2) => {
            float diff = p2.score - p1.score;

            if (diff < 0)
            {
                return -1;
            }

            if (diff > 0)
            {
                return 1;
            }

            return 0;
        });
        GameMaster.INSTANCE.minigameScoreboard = new List<PlayerData>();

        for (int i = 0; i < players.Count; ++i)
        {
            GameMaster.INSTANCE.minigameScoreboard.Add(players[i].data);
        }

        TaleExtra.MinigameScoreboard();
    }
}
