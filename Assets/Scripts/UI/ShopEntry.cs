using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class ShopEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    static readonly Color DISABLED_COLOR = new Color(0.3f, 0.3f, 0.3f);
    static readonly Color IDLE_COLOR = new Color(0.8f, 0.8f, 0.8f);
    static readonly Color SELECTED_COLOR = new Color(1f, 1f, 1f);

    [NonSerialized]
    public new RectTransform transform;
    [NonSerialized]
    public Image image;     // Background
    public Image itemImage; // Foreground
    public TextMeshProUGUI priceText;
    private PlayerData player;
    public ItemStack stack;

    void Awake()
    {
        transform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void Construct(Item item)
    {
        stack = GameMaster.INSTANCE.GetCurrentPlayer().stacks.Find((x) => x.item == item);
        player = GameMaster.INSTANCE.GetCurrentPlayer();
        itemImage.sprite = Resources.Load<Sprite>(item.spritePath);

        priceText.text = item.price + "c";

        if (player.coins < item.price)
        {
            image.color = DISABLED_COLOR;
            itemImage.color = DISABLED_COLOR;
        }
        else
        {
            image.color = IDLE_COLOR;
            itemImage.color = IDLE_COLOR;
        }
    }

    public void OnPointerEnter(PointerEventData e)
    {
        if (player.coins >= stack.item.price)
        {
            image.color = SELECTED_COLOR;
            itemImage.color = SELECTED_COLOR;
        }
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (player.coins >= stack.item.price)
        {
            image.color = IDLE_COLOR;
            itemImage.color = IDLE_COLOR;
        }
    }

    public void OnPointerClick(PointerEventData e)
    {
        if (player.coins < stack.item.price)
        {
            return;
        }

        stack.count++;
        player.coins -= stack.item.price;

        GameMaster.INSTANCE.guard.shopMaster.CloseShop();
    }
}
