using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ItemContainerEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    static readonly Color DISABLED_COLOR = new Color(0.3f, 0.3f, 0.3f);
    static readonly Color IDLE_COLOR = new Color(0.8f, 0.8f, 0.8f);
    static readonly Color SELECTED_COLOR = new Color(1f, 1f, 1f);

    [NonSerialized]
    public new RectTransform transform;
    [NonSerialized]
    public Image image;     // Background
    public Image itemImage; // Foreground
    public TextMeshProUGUI counter;
    public ItemStack stack;

    void Awake()
    {
        transform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void Construct(Item item)
    {
        stack = GameMaster.INSTANCE.GetCurrentPlayer().stacks.Find((x) => x.item == item);
        itemImage.sprite = (Sprite)Resources.Load<Sprite>(stack.item.spritePath);

        if (stack.count == 0)
        {
            image.color = DISABLED_COLOR;
            itemImage.color = DISABLED_COLOR;
            counter.text = "";
        }
        else
        {
            image.color = IDLE_COLOR;
            itemImage.color = IDLE_COLOR;
            counter.text = stack.count.ToString();
        }
    }

    public void OnPointerEnter(PointerEventData e)
    {
        if (stack.count > 0)
        {
            image.color = SELECTED_COLOR;
            itemImage.color = SELECTED_COLOR;
        }
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (stack.count > 0)
        {
            image.color = IDLE_COLOR;
            itemImage.color = IDLE_COLOR;
        }
    }

    public void OnPointerClick(PointerEventData e)
    {
        if(stack.count == 0)
        {
            return;
        }

        stack.item.onUse(GameMaster.INSTANCE.GetCurrentPlayer());
        --stack.count;

        GameMaster.INSTANCE.guard.itemContainerMaster.HideItems();
    }
}
