using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class BaseballPlayer : MonoBehaviour
{
    [NonSerialized]
    public PlayerData data;
    [NonSerialized]
    public Rigidbody2D rigidBody;
    [NonSerialized]
    public new Transform transform;
    [NonSerialized]
    public GameObject ball;
    [NonSerialized]
    public GameObject scoreBoard;
    [NonSerialized]
    public GameObject hitText;
    [NonSerialized]
    public int iter;
    [NonSerialized]
    public int score;
    [NonSerialized]
    public DateTime cooldown = DateTime.Now;
    [NonSerialized]
    public bool stunned = false;
    [NonSerialized]
    public DateTime resetBall;
    [NonSerialized]
    public bool resetBallBool = false;
    [NonSerialized]
    public Vector3 initPos;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        if (stunned && (DateTime.Now - cooldown).TotalSeconds < 1)
        {
            scoreBoard.GetComponent<TextMeshProUGUI>().SetText("Score: " + score.ToString() + "\nStunned: " + (1 - (DateTime.Now - cooldown).TotalSeconds).ToString().Substring(0, 5));
        }
        else if (stunned)
        {
            stunned = false;
            scoreBoard.GetComponent<TextMeshProUGUI>().SetText("Score: " + score.ToString());
        }
        // The key is different based on the player
        // In the future, the key will be the same for all players
        // The input sensor receives the Space for both players,
        // so only this method will need to be modified when multiplayer is implemented
        if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.Space))
        {
            InputSensor.TriggerKey(KeyCode.Space, 0);
        }
        else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.Return))
        {
            InputSensor.TriggerKey(KeyCode.Space, 1);
        }
    }
}
