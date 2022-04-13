using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[DefaultExecutionOrder(10)]
public class GameGuard : MonoBehaviour
{
    public static GameGuard INSTANCE;

    public TextMeshProUGUI diceText;
    public GameObject board;
    public Transform itemContainer;
    public ItemContainerMaster itemContainerMaster;

    public List<Player> players;
    public GameObject pathPrefab;
    [NonSerialized]
    public List<Spot> boardSpots;

    public Spot startingSpot;
    public List<Spot> graveyardSpots;
    [NonSerialized]
    public List<GameObject> arrows;

    public Material normalSpotMaterial;
    public Material crownSpotMaterial;
    public Material reverseSpotMaterial;
    public Material hpplusSpotMaterial;
    public Material hpminusSpotMaterial;
    public Material coinsSpotMaterial;
    public Material teleportSpotMaterial;
    public Material respawnSpotMaterial;
    public Material itemSpotMaterial;
    public Material startSpotMaterial;

    public GameObject scorePrefab;
    private GameObject[] scores;
    public RectTransform canvasTransform;

    public TextMeshProUGUI message;
    private float messageTime = 0;

    void Awake()
    {
        INSTANCE = this;

        boardSpots = new List<Spot>();

        foreach (Transform child in board.GetComponent<Transform>())
        {
            if (child.name.StartsWith("Spot"))
            {
                boardSpots.Add(child.GetComponent<Spot>());
            }
        }

        boardSpots.Sort((x, y) => x.name.CompareTo(y.name));

        for(int i = 0; i < boardSpots.Count; ++i)
        {
            boardSpots[i].index = i;
            
        }

        //lambda function that takes 2 positions and returns the distance between them
        Func<Vector3, Vector3, float> distance = (a, b) =>
        {
            return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.z - b.z, 2));
        };


        //lambda function that takes 2 positions and returns the angle between them
        Func<Vector3, Vector3, float> angleBetween = (a, b) =>
        {
            Vector3 dir = b - a;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            return angle;
        };

        arrows = new List<GameObject>();

        for (int i = 0; i < boardSpots.Count; ++i)
        {
            //aici pun sagetile     
            for (int j = 0; j < boardSpots[i].next.Count; j++)
            {
                //pui sageata aici
                GameObject obj = Instantiate(pathPrefab,
                    new Vector3(
                        (boardSpots[i].transform.position.x + boardSpots[i].next[j].transform.position.x) / 2,
                        0.2f,
                        (boardSpots[i].transform.position.z + boardSpots[i].next[j].transform.position.z) / 2
                    ),
                    Quaternion.Euler(-90, 180 + angleBetween(boardSpots[i].next[j].transform.position, boardSpots[i].transform.position), 0)
                );
                float spriteHeight = obj.GetComponent<SpriteRenderer>().bounds.size.y * obj.GetComponent<Transform>().localScale.y;
                obj.GetComponent<Transform>().localScale = new Vector3(
                    obj.GetComponent<Transform>().localScale.x / 2,
                    distance(boardSpots[i].transform.position, boardSpots[i].next[j].transform.position) / (spriteHeight) / 100 / 2,
                    obj.GetComponent<Transform>().localScale.z / 2
                );
                arrows.Add(obj);
            }

            // Force update the material
            boardSpots[i].Init();
        }


        GameMaster.INSTANCE.OnReturnToGame();

        for(int i = 0; i < players.Count; ++i) {
            if(GameMaster.INSTANCE.playerData[i].spot == -1)
            {
                GameMaster.INSTANCE.playerData[i].spot = startingSpot.index;
            }

            players[i].data = GameMaster.INSTANCE.playerData[i];
            players[i].Reposition();
            players[i].ChangeColor(GameMaster.INSTANCE.playerData[i].superColor.material);
        }

        CreateScores();
    }

    void Update()
    {
        messageTime -= Time.deltaTime;
        if (messageTime < 0)
            message.text = "";
    }

    public bool IsGraveyard(Spot spot)
    {
        return graveyardSpots.Contains(spot);
    }

    public Spot GetRandomGraveyard()
    {
        return graveyardSpots[UnityEngine.Random.Range(0, graveyardSpots.Count)];
    }

    public Spot GetRandomSpot(bool includeCrown = false)
    {
        Spot spot = boardSpots[UnityEngine.Random.Range(0, boardSpots.Count)];
        while (IsGraveyard(spot) || (spot == GameMaster.INSTANCE.crownSpot && includeCrown))
        {
            spot = boardSpots[UnityEngine.Random.Range(0, boardSpots.Count)];
        }
        return spot;
    }

    public void RotateArrows(bool reversed)
    {
        for (int i = 0; i < arrows.Count; i++)
        {
            double newY = arrows[i].transform.eulerAngles.y;
            if (reversed)
            {
                newY += 180;
            }
            else
            {
                newY -= 180;
            }
            arrows[i].transform.localEulerAngles = new Vector3(arrows[i].transform.eulerAngles.x, (float)newY, arrows[i].transform.eulerAngles.z);
        }
    }

    public void CreateScores()
    {
        const float padding = 5f;

        Vector3[] corners = new Vector3[4];
        canvasTransform.GetLocalCorners(corners);
        float minX = corners[0].x;
        float minY = corners[0].y;
        float maxX = corners[2].x;
        float maxY = corners[2].y;
        float sizeX = maxX - minX;
        float sizeY = maxY - minY;
        float scoreWidth = sizeX / 8;
        float scoreHeight = sizeY / 8;
        scores = new GameObject[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            Vector3 pos = new Vector3(scoreWidth / 2 - 20, -(scoreHeight / 2 + ((padding + scoreHeight) * i)) - 20, 0);
            scores[i] = Instantiate(scorePrefab, pos, Quaternion.identity);
            scores[i].GetComponent<RectTransform>().SetParent(canvasTransform, false);
            scores[i].GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            scores[i].GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            scores[i].GetComponent<RectTransform>().sizeDelta = new Vector2(scoreWidth, scoreHeight);
            scores[i].GetComponent<PlayerScore>().data = players[i].data;
        }
    }

    public void SortScores(int id)
    {
        int pos = 0;
        for (int i=0;i<players.Count;i++)
            if (scores[i].GetComponent<PlayerScore>().data.id == id)
            {
                pos = i;
                break;
            }

        PlayerScore currentScore = scores[pos].GetComponent<PlayerScore>();
        while (pos > 0)
        {
            PlayerScore nextScore = scores[pos-1].GetComponent<PlayerScore>();
            if (currentScore.data.crowns > nextScore.data.crowns ||
                currentScore.data.crowns == nextScore.data.crowns && currentScore.data.hp > nextScore.data.hp)
            {
                Vector3 temp = scores[pos].transform.position;
                scores[pos].transform.position = scores[pos - 1].transform.position;
                scores[pos-1].transform.position = temp;

                GameObject obj = scores[pos];
                scores[pos] = scores[pos - 1];
                scores[pos - 1] = obj;
            }
            pos--;
        }
    }

    public void DisplayMessage(string message, float time = 5f)
    {
        this.message.text = message;
        messageTime = time;
    }
}
