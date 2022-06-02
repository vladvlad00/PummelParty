using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class FishingPlayer : MonoBehaviour
{
    [NonSerialized]
    public PlayerData data;
    [NonSerialized]
    public Rigidbody2D rigidBody;
    [NonSerialized]
    public new Transform transform;
    [NonSerialized]
    public GameObject tackle;
    [NonSerialized]
    public int fishing = 0;
    [NonSerialized]
    public float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fishing == 0)
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
            else if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.Space))
            {
                InputSensor.TriggerKey(KeyCode.Space, 0);
            }
            else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.Return))
            {
                InputSensor.TriggerKey(KeyCode.Space, 1);
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
        else
        {
            Vector3 tacklePos = tackle.GetComponent<RectTransform>().localPosition;
            if (fishing == 1)
            {
                tackle.GetComponent<RectTransform>().localPosition = new Vector3(tacklePos.x, tacklePos.y - 1, tacklePos.z);
                if (tackle.GetComponent<RectTransform>().localPosition.y < -270)
                {
                    fishing = 0;
                    tackle.GetComponent<RectTransform>().localPosition = new Vector3(tacklePos.x, 135, tacklePos.z);
                }
            }
            else if (fishing == 2)
            {
                tackle.GetComponent<RectTransform>().localPosition = new Vector3(tacklePos.x, tacklePos.y + 1, tacklePos.z);
            }
        }
    }
}
