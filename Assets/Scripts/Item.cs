public class Item
{
    public string name;
    public string description;

    public Delegates.UseItemDelegate onUse;

    public Item(string name, string description, Delegates.UseItemDelegate onUse)
    {
        this.name = name;
        this.description = description;
        this.onUse = onUse;
    }

    public void Use(PlayerData player)
    {
        onUse(player);
    }
}
