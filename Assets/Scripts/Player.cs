using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {
    public enum State {
        IDLE,
        MOVING
    }

    public const float SPEED = 7.5f; // Moving speed

    [NonSerialized]
    public PlayerData data;

    [NonSerialized]
    public new Transform transform;
    [NonSerialized]
    public State state;

    int moveCount;
    Spot intermediarySpot;
    float clock;
    float moveTime;

    void Awake()
    {
        transform = GetComponent<Transform>();
        clock = 0f;
    }

    void Update()
    {
        switch(state)
        {
            case State.IDLE:
                return;
            case State.MOVING:
                Move();
                break;
        }
    }

    public void SetInMotion(int count)
    {
        moveCount = count;

        state = State.MOVING;
        RecalculateIntermediary();
    }

    public void Reposition()
    {
        Vector3 pos = data.GetSpot().transform.position;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
    }

    void Move()
    {
        clock += Time.deltaTime;

        if(clock > moveTime)
        {
            clock = moveTime;
        }

        // 0f -> player on the current spot
        // 1f -> player reached the intermediary spot
        float factor = MathSET.Map(clock, 0f, moveTime, 0f, 1f);
        factor = TaleUtil.Math.ParametricBlend(factor); // Ease in-out

        // 0f -> base spot position
        // 1f -> intermediary spot position
        Vector3 pos = TaleUtil.Math.Interpolate(data.GetSpot().transform.position, intermediarySpot.transform.position, factor);
        pos.y = transform.position.y; // Preserve player Y

        transform.position = pos;

        if (factor == 1f)
        {
            data.spot = intermediarySpot.index;
            --moveCount;
            clock = 0f;

            if(moveCount == 0)
            {
                // Reached the target spot, stop moving
                state = State.IDLE;
            }
            else
            {
                RecalculateIntermediary();
            }
        }
    }

    void RecalculateIntermediary()
    {
        //intermediarySpot = (data.spot + 1) % GameMaster.INSTANCE.guard.boardSpots.Count;
        intermediarySpot = data.GetSpot().next[0]; // TODO: allow the player to choose if there are multiple nexts.
        Spot spot = data.GetSpot();

        float highestDiff = Mathf.Max(
            Mathf.Abs(spot.transform.position.x - intermediarySpot.transform.position.x),
            Mathf.Abs(spot.transform.position.y - intermediarySpot.transform.position.y),
            Mathf.Abs(spot.transform.position.z - intermediarySpot.transform.position.z));

        moveTime = highestDiff / SPEED;

        float angle = MathSET.AngleBetween(intermediarySpot.transform.position, transform.position);
        transform.eulerAngles = new Vector3(0f, angle, 0);
    }
}
