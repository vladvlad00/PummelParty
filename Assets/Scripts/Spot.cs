using System.Collections.Generic;
using UnityEngine;
using System;

public class Spot : MonoBehaviour
{
    public enum Type
    {
        NORMAL, // GRAY
        ITEM, // PURPL
        HP_PLUS, // GREEN
        HP_MINUS, // RED
        TELEPORT, // BLUE
        CROWN, // GOLD
        COINS, // ORANGE
        REV, // CYAN
        RESPAWN, // BLACK 
        START // MAGENTA
    }

    [NonSerialized]
    public new Transform transform;
    [NonSerialized]
    public new MeshRenderer renderer;
    [NonSerialized]
    public int index;
    public List<Spot> next;
    public Type originalType = Type.NORMAL;
    public Type type = Type.NORMAL;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        renderer = GetComponent<MeshRenderer>();
    }

    public void Init()
    {
        originalType = type;
        ChangeType(type);
    }

    public void ChangeType(Type type)
    {
        this.type = type;

        switch(type)
        {
            case Type.NORMAL:
                renderer.sharedMaterial = GameGuard.INSTANCE.normalSpotMaterial;
                break;
            case Type.CROWN:
                renderer.sharedMaterial = GameGuard.INSTANCE.crownSpotMaterial;
                break;
            case Type.REV:
                renderer.sharedMaterial = GameGuard.INSTANCE.reverseSpotMaterial;
                break;
            case Type.TELEPORT:
                renderer.sharedMaterial = GameGuard.INSTANCE.teleportSpotMaterial;
                break;
            case Type.HP_PLUS:
                renderer.sharedMaterial = GameGuard.INSTANCE.hpplusSpotMaterial;
                break;
            case Type.HP_MINUS:
                renderer.sharedMaterial = GameGuard.INSTANCE.hpminusSpotMaterial;
                break;
            case Type.COINS:
                renderer.sharedMaterial = GameGuard.INSTANCE.coinsSpotMaterial;
                break;
            case Type.RESPAWN:
                renderer.sharedMaterial = GameGuard.INSTANCE.respawnSpotMaterial;
                break;
            case Type.START:
                renderer.sharedMaterial = GameGuard.INSTANCE.startSpotMaterial;
                break;
            case Type.ITEM:
                renderer.sharedMaterial = GameGuard.INSTANCE.itemSpotMaterial;
                break;
            default:
                Debug.LogError("Unknown material!");
                Debug.Break();
                break;
        }
    }
}
