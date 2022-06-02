using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BombBattlePlayer : MonoBehaviour
{    
        [NonSerialized]
        public PlayerData data;
        [NonSerialized]
        public Rigidbody2D rigidBody;
        [NonSerialized]
        public new Transform transform;

        public Vector2 pos;
        public int position=0;
        public bool finishedGame=false;
        public bool hasBomb=false;
        public bool hasDied=false;
        public float stunnedTimestamp=0f;
    
        void Awake(){
            rigidBody = GetComponent<Rigidbody2D>();
            transform = GetComponent<Transform>();
        }
        void Update(){
            if (data.id == 0 && InputMaster.GetKeyHold(KeyCode.S)){
                InputSensor.TriggerKeyHold(KeyCode.S, 0);
            }
            else if (data.id == 0 && InputMaster.GetKeyHold(KeyCode.D)){
                InputSensor.TriggerKeyHold(KeyCode.D, 0);
            }
            else if (data.id == 0 && InputMaster.GetKeyHold(KeyCode.A)){
                InputSensor.TriggerKeyHold(KeyCode.A, 0);
            }
            else if (data.id == 0 && InputMaster.GetKeyHold(KeyCode.W)){
                InputSensor.TriggerKeyHold(KeyCode.W, 0);
            }
            else if (data.id == 0 && InputMaster.GetKeyHold(KeyCode.X)){
                InputSensor.TriggerKeyHold(KeyCode.X, 0);
            }
            else if (data.id == 1 && InputMaster.GetKeyHold(KeyCode.DownArrow)){
                InputSensor.TriggerKeyHold(KeyCode.S, 1);
            }
            else if (data.id == 1 && InputMaster.GetKeyHold(KeyCode.RightArrow)){
                InputSensor.TriggerKeyHold(KeyCode.D, 1);
            }
            else if (data.id == 1 && InputMaster.GetKeyHold(KeyCode.UpArrow)){
                InputSensor.TriggerKeyHold(KeyCode.W, 1);
            }
            else if (data.id == 1 && InputMaster.GetKeyHold(KeyCode.LeftArrow)){
                InputSensor.TriggerKeyHold(KeyCode.A, 1);
            }
            else if (data.id == 1 && InputMaster.GetKeyHold(KeyCode.C)){
                InputSensor.TriggerKeyHold(KeyCode.X, 1);
            }
        }

}