using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UwUBridgesPlayer : MonoBehaviour
{    
        [NonSerialized]
        public PlayerData data;
        [NonSerialized]
        public Rigidbody2D rigidBody;
        [NonSerialized]
        public new Transform transform;

        public Vector2Int pos;
        public bool finishedGame=false;
        public int position=0;
        public int xChosen=0;
        public int yChosen=0;
        public int score=0;
    
        void Awake(){
            rigidBody = GetComponent<Rigidbody2D>();
            transform = GetComponent<Transform>();
        }
        void Update(){
           
            if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.S)){
                InputSensor.TriggerKey(KeyCode.S, 0);
            }
            else if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.D)){
                InputSensor.TriggerKey(KeyCode.D, 0);
            }
            else if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.A)){
                InputSensor.TriggerKey(KeyCode.A, 0);
            }
            else if (data.id == 0 && InputMaster.GetKeyDown(KeyCode.W)){
                InputSensor.TriggerKey(KeyCode.W, 0);
            }
            else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.DownArrow)){
                InputSensor.TriggerKey(KeyCode.S, 1);
            }
            else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.RightArrow)){
                InputSensor.TriggerKey(KeyCode.D, 1);
            }
            else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.UpArrow)){
                InputSensor.TriggerKey(KeyCode.W, 1);
            }
            else if (data.id == 1 && InputMaster.GetKeyDown(KeyCode.LeftArrow)){
                InputSensor.TriggerKey(KeyCode.A, 1);
            }
        }
}