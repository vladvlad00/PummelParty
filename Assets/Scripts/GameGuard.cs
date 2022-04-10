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

    public List<Player> players;
    public GameObject pathPrefab;
    [NonSerialized]
    public List<Spot> boardSpots;

    public Spot startingSpot;
    public Spot graveyardSpot;

    public Material normalSpotMaterial;
    public Material crownSpotMaterial;

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
                    obj.GetComponent<Transform>().localScale.x,
                    distance(boardSpots[i].transform.position, boardSpots[i].next[j].transform.position) / (spriteHeight) / 100,
                    obj.GetComponent<Transform>().localScale.z
                );
            }
        }


        GameMaster.INSTANCE.OnReturnToGame();

        for(int i = 0; i < players.Count; ++i) {
            if(GameMaster.INSTANCE.playerData[i].spot == -1)
            {
                GameMaster.INSTANCE.playerData[i].spot = startingSpot.index;
            }

            players[i].data = GameMaster.INSTANCE.playerData[i];
            players[i].Reposition();
        }
    }
}
