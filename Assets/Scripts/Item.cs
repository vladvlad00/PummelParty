using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string name;
    public string description;
    public string spritePath;
    public int price;

    public Delegates.UseItemDelegate onUse;

    public Item(string name, string description, string spritePath, int price, Delegates.UseItemDelegate onUse)
    {
        this.name = name;
        this.description = description;
        this.spritePath = spritePath;
        this.price = price;
        this.onUse = onUse;
    }

    public void Use(PlayerData player)
    {
        onUse(player);
    }

    public static readonly List<Item> ITEMS = new List<Item>()
    {
        new Item("Medkit", "Heals 10hp", "Sprites/UI/ItemMedkit", 10, (player) =>
        {
            player.ModifyHP(10);
            GameMaster.INSTANCE.guard.DisplayMessage(string.Format("{0} healed 10 health", player.name));
        }),
        new Item("Sniper", "Deals 10 damage to a random player", "Sprites/UI/ItemSniper", 20, (player) =>
        {
            PlayerData other;

            do
            {
                other = GameMaster.INSTANCE.playerData[UnityEngine.Random.Range(0, GameMaster.INSTANCE.playerData.Count)];
            } while(other.id == player.id);

            other.ModifyHP(-10);
            GameMaster.INSTANCE.guard.DisplayMessage(string.Format("{0} sniped {1} for 10 health", player.name, other.name));
        }),
        new Item("Teleporter", "Swaps your place with someone else's", "Sprites/UI/ItemTeleporter", 30, (player) =>
        {
            PlayerData other;

            do
            {
                other = GameMaster.INSTANCE.playerData[UnityEngine.Random.Range(0, GameMaster.INSTANCE.playerData.Count)];
            } while(other.id == player.id);

            int otherSpot = other.spot;

            other.TeleportToSpot(GameMaster.INSTANCE.guard.boardSpots[player.spot]);
            player.TeleportToSpot(GameMaster.INSTANCE.guard.boardSpots[otherSpot]);
            GameMaster.INSTANCE.guard.DisplayMessage(string.Format("{0} swapped places with {1}", player.name, other.name));
        })
    };
}
