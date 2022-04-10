using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {
    public enum State {
        IDLE,
        MOVING,
        SELECTING_SPOT
    }

    public const float SPEED = 7.5f; // Moving speed

    [NonSerialized]
    public PlayerData data;

    [NonSerialized]
    public new Transform transform;
    [NonSerialized]
    public State state;

    [SerializeField]
    GameObject selectSpotPrefab;

    int moveCount;
    Spot intermediarySpot;
    float clock;
    float moveTime;

    // The arrows that the player uses to select a spot
    List<GameObject> spotSelectionArrows;

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
            case State.SELECTING_SPOT:
                // The player has selected a spot and should now start moving again
                if(GameMaster.INSTANCE.state == GameMaster.State.PLAYER_MOVING)
                {
                    state = State.MOVING;

                    DeleteSpotSelectionArrows();
                    SelectIntermediary(GameMaster.INSTANCE.lastSelectedSpot);
                }
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

            if(intermediarySpot.type == Spot.Type.CROWN)
            {
                intermediarySpot.ChangeType(Spot.Type.NORMAL);

                moveCount = 0;
                ++data.crowns;
                GameMaster.INSTANCE.ChooseNewCrownSpot();
            }

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

    void DeleteSpotSelectionArrows()
    {
        for(int i = 0; i < spotSelectionArrows.Count; ++i)
        {
            Destroy(spotSelectionArrows[i]);
        }

        spotSelectionArrows.Clear();
        spotSelectionArrows = null;
    }

    void RecalculateIntermediary()
    {
        List<Spot> nextSpots = data.GetSpot().next;

        Debug.Assert(nextSpots.Count > 0, string.Format("No next spot available for current spot (index {0})", data.GetSpot().index));

        if(nextSpots.Count > 1)
        {
            GameMaster.INSTANCE.OnPlayerSelectingSpot();
            state = State.SELECTING_SPOT;

            spotSelectionArrows = new List<GameObject>(nextSpots.Count);

            for (int i = 0; i < nextSpots.Count; ++i)
            {
                GameObject obj = Instantiate(selectSpotPrefab, new Vector3(0f, 13f, 0f), Quaternion.identity);

                RectTransform transform = obj.GetComponent<RectTransform>();
                transform.SetParent(nextSpots[i].transform, false);
                transform.GetComponentInChildren<SelectSpotArrow>().spot = nextSpots[i];

                spotSelectionArrows.Add(obj);
            }

            return;
        }

        SelectIntermediary(data.GetSpot().next[0]);
    }

    void SelectIntermediary(Spot spot)
    {
        intermediarySpot = spot;

        Spot currentSpot = data.GetSpot();

        float highestDiff = Mathf.Max(
            Mathf.Abs(currentSpot.transform.position.x - intermediarySpot.transform.position.x),
            Mathf.Abs(currentSpot.transform.position.y - intermediarySpot.transform.position.y),
            Mathf.Abs(currentSpot.transform.position.z - intermediarySpot.transform.position.z));

        moveTime = highestDiff / SPEED;

        float angle = MathSET.AngleBetween(intermediarySpot.transform.position, transform.position);
        transform.eulerAngles = new Vector3(0f, angle, 0);
    }
}
