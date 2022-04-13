using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartShow : MonoBehaviour
{
    [SerializeField]
    RectTransform canvasTransform;

    private List<GameObject[]> playerScore;
    private float opacity = -2f;

    void Start()
    {
        List<PlayerData> players = GameMaster.INSTANCE.playerData;
        players.Sort((p1, p2) => {
            int diff = p2.crowns - p1.crowns;

            if (diff == 0)
            {
                diff = p2.coins - p1.coins;

                if (diff < 0)
                {
                    return -1;
                }
                if (diff > 0)
                {
                    return 1;
                }
                return 0;
            }

            if (diff < 0)
            {
                return -1;
            }
            else 
            {
                return 1;
            }
        });
        playerScore = new List<GameObject[]>();

        int place = 1;
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerData data = players[i];

            GameObject[] texts = new GameObject[8];
            for (int j = 0; j < 8; ++j)
            {
                texts[j] = new GameObject();
                texts[j].AddComponent<TextMeshProUGUI>();
                texts[j].transform.localPosition = new Vector3(j * 200 - 1920 / 2 + 128, 1080 / 2 - i * 100 - 64, 1);
                texts[j].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0);
                texts[j].GetComponent<TextMeshProUGUI>().outlineWidth = 0f;
                texts[j].GetComponent<TextMeshProUGUI>().fontSize = 30;
                texts[j].transform.SetParent(canvasTransform, false);
            }

            texts[0].GetComponent<TextMeshProUGUI>().text = ToRoman(place) + ".";
            texts[0].GetComponent<TextMeshProUGUI>().horizontalAlignment = HorizontalAlignmentOptions.Center;
            texts[1].GetComponent<TextMeshProUGUI>().text = data.name;
            texts[1].GetComponent<TextMeshProUGUI>().color = new Color(data.superColor.color.r, data.superColor.color.g, data.superColor.color.b, 0);
            texts[1].GetComponent<TextMeshProUGUI>().outlineColor = new Color(data.superColor.outline.r, data.superColor.outline.g, data.superColor.outline.b, 0);
            texts[1].GetComponent<TextMeshProUGUI>().outlineWidth = 0.1f;
            texts[2].GetComponent<TextMeshProUGUI>().text = "Crowns: " + data.crowns.ToString();
            texts[3].GetComponent<TextMeshProUGUI>().text = "Coins: " + data.coins.ToString();
            texts[4].GetComponent<TextMeshProUGUI>().text = "Deaths: " + data.deaths.ToString();
            texts[5].GetComponent<TextMeshProUGUI>().text = "Kills: " + data.kills.ToString();
            texts[6].GetComponent<TextMeshProUGUI>().text = "Wins: " + data.minigames_won.ToString();
            texts[7].GetComponent<TextMeshProUGUI>().text = "Loses: " + data.minigames_lost.ToString();

            if (place == 1)
            {
                texts[0].GetComponent<TextMeshProUGUI>().color = new Color(204f / 255f, 164f / 255f, 61f / 255f, 0);
            }
            else if (place == 2)
            {
                texts[0].GetComponent<TextMeshProUGUI>().color = new Color(196f / 255f, 202f / 255f, 206f / 255f, 0);
            }
            else if (place == 3)
            {
                texts[0].GetComponent<TextMeshProUGUI>().color = new Color(191f / 255f, 137f / 255f, 112f / 255f, 0);
            }

            if (i + 1 < players.Count)
            {
                if (!(players[i].crowns == players[i + 1].crowns && players[i].coins == players[i + 1].coins))
                {
                    place++;
                }
            }

            playerScore.Add(texts);
        }
    }

    // Update is called once per frame
    void Update()
    {
        opacity += 0.25f;
        for (int i = 0; i < playerScore.Count; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                Color color = playerScore[i][j].GetComponent<TextMeshProUGUI>().color;
                playerScore[i][j].GetComponent<TextMeshProUGUI>().color = new Color(color.r, color.g, color.b, Mathf.Min(1, Mathf.Max(0, opacity / 60 - i)));
                Color outline = playerScore[i][j].GetComponent<TextMeshProUGUI>().outlineColor;
                playerScore[i][j].GetComponent<TextMeshProUGUI>().outlineColor = new Color(outline.r, outline.g, outline.b, Mathf.Min(1, Mathf.Max(0, opacity / 60 - i)));
            }
        }
    }

    public static string ToRoman(int number)
    {
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        return "XIV";
    }
}
