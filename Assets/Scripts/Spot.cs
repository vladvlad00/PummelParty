using System.Collections.Generic;
using UnityEngine;
using System;

public class Spot : MonoBehaviour
{
<<<<<<< HEAD
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
=======
    public enum Type
    {
        NORMAL,
        CROWN,
        HP_PLUS,
        HP_MINUS,
        ITEM
>>>>>>> de38f3c9ef67ebf757f00caf256fb818067def60
    }

    [NonSerialized]
    public new Transform transform;
    [NonSerialized]
    public new MeshRenderer renderer;
    [NonSerialized]
    public int index;
    public List<Spot> next;
<<<<<<< HEAD
    public SpotType type;
=======
    public Type type = Type.NORMAL;
>>>>>>> de38f3c9ef67ebf757f00caf256fb818067def60

    private void Awake()
    {
        transform = GetComponent<Transform>();
        renderer = GetComponent<MeshRenderer>();
    }

    public void ChangeType(Type type)
    {
        this.type = type;

        switch(type)
        {
            case Type.NORMAL:
                renderer.sharedMaterial = GameMaster.INSTANCE.guard.normalSpotMaterial;
                break;
            case Type.CROWN:
                renderer.sharedMaterial = GameMaster.INSTANCE.guard.crownSpotMaterial;
                break;
            default:
                Debug.LogError("Unknown material!");
                Debug.Break();
                break;
        }
    }
}
