using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMaster : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;

    List<GameObject> items;

    const int itemsInShop = 2;

    void Awake()
    {
        items = new List<GameObject>();
    }

    public void OpenShop()
    {
        ShopEntry dummy = Instantiate(itemPrefab, new Vector3(9999f, 9999f, 9999f), Quaternion.identity).GetComponent<ShopEntry>();

        RectTransform tform = dummy.transform;

        float width = tform.rect.width;
        float height = tform.rect.height;

        Destroy(dummy.gameObject);

        float totalWidth = width * Item.ITEMS.Count;

        bool[] usedItems = new bool[Item.ITEMS.Count];

        for (int i = 0; i < itemsInShop; ++i)
        {
            float x = (-totalWidth / 2) + (1 + 2 * i) * (width / 2);
            float y = 0.0f;

            GameObject obj = Instantiate(itemPrefab, new Vector3(x, y, 0.0f), Quaternion.identity);
            obj.GetComponent<RectTransform>().SetParent(GameMaster.INSTANCE.guard.shopContainer, false);

            int index = UnityEngine.Random.Range(0, Item.ITEMS.Count);
            while (usedItems[index])
            {
                index = UnityEngine.Random.Range(0, Item.ITEMS.Count);
            }
            obj.GetComponent<ShopEntry>().Construct(Item.ITEMS[index]);
            usedItems[index] = true;

            items.Add(obj);
        }
    }

    public void CloseShop()
    {
        for (int i = 0; i < items.Count; ++i)
        {
            Destroy(items[i]);
        }

        items.Clear();
    }
}
