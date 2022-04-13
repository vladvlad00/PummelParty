using System.Collections.Generic;
using UnityEngine;

public class ItemContainerMaster : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;

    List<GameObject> instantiated;

    void Awake()
    {
        instantiated = new List<GameObject>();
    }

    public void ShowItems()
    {
        ItemContainerEntry dummy = Instantiate(itemPrefab, new Vector3(9999f, 9999f, 9999f), Quaternion.identity).GetComponent<ItemContainerEntry>();

        RectTransform tform = dummy.transform;

        float width = tform.rect.width;
        float height = tform.rect.height;

        Destroy(dummy.gameObject);

        float totalWidth = width * Item.ITEMS.Count;

        for(int i = 0; i < Item.ITEMS.Count; ++i)
        {
            float x = (-totalWidth / 2) + (1 + 2 * i) * (width / 2);
            float y = 0.0f;

            GameObject obj = Instantiate(itemPrefab, new Vector3(x, y, 0.0f), Quaternion.identity);
            obj.GetComponent<RectTransform>().SetParent(GameMaster.INSTANCE.guard.itemContainer, false);

            obj.GetComponent<ItemContainerEntry>().Construct(Item.ITEMS[i]);

            instantiated.Add(obj);
        }
    }

    public void HideItems()
    {
        for(int i = 0; i < instantiated.Count; ++i)
        {
            Destroy(instantiated[i]);
        }

        instantiated.Clear();
    }
}
