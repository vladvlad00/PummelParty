using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorRunPlayer : MonoBehaviour
{
    [NonSerialized]
    public PlayerData data;

    public Vector2Int pos;

    void Update()
    {
        // The key is different based on the player
        // In the future, the key will be the same for all players
        // The input sensor receives the Space for both players,
        // so only this method will need to be modified when multiplayer is implemented
        if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.W))
        {
            InputSensor.TriggerKey(KeyCode.W, 0);
        }
        else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.UpArrow))
        {
            InputSensor.TriggerKey(KeyCode.W, 1);
        }
        else if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.S))
        {
            InputSensor.TriggerKey(KeyCode.S, 0);
        }
        else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.DownArrow))
        {
            InputSensor.TriggerKey(KeyCode.S, 1);
        }
        else if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.A))
        {
            InputSensor.TriggerKey(KeyCode.A, 0);
        }
        else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.LeftArrow))
        {
            InputSensor.TriggerKey(KeyCode.A, 1);
        }
        else if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.D))
        {
            InputSensor.TriggerKey(KeyCode.D, 0);
        }
        else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.RightArrow))
        {
            InputSensor.TriggerKey(KeyCode.D, 1);
        }
    }
}
