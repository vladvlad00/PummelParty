using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeteorRainPlayer : MonoBehaviour
{
    public const float SPEED = 500f;

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
        if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.A))
        {
            InputSensor.TriggerKey(KeyCode.A, 0);
        }
        else if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.D))
        {
            InputSensor.TriggerKey(KeyCode.D, 0);
        }
        else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.LeftArrow))
        {
            InputSensor.TriggerKey(KeyCode.A, 1);
        }
        else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.RightArrow))
        {
            InputSensor.TriggerKey(KeyCode.D, 1);
        }

        if (data.id == 0 && InputMaster.GetKeyHold(KeyCode.A))
        {
            InputSensor.TriggerKeyHold(KeyCode.A, 0);
        }
        else if (data.id == 0 && InputMaster.GetKeyHold(KeyCode.D))
        {
            InputSensor.TriggerKeyHold(KeyCode.D, 0);
        }
        else if (data.id == 1 && InputMaster.GetKeyHold(KeyCode.LeftArrow))
        {
            InputSensor.TriggerKeyHold(KeyCode.A, 1);
        }
        else if (data.id == 1 && InputMaster.GetKeyHold(KeyCode.RightArrow))
        {
            InputSensor.TriggerKeyHold(KeyCode.D, 1);
        }
    }
}
