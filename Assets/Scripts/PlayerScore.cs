using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public PlayerData data;

    private void Start()
    {
        transform.Find("PlayerName").gameObject.GetComponent<TextMeshProUGUI>().text = data.name;
        transform.Find("PlayerName").gameObject.GetComponent<TextMeshProUGUI>().color = data.superColor.color;
    }
    void Update()
    {
        transform.Find("PlayerHealth").gameObject.GetComponent<TextMeshProUGUI>().text = data.hp + " health";
        transform.Find("PlayerCrowns").gameObject.GetComponent<TextMeshProUGUI>().text = data.crowns + "/3 crowns";
    }
}
