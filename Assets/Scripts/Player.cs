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
    public new Transform transform;
    [NonSerialized]
    public int spot;
    [NonSerialized]
    public State state;

    [SerializeField]
    GameMaster master;

    int targetSpot;
    int intermediarySpot;
    float clock;
    float moveTime;

    void Awake()
    {
        transform = GetComponent<Transform>();
        clock = 0f;
        spot = 0;
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

    public void SetTargetSpot(int where)
    {
        Debug.Assert(where != spot, "SetTargetSpot() called with the same spot as the current one");

        targetSpot = where;
        state = State.MOVING;
        RecalculateIntermediary();
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
        Vector3 pos = TaleUtil.Math.Interpolate(master.boardSpots[spot].position, master.boardSpots[intermediarySpot].position, factor);
        pos.y = transform.position.y; // Preserve player Y

        transform.position = pos;

        if (factor == 1f)
        {
            spot = intermediarySpot;
            clock = 0f;

            if(spot == targetSpot)
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
        intermediarySpot = (spot + 1) % master.boardSpots.Count;

        float highestDiff = Mathf.Max(
            Mathf.Abs(master.boardSpots[spot].position.x - master.boardSpots[intermediarySpot].position.x),
            Mathf.Abs(master.boardSpots[spot].position.y - master.boardSpots[intermediarySpot].position.y),
            Mathf.Abs(master.boardSpots[spot].position.z - master.boardSpots[intermediarySpot].position.z));

        moveTime = highestDiff / SPEED;

        float angle = MathSET.AngleBetween(master.boardSpots[intermediarySpot].position, transform.position);
        transform.eulerAngles = new Vector3(0f, angle, 0);
    }
}
