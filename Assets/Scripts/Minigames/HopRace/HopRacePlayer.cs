using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HopRacePlayer : MonoBehaviour
{
    public const float SPEED = 45f;

    [NonSerialized]
    public PlayerData data;
    [NonSerialized]
    public Rigidbody2D rigidBody;
    [NonSerialized]
    public new Transform transform;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    void Update()
    {
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
