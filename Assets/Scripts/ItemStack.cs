public class ItemStack
{
    public Item item;
    public int count;

    public ItemStack(Item item)
    {
        this.item = item;
        this.count = 0;
    }
}