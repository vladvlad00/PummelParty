using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FallingFloorPlayer : MonoBehaviour
{
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
        if (data.id == 0 && InputMaster.GetKeyHold(KeyCode.W))
        {
            InputSensor.TriggerKeyHold(KeyCode.W, 0);
        }
        else if (data.id == 1 && InputMaster.GetKeyHold(KeyCode.UpArrow))
        {
            InputSensor.TriggerKeyHold(KeyCode.W, 1);
        }
        else if (data.id == 0 && InputMaster.GetKeyHold(KeyCode.S))
        {
            InputSensor.TriggerKeyHold(KeyCode.S, 0);
        }
        else if (data.id == 1 && InputMaster.GetKeyHold(KeyCode.DownArrow))
        {
            InputSensor.TriggerKeyHold(KeyCode.S, 1);
        }
        else if (data.id == 0 && InputMaster.GetKeyHold(KeyCode.A))
        {
            InputSensor.TriggerKeyHold(KeyCode.A, 0);
        }
        else if (data.id == 1 && InputMaster.GetKeyHold(KeyCode.LeftArrow))
        {
            InputSensor.TriggerKeyHold(KeyCode.A, 1);
        }
        else if (data.id == 0 && InputMaster.GetKeyHold(KeyCode.D))
        {
            InputSensor.TriggerKeyHold(KeyCode.D, 0);
        }
        else if (data.id == 1 && InputMaster.GetKeyHold(KeyCode.RightArrow))
        {
            InputSensor.TriggerKeyHold(KeyCode.D, 1);
        }
    }
}
