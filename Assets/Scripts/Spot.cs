using System.Collections.Generic;
using UnityEngine;
using System;

public class Spot : MonoBehaviour
{
    public enum SpotType
    {
        NORMAL, // GRAY
        ITEM, // PURPL
        HP, // GREEN
        DAMAGE, // RED
        TELEPORT, // BLUE
        CHEST, // CYAN
        COINS, // ORANGE
        REV, // YELLOW
        RESPAWN, // BLACK 
        START // MAGENTA
    }

    [NonSerialized]
    public new Transform transform;
    [NonSerialized]
    public int index;
    public List<Spot> next;
    public SpotType type;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }
}
