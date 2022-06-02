using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class FishingFish : MonoBehaviour
{
    [NonSerialized]
    public PlayerData data;
    [NonSerialized]
    public new Transform transform;
    [NonSerialized]
    public GameObject scoreBoard;
    [NonSerialized]
    public bool left;
    [NonSerialized]
    public float leftMargin;
    [NonSerialized]
    public float rightMargin;
    [NonSerialized]
    public float topMargin;
    [NonSerialized]
    public float bottomMargin;
    [NonSerialized]
    public bool caught = false;
    [NonSerialized]
    public float size = 1;
    [NonSerialized]
    public int player = -1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int moveFish()
    {
        if (!caught)
        {
            if (left)
            {
                transform.localPosition = new Vector3(transform.localPosition.x - 1 / (size * 2), transform.localPosition.y, transform.localPosition.z);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x + 1 / (size * 2), transform.localPosition.y, transform.localPosition.z);
            }
            if (transform.localPosition.x >= rightMargin - 32)
            {
                left = true;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (transform.localPosition.x <= leftMargin + 32)
            {
                left = false;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            return 0;
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z);
            if (transform.localPosition.y >= topMargin)
            {
                return 1;
            }
            return 0;
        }
    }

    public void spawnFish(float lft, float rgt, float top, float btm)
    {
        transform = GetComponent<Transform>();
        leftMargin = lft;
        rightMargin = rgt;
        topMargin = top;
        bottomMargin = btm;
        size = 0.5f * UnityEngine.Random.Range(1f, 4f);
        if (UnityEngine.Random.value >= 0.5)
        {
            left = true;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            left = false;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        transform.localPosition = new Vector3(leftMargin + Mathf.Abs(leftMargin - rightMargin) * UnityEngine.Random.value, bottomMargin + Mathf.Abs(topMargin - bottomMargin) * UnityEngine.Random.value, 5);
        transform.localScale = new Vector3(transform.localScale.x * size, transform.localScale.y * size, transform.localScale.z * size);
    }
}
