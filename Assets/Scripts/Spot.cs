using System.Collections.Generic;
using UnityEngine;
using System;

public class Spot : MonoBehaviour
{
    [NonSerialized]
    public new Transform transform;
    [NonSerialized]
    public int index;
    public List<Spot> next;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }
}
