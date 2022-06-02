using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BombBattleMaster : MinigameMaster
{
    private const int ARENA_SIZE_X = 17;

    private const int ARENA_SIZE_Y = 30;

    private const int padding = 1;

    private const int LIGHT_DISTANCE = 2;

    private int playersThatFinished = 0;

    private bool gameDone = false; 

    private int playersAlive=0;

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

    private float ROUND_MAX_TIME = 60f;

    private bool makeBlack = true;

    private float minX;

    private float minY;

    private float squareSize;

    [NonSerialized]
    public List<BombBattlePlayer> players;

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

        //ROUND_MAX_TIME=UnityEngine.Random.Range(60,120);
        ROUND_MAX_TIME=UnityEngine.Random.Range(15,20);
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

        players = new List<BombBattlePlayer>();
        arena = new int[ARENA_SIZE_X, ARENA_SIZE_Y];
        arenaSquares = new GameObject[ARENA_SIZE_X, ARENA_SIZE_Y];

        for (int i = 0; i < ARENA_SIZE_X; i++)
        {
            for (int j = 0; j < ARENA_SIZE_Y; j++)
            {
                arena[i, j] = 0;
                if (UnityEngine.Random.Range(0,100)%11==0){
                    arena[i,j]=-1;
                }
                 Vector3 pos = GetPosFromLineCol(i, j);
                arenaSquares[i, j] =
                    Instantiate(squarePrefab, pos, Quaternion.identity);
                arenaSquares[i, j]
                    .GetComponent<RectTransform>()
                    .SetParent(canvasTransform, false);
                arenaSquares[i, j].GetComponent<RectTransform>().sizeDelta =
                    new Vector2(squareSize, squareSize);
                if (arena[i,j]==0){
                    arenaSquares[i, j].GetComponent<Image>().color = Color.gray;
                }else{
                     arenaSquares[i, j].GetComponent<Image>().color = Color.black;
                }

            }
        }
        
        int playerWithBomb=UnityEngine.Random.Range(0,GameMaster.INSTANCE.minigamePlayers.Count);
        playersAlive=GameMaster.INSTANCE.minigamePlayers.Count;
        Debug.Log("Player"+playerWithBomb+"has the bomb");

        for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; i++)
        {
            PlayerData data = GameMaster.INSTANCE.minigamePlayers[i];

            Vector3 playerPos =
                GetPosFromLineCol(startPositions[i].x, startPositions[i].y);

            GameObject obj =
                Instantiate(playerPrefab, playerPos, Quaternion.identity);
            obj.GetComponent<RectTransform>().SetParent(canvasTransform, false);


            if(i==playerWithBomb){
                obj.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(squareSize*1.4f, squareSize*1.4f); //20 20
            }else{
                 obj.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(squareSize, squareSize); //20 20
            }
            
            obj.GetComponent<Image>().color = data.superColor.color;
            players.Add(obj.GetComponent<BombBattlePlayer>());
            players[i].pos = startPositions[i];
            players[i].data = data;
             if(i==playerWithBomb){
                players[i].hasBomb=true;
                Debug.Log("Player"+i+"has the bomb");
            }
        }

        gameTimer = new GameTimer(ROUND_MAX_TIME, timerText, finishGameTime);
    }

    public override void OnPlayerKeyHold(int playerId, KeyCode key)
    {
        // Find the player who pressed the key
        BombBattlePlayer player = players.Find((x) => x.data.id == playerId);

        if (!player)
        {
            return;
        }

        float
            dx = 0f,
            dy = 0f;

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
            case KeyCode.X:
                if (player.hasBomb==true){
                    Debug.Log("Player"+playerId+"tried to pass the bomb");
                    for(int i=0;i<GameMaster.INSTANCE.minigamePlayers.Count;++i){
                        Debug.Log("Id:"+i+"position:"+players[i].pos.x+players[i].pos.y);
                        Debug.Log("Bomb player position"+player.pos.x+player.pos.y);
                        if(i!=playerId){
                            if((int)(player.pos.x)==(int)(players[i].pos.x) && (int)(player.pos.y)==(int)(players[i].pos.y)){
                                player.hasBomb=false;
                                players[i].hasBomb=true;
                                player.GetComponent<RectTransform>().sizeDelta=new Vector2(squareSize, squareSize);
                                players[i].GetComponent<RectTransform>().sizeDelta=new Vector2(squareSize*1.4f, squareSize*1.4f);
                            }
                        }
                    }
                }{
                    Debug.Log("Player"+playerId+"tried to stun");
                    for(int i=0;i<GameMaster.INSTANCE.minigamePlayers.Count;++i){
                        Debug.Log("Id:"+i+"position:"+players[i].pos.x+players[i].pos.y);
                        Debug.Log("Bomb player position"+player.pos.x+player.pos.y);
                        if(i!=playerId){
                            if((int)(player.pos.x)==(int)(players[i].pos.x) && (int)(player.pos.y)==(int)(players[i].pos.y) && players[i].hasBomb==false){
                               players[i].stunnedTimestamp=1f;
                               Debug.Log("Player was stunned");
                            }
                        }
                    }
                }
                break;
            
        }
        float auxX=player.pos.x + dx;
        float auxY=player.pos.y + dy;
        int newX = (int)(auxX);
        int newY = (int)(auxY);
        if (newX >= ARENA_SIZE_X || newX < 0 || newY >= ARENA_SIZE_Y || newY < 0 || arena[newX,newY]!=0
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

        foreach (BombBattlePlayer p in players)
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
        foreach (BombBattlePlayer p in players)
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
        //
        if (elapsedTime>ROUND_MAX_TIME){
            Debug.Log("One player must die");
            for(int i=0;i<GameMaster.INSTANCE.minigamePlayers.Count;++i){
                if(players[i].hasBomb==true){
                    players[i].hasDied=true;
                    players[i].position=playersAlive;
                    players[i].GetComponent<RectTransform>().sizeDelta=new Vector2(squareSize*0.00001f, squareSize*0.00001f);
                    playersAlive-=1;
                    break;
                }
            }
            ROUND_MAX_TIME=UnityEngine.Random.Range(12,17);
            elapsedTime=0f;
            int playerWithBomb=UnityEngine.Random.Range(0,GameMaster.INSTANCE.minigamePlayers.Count);
            while (true){
                if(players[playerWithBomb].hasDied==false){
                    players[playerWithBomb].hasBomb=true;
                    break;
                }
                playerWithBomb=UnityEngine.Random.Range(0,GameMaster.INSTANCE.minigamePlayers.Count);
            }
        }
        //Debug.Log("Elapsed time: " + elapsedTime);
     
        if (playersAlive == 0)
        {
            Dictionary<int, int> playerScores = new Dictionary<int, int>();
            Debug.Log("All players finished");
            foreach (BombBattlePlayer p in players)
            {
                playerScores.Add(p.data.id, p.position);
            }
            
            GameMaster.INSTANCE.minigameScoreboard = new List<PlayerData>();
            foreach (BombBattlePlayer p in players)
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


}
