using System;
using System.Linq;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PuzzleRaceMaster : MinigameMaster
{

    private int[] x_start_player={1,7,13,1,7,13, 1, 7,13};
    private int[] y_start_player={1,1, 1,7,7, 7,13,13,13};

    private int playersThatFinished = 0;

    private int ROUNDS_REMAINING = 1;

    private float ROUND_MAX_TIME = 100f;

    bool roundPlaying = true;

    private float PAUSE_TIME = 3f;

    private float elapsedTime = 0f;

    private const int ARENA_SIZE = 15;

    private const int padding = 2;

    private const int LIGHT_DISTANCE = 2;

    private bool gameDone = false;

    private readonly Vector2Int[]
        startPositions =
        {
            new Vector2Int(1, 1),
            new Vector2Int(7, 1),
            new Vector2Int(13, 1),
            new Vector2Int(1, 7),
            new Vector2Int(13, 7),
            new Vector2Int(1, 13),
            new Vector2Int(7, 13),
            new Vector2Int(13, 13)
        };

    public enum Squares : int
    {
        NORD = 0,
        EST = 1,
        SUD = 2,
        WEST = 3,
        NORD_EST = 4,
        NORD_WEST = 5,
        SUD_EST = 6,
        SUD_WEST = 7
    }

    private float minX;

    private float minY;

    private float squareSize;

    [NonSerialized]
    public List<PuzzleRacePlayer> players;

    [SerializeField]
    RectTransform canvasTransform;

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    GameObject squarePrefab;


    int[,] arena;

    GameObject[,] arenaSquares;
    [NonSerialized]
    public GameObject[] squareScores;

    [NonSerialized]
    public GameObject[] playerMgScores;

    private MinigameScoreboard mgScoreboard;

    private Vector3 GetPosFromLineCol(int i, int j)
    {
        return new Vector3(minX + j * (squareSize + padding),
            minY + i * (squareSize + padding),
            0);
    }

        private Vector3 GetPosForScores(int i, int j)
    {
        return new Vector3(minX + j * (squareSize + padding)+93,
            minY + i * (squareSize + padding)-12,
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
            (Math.Min(sizeX, sizeY) - (ARENA_SIZE - 1) * padding) / ARENA_SIZE;
        minX += squareSize / 2;
        minY += squareSize / 2;
        if (sizeX > sizeY)
            minX += (sizeX - sizeY) / 2;
        else
            minY += (sizeY - sizeX) / 2;

        players = new List<PuzzleRacePlayer>();
        arena = new int[ARENA_SIZE, ARENA_SIZE];
        arenaSquares = new GameObject[ARENA_SIZE, ARENA_SIZE];
        squareScores = new GameObject[ARENA_SIZE];

        for (int i = 0; i < ARENA_SIZE; i++)
        {
            for (int j = 0; j < ARENA_SIZE; j++)
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
                if (
                    ((i > 2 && i < 6) || (i > 8 && i < 12)) ||
                    ((j > 2 && j < 6) || (j > 8 && j < 12))
                )
                {
                        arenaSquares[i, j].GetComponent<Image>().color =
                            Color.black;
                }
                else
                {
                    arenaSquares[i, j].GetComponent<Image>().color =
                        Color.white;
                
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
                new Vector2(squareSize * 0.9f, squareSize * 0.9f); //20 20
            obj.GetComponent<Image>().color = data.superColor.color;
            players.Add(obj.GetComponent<PuzzleRacePlayer>());
            players[i].pos = startPositions[i];
            players[i].data = data;
            players[i].position=100;
        }

        for(int i=0;i<GameMaster.INSTANCE.minigamePlayers.Count;++i){
            createPuzzle(startPositions[i].x,startPositions[i].y);
        }

        playerMgScores = new GameObject[players.Count];
        TextMeshProUGUI[] scoreText = new TextMeshProUGUI[players.Count];
        for (int i=0;i<players.Count;++i){
            playerMgScores[i]=createScoreboardComponent(i);
            scoreText[i]=playerMgScores[i].GetComponent<TextMeshProUGUI>();
        }
        int[] scoreArray=new int[players.Count];
        mgScoreboard = new MinigameScoreboard(scoreText,scoreArray,finishGame);
    }

    public override void OnPlayerKeyDown(int playerId, KeyCode key)
    {
        // Find the player who pressed the key
        PuzzleRacePlayer player = players.Find((x) => x.data.id == playerId);

        if (!player)
        {
            return;
        }

        // Handle the key
        switch (key)
        {
            case KeyCode.W:
                if (player.pos.x<=startPositions[playerId].x){
                    player.pos.x+=1;
                }
                break;
            case KeyCode.S:
                if (player.pos.x>=startPositions[playerId].x){
                    player.pos.x-=1;
                }

                break;
            case KeyCode.A:
                if (player.pos.y>=startPositions[playerId].y){
                    player.pos.y-=1;
                }
                break;
            case KeyCode.D:
                if (player.pos.y<=startPositions[playerId].y){
                     player.pos.y+=1;
                }
                break;
            case KeyCode.X:
                if (player.valueCarried ==-1 && arena[player.pos.x,player.pos.y]!=-1){
                    player.valueCarried = arena[player.pos.x,player.pos.y];
                    arena[player.pos.x,player.pos.y]=-1;
                    cleanPuzzleTile(player.pos.x,player.pos.y);
                }
                else if(player.valueCarried!=-1 && arena[player.pos.x,player.pos.y]==-1){
                    arena[player.pos.x,player.pos.y]=player.valueCarried;
                    player.valueCarried=-1;
                    createPuzzleTile(player.pos.x,player.pos.y);
                    if (playerFinished(playerId)){
                        Debug.Log("Updating player"+playerId+"because he finished");
                        player.position=playersThatFinished;
                        playersThatFinished+=1;
                        player.position+=1;
                        cleanAllPuzzleTiles(playerId);
                        int[] scores=new int[GameMaster.INSTANCE.minigamePlayers.Count];
                        for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; i++)
                        {
                            Debug.Log("Player:"+i);
                            Debug.Log("Position:"+players[i].position);
                            Debug.Log("Scores:"+players[i].score);
                            if (players[i].position!=100){
                                scores[i] = 100-(players[i].position-1)*10;
                                players[i].score=scores[i];
                                Debug.Log("Player:"+i);
                                Debug.Log("Position:"+players[i].position);
                                Debug.Log("Scores:"+players[i].score);
                            }else{
                                scores[i] = 0;
                            }

                        }
                        mgScoreboard.Update(scores,gameDone); 
                    }
                }
                break;
        }

        player.GetComponent<RectTransform>().localPosition =
            GetPosFromLineCol(player.pos.x, player.pos.y);
    }

    void Update()
    {
        if (roundPlaying)
        {
            if (gameDone) return;
            elapsedTime += Time.deltaTime;

            if (elapsedTime > ROUND_MAX_TIME || playersThatFinished==GameMaster.INSTANCE.minigamePlayers.Count)
            {
                elapsedTime = 0;
                ROUNDS_REMAINING--;
                roundPlaying = false;
            }
        }else{
            int[] scores=new int[GameMaster.INSTANCE.minigamePlayers.Count];
            for (int i = 0; i < GameMaster.INSTANCE.minigamePlayers.Count; i++)
            {
                if (players[i].position!=100){
                    scores[i] = 100-(players[i].position-1)*10;
                    players[i].score=scores[i];
                }
            }
            mgScoreboard.Update(scores,gameDone); 
            //Debug.Log("Round not playing"+elapsedTime+" "+PAUSE_TIME);
            elapsedTime += Time.deltaTime;
            //Debug.Log("Elapsed time: " + elapsedTime);
            if (elapsedTime > PAUSE_TIME){
                int pos_counter=0;
                foreach (PuzzleRacePlayer player in players)
                {
                    player.pos = startPositions[pos_counter];
                    player.GetComponent<RectTransform>().localPosition =
                        GetPosFromLineCol(player.pos.x, player.pos.y);
                    ++pos_counter;
                }

                if (ROUNDS_REMAINING == 0)
                {
                    finishGame();
                }

                elapsedTime = 0;
                roundPlaying = true;
            }

        }
    }
    public void createBridgeScore(int x,int y,int position){
        GameObject txt = new GameObject();
        txt.AddComponent<TextMeshProUGUI>();
        txt.GetComponent<TextMeshProUGUI>().SetText(arena[x, y].ToString());
        txt.GetComponent<TextMeshProUGUI>().color = Color.red;
        txt.GetComponent<TextMeshProUGUI>().fontSize = 24;
        txt.transform.position = GetPosForScores(x,y);
        txt.transform.SetParent(canvasTransform, false);
        squareScores[position] = txt;
    }


    public void createPuzzle(int x,int y){
        Debug.Log("Players"+players.Count);
        int[] scoreArray={1,2,3,4,5,6,7,8};
        Debug.Log(scoreArray);
        var r=new System.Random();
        scoreArray = scoreArray.OrderBy(x => r.Next()).ToArray();
       for(int i=0;i<8;++i){
        GameObject txt = new GameObject();
        switch (i)
        {
            case 0:
                arena[x-1,y]=scoreArray[i];
                createPuzzleTile(x-1,y);
                break;
            case 1:
                arena[x+1,y]=scoreArray[i];
                createPuzzleTile(x+1,y);
                break;
            case 2:
                arena[x,y-1]=scoreArray[i];
                createPuzzleTile(x,y-1);
                break;
            case 3:
                arena[x,y+1]=scoreArray[i];
                createPuzzleTile(x,y+1);
                break;
            case 4:
                arena[x-1,y-1]=scoreArray[i];
                createPuzzleTile(x-1,y-1);
                break;
            case 5:
                arena[x-1,y+1]=scoreArray[i];
                createPuzzleTile(x-1,y+1);
                break;
            case 6:
                arena[x+1,y-1]=scoreArray[i];
                createPuzzleTile(x+1,y-1);
                break;
            case 7:
                arena[x+1,y+1]=scoreArray[i];
                createPuzzleTile(x+1,y+1);
                break;
        } 
       }
    }

    public void createPuzzleTile(int x,int y){
        GameObject txt = new GameObject();
        txt.AddComponent<TextMeshProUGUI>();
        txt.GetComponent<TextMeshProUGUI>().SetText(arena[x, y].ToString());
        txt.GetComponent<TextMeshProUGUI>().color = Color.green;
        txt.GetComponent<TextMeshProUGUI>().fontSize = 24;
        txt.transform.position = GetPosForScores(x,y);
        txt.transform.SetParent(canvasTransform, false);
    }

    public void cleanAllPuzzleTiles(int playerId){
       for(int i=startPositions[playerId].x-1;i<=startPositions[playerId].x+1;++i){
            for(int j=startPositions[playerId].y-1;j<=startPositions[playerId].y+1;++j){
                arena[i,j]=-1;
                cleanPuzzleTile(i,j);
            }
        }
    }

    public void cleanPuzzleTile(int x,int y){
        for (int i=0;i<=8;++i){
            GameObject txt = new GameObject();
            txt.AddComponent<TextMeshProUGUI>();
            txt.GetComponent<TextMeshProUGUI>().SetText(i.ToString());
            txt.GetComponent<TextMeshProUGUI>().color = Color.white;
            txt.GetComponent<TextMeshProUGUI>().fontSize = 24;
            txt.transform.position = GetPosForScores(x,y);
            txt.transform.SetParent(canvasTransform, false);
        } 

    }

    public bool playerFinished(int playerId){
        int previous_value=-100;
        for(int i=startPositions[playerId].x+1;i>=startPositions[playerId].x-1;--i){
            for(int j=startPositions[playerId].y-1;j<=startPositions[playerId].y+1;++j){
                if(arena[i,j]!=-1){
                    if (previous_value>arena[i,j]){
                        Debug.Log("Player did not finish");
                        return false;
                    }
                    previous_value=arena[i,j];
                }
            }
        }
        Debug.Log("Player finished"+playerId);
        return true;
    }


    public void displayBridgeScore(int x,int y,int position){
        squareScores[position].GetComponent<TextMeshProUGUI>().SetText(arena[x, y].ToString());
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

    public void finishGame(){
        Dictionary<int, int> playerScores =
            new Dictionary<int, int>();

        foreach (PuzzleRacePlayer p in players)
        {
            playerScores.Add(p.data.id, p.score);
            //Debug.Log("Total score for player " +
            //            p.data.id +": " +p.score);
        }

        GameMaster.INSTANCE.minigameScoreboard =
        new List<PlayerData>();
        foreach (PuzzleRacePlayer p in players)
        GameMaster.INSTANCE.minigameScoreboard.Add(p.data);
        GameMaster.INSTANCE.minigameScoreboard
            .Sort((p1, p2) =>
            {
                if (playerScores[p1.id] == playerScores[p2.id])
                    return p2.id - p1.id;
                return playerScores[p2.id] - playerScores[p1.id];
            });
        gameDone = true;
        TaleExtra.MinigameScoreboard();
    }


    
    public GameObject createScoreboardComponent(int playerNumber){
        GameObject txt = new GameObject();
        txt.AddComponent<TextMeshProUGUI>();
        txt.GetComponent<TextMeshProUGUI>().SetText("0");
        txt.GetComponent<TextMeshProUGUI>().color =  GameMaster.INSTANCE.minigamePlayers[playerNumber].superColor.color;
        txt.GetComponent<TextMeshProUGUI>().fontSize = 24;
        txt.transform.position = GetPosFromLineCol((int)(ARENA_SIZE/(2+players.Count)*(1+playerNumber)),0);
        txt.transform.SetParent(canvasTransform, false);
        return txt;
    }


}
