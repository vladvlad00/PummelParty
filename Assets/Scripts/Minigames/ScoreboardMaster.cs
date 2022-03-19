using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreboardMaster : MonoBehaviour
{
    public const float PLAYER_PLACEMENT_Y_DIFF = 1.5f;
    public static readonly string[] PLAYER_PLACEMENT_COLORS = new string[]
    {
        "FFC53A",
        "C9D6EA",
        "B26700"
    };

    [SerializeField]
    GameObject playerPlacementPrefab;

    void Awake()
    {
        Vector3 basePos = new Vector3(0f, 3f, 0f);

        for(int i = 0; i < GameMaster.INSTANCE.minigameScoreboard.Count; ++i)
        {
            string color = "FFFFFF";

            if(i < PLAYER_PLACEMENT_COLORS.Length)
            {
                color = PLAYER_PLACEMENT_COLORS[i];
            }

            PlayerData data = GameMaster.INSTANCE.minigameScoreboard[i];

            GameObject obj = Instantiate(playerPlacementPrefab, basePos, Quaternion.identity);
            obj.GetComponent<TextMeshPro>().text = string.Format("<color=#{0}>{1}. {2}</color>", color, (i + 1).ToString(), data.name);

            basePos.y -= PLAYER_PLACEMENT_Y_DIFF;
        }
    }

    void Update()
    {
        if(InputMaster.GetKeyDown(KeyCode.Space))
        {
            TaleExtra.ReturnFromMinigame();
        }
    }
}
