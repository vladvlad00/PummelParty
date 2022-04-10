using System.Collections.Generic;
using UnityEngine;
using System;

public class Spot : MonoBehaviour
{
    public enum Type
    {
        NORMAL,
        CROWN,
        HP_PLUS,
        HP_MINUS,
        ITEM
    }

    [NonSerialized]
    public new Transform transform;
    [NonSerialized]
    public new MeshRenderer renderer;
    [NonSerialized]
    public int index;
    public List<Spot> next;
    public Type type = Type.NORMAL;

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
