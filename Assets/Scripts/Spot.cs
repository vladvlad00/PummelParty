using System.Collections.Generic;
using UnityEngine;
using System;

public class Spot : MonoBehaviour
{
    public enum Type
    {
        NORMAL,
        HP_PLUS,
        HP_MINUS,
        ITEM
    }

    [NonSerialized]
    public new Transform transform;
    [NonSerialized]
    public int index;
    public List<Spot> next;
    public Type type = Type.NORMAL;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }
}
