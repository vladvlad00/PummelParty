using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class BaseballMaster : MinigameMaster
{
    private const float PLAYER_Y_DIFF = 150f;

    [SerializeField]
    RectTransform canvasTransform;
    [SerializeField]
    RectTransform boardTransform;
    [SerializeField]
    GameObject playerPrefab;

    [NonSerialized]
    public List<BaseballPlayer> players;

    public GameObject circlePrefab;
    public GameObject ballPrefab;

    [NonSerialized]
    public float ballStartY = 81f;
    [NonSerialized]
    public float ballStopY = 3f;
    [NonSerialized]
    public float maxSpeed = -200f;
    [NonSerialized]
    public float maxBallTime = 1.1f;

    public TextMeshProUGUI timeText;
    public DateTime startTime;
    [NonSerialized]
    public int maxTime = 30;

    [NonSerialized]
    private bool gameEnded = false;

    private GameTimer gameTimer;

    void Awake()
    {
        InitInputMaster();

        // Get the player IDs from the master and instantiate the minigame player objects
        players = new List<BaseballPlayer>();

        float xModif = 1920 / (3 * GameMaster.INSTANCE.minigamePlayers.Count + 1);
        float baseX = -1920 / 2 + xModif * 2;

        Vector3 basePos = new Vector3(baseX, -300f, 0f);

        for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; ++i)
        {
            PlayerData data = GameMaster.INSTANCE.minigamePlayers[i];

            GameObject obj = Instantiate(playerPrefab, basePos, Quaternion.identity);

            //RectTransform transform = obj.GetComponent<RectTransform>();
            //transform.SetParent(canvasTransform, false);
            drawCircle(basePos.x, data.superColor.color);

            players.Add(obj.GetComponent<BaseballPlayer>());
            players[i].data = data;

            GameObject playerBall = Instantiate(ballPrefab, new Vector3(basePos.x / 10, ballStartY, -100f), Quaternion.identity);
            playerBall.GetComponent<RectTransform>().SetParent(boardTransform, false);
            playerBall.GetComponent<Renderer>().material = data.superColor.material;
            players[i].ball = playerBall;
            players[i].initPos = playerBall.transform.position;

            players[i].score = 0;
            players[i].iter = 0;

            GameObject txt = new GameObject();
            txt.AddComponent<TextMeshProUGUI>();
            txt.GetComponent<TextMeshProUGUI>().SetText("Score: 0");
            txt.GetComponent<TextMeshProUGUI>().color = data.superColor.color;
            txt.GetComponent<TextMeshProUGUI>().fontSize = 24;
            txt.transform.position = new Vector3(basePos.x + 150, -30f, 0f);
            txt.transform.SetParent(canvasTransform, false);

            players[i].scoreBoard = txt;

            GameObject hit = new GameObject();
            hit.AddComponent<TextMeshProUGUI>();
            hit.GetComponent<TextMeshProUGUI>().SetText("");
            hit.GetComponent<TextMeshProUGUI>().color = data.superColor.color;
            hit.GetComponent<TextMeshProUGUI>().fontSize = 32;
            hit.transform.position = new Vector3(basePos.x + 150, 30f, 0f);
            hit.transform.SetParent(canvasTransform, false);
            players[i].hitText = hit;

            basePos.x += xModif * 2;
        }
        startTime = DateTime.Now;
        gameTimer = new GameTimer(maxTime, timeText, finishGame);
    }   

    float ballFormula(int x)
    {
        return (float)(15 * Math.Pow(Math.Log(x), 2));
    }

    void Update()
    {
        if (gameEnded)
        {
            return;
        }
        gameTimer.Update();

        foreach(BaseballPlayer player in players)
        {
            if ((player.resetBallBool && (DateTime.Now - player.resetBall).TotalSeconds >= maxBallTime) || (!player.resetBallBool && player.ball.transform.position.y <= ballStopY))
            {
                player.ball.GetComponent<BaseballBall>().hit = false;
                player.ball.GetComponent<BaseballBall>().stopSmall();
                player.resetBallBool = false;
                //Debug.Log(player.ball.GetComponent<Rigidbody>().velocity);
                player.ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                player.iter++;
                float pwr = Math.Max(maxSpeed, -ballFormula(player.iter));
                player.ball.GetComponent<Rigidbody>().velocity = new Vector3(0f, pwr, 0f);
                player.ball.transform.position = player.initPos;
                //player.ball.GetComponent<Rigidbody>().AddForce(new Vector3(0f, pwr, 0f));
            }
        }
    }

    public void drawCircle(float playerX, Color c)
    {
        GameObject circleObj = Instantiate(circlePrefab, new Vector3(playerX, 0f, -1f), Quaternion.identity);
        circleObj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        circleObj.GetComponent<Image>().color = c;
        circleObj.GetComponent<RectTransform>().SetParent(canvasTransform, false);
    }

    public override void OnPlayerKeyDown(int playerId, KeyCode key)
    {
        // Find the player who pressed the key
        BaseballPlayer player = players.Find((x) => x.data.id == playerId);

        if (!player)
        {
            return;
        }

        // Handle the key
        switch (key)
        {
            case KeyCode.Space:
                if (player.stunned)
                {
                    return;
                }
                if (Math.Abs(player.ball.transform.position.y - 52) < 7 && !player.ball.GetComponent<BaseballBall>().hit)
                {
                    int modif = (int)Math.Pow(10 - Math.Abs(player.ball.transform.position.y - 52) / 7 * 10, 2);
                    player.score += modif;
                    player.scoreBoard.GetComponent<TextMeshProUGUI>().SetText("Score: " + player.score.ToString());
                    player.hitText.GetComponent<TextMeshProUGUI>().SetText("+" + modif.ToString());
                    player.resetBallBool = true;
                    player.resetBall = DateTime.Now;
                    player.ball.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.Range(-1000f, 1000f), UnityEngine.Random.Range(1000f, 2000f), 20000);
                    player.ball.GetComponent<BaseballBall>().startSmall(UnityEngine.Random.Range(0.015f, 0.025f));
                    player.ball.GetComponent<BaseballBall>().hit = true;
                }
                else
                {
                    player.stunned = true;
                    player.cooldown = DateTime.Now;
                }
                break;
        }
    }

    public override void OnPlayerMouseClick(int playerId, bool rightClick)
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerMouseMove(int playerId, Vector2 pos)
    {
        throw new System.NotImplementedException();
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

    public override void OnPlayerKeyHold(int playerId, KeyCode key)
    {
        return;
    }
}
