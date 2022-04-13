using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    private static int playersNum = 0;

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
    public int deaths = 0;

    [NonSerialized]
    public int kills = 0;

    [NonSerialized]
    public int minigames_won = 0;

    [NonSerialized]
    public int minigames_lost = 0;

    public GameMaster.SuperColor superColor;

    [NonSerialized]
    public List<ItemStack> stacks = new List<ItemStack>();

    public PlayerData(string name, GameMaster.SuperColor superColor)
    {
        this.name = name;
        this.superColor = superColor;
        this.id = playersNum++;

        for(int i = 0; i < Item.ITEMS.Count; ++i)
        {
            stacks.Add(new ItemStack(Item.ITEMS[i]));
        }
    }
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
        if (value < 0)
        {
            GameGuard.INSTANCE.DisplayMessage(name + " took " + -value + " damage");
        }
        else
        {
            GameGuard.INSTANCE.DisplayMessage(name + " healed for " + value);
        }
        hp += value;
        if (hp > MAX_HP)
        {
            hp = MAX_HP;
        }

        if (hp <= 0)
        {
            GameGuard.INSTANCE.DisplayMessage(name + " died!");
            TeleportToSpot(GameMaster.INSTANCE.guard.GetRandomGraveyard());
            deaths++;
            hp = MAX_HP;
        }
    }

    public void AddItem(Item item)
    {
        stacks.Find(x => x.item == item).count++;
    }

    public void ModifyCoins(int value)
    {
        coins += value;

        if (coins < 0)
        {
            coins = 0;
        }
    }

    public bool HasCoins(int value)
    {
        return coins >= value;
    }
}
