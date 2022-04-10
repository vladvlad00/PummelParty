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
            case Type.REV:
                renderer.sharedMaterial = GameMaster.INSTANCE.guard.reverseSpotMaterial;
                break;
            case Type.TELEPORT:
                renderer.sharedMaterial = GameMaster.INSTANCE.guard.teleportSpotMaterial;
                break;
            case Type.HP_PLUS:
                renderer.sharedMaterial = GameMaster.INSTANCE.guard.hpplusSpotMaterial;
                break;
            case Type.HP_MINUS:
                renderer.sharedMaterial = GameMaster.INSTANCE.guard.hpminusSpotMaterial;
                break;
            case Type.COINS:
                renderer.sharedMaterial = GameMaster.INSTANCE.guard.coinsSpotMaterial;
                break;
            case Type.RESPAWN:
                renderer.sharedMaterial = GameMaster.INSTANCE.guard.respawnSpotMaterial;
                break;
            case Type.START:
                renderer.sharedMaterial = GameMaster.INSTANCE.guard.startSpotMaterial;
                break;
            case Type.ITEM:
                renderer.sharedMaterial = GameMaster.INSTANCE.guard.itemSpotMaterial;
                break;
            default:
                Debug.LogError("Unknown material!");
                Debug.Break();
                break;
        }
    }
}
