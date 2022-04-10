using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public const int MAX_HP = 20;

    public int id;
    public string name;

    [NonSerialized]
    public int spot = -1;

    [NonSerialized]
    public int crowns = 0;

    [NonSerialized]
    public int coins = 0;

    [NonSerialized]
    public int hp = MAX_HP;

    [NonSerialized]
    public List<ItemStack> stacks = new List<ItemStack>();

    public Spot GetSpot()
    {
        Debug.Assert(spot != -1, "GetSpot was called, but the spot wasn't assigned to");
        return GameGuard.INSTANCE.boardSpots[spot];
    }

    public void TeleportToSpot(Spot spot)
    {
        this.spot = spot.index;
        Transform transform = GameMaster.INSTANCE.guard.players[id].transform;

        // Preserve player Y
        transform.position = new Vector3(spot.transform.position.x, transform.position.y, spot.transform.position.z);
    }

    public void ModifyHP(int value)
    {
        hp += value;
        if (hp > MAX_HP)
        {
            hp = MAX_HP;
        }

        if (hp <= 0)
        {
            TeleportToSpot(GameMaster.INSTANCE.guard.GetRandomGraveyard());

            hp = MAX_HP;
        }
    }
}
