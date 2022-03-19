using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopRaceFinishLine : MonoBehaviour
{
    [SerializeField]
    HopRaceMaster master;
    new Transform transform;

    bool gameEnded = false;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameEnded)
        {
            return;
        }

        master.players.Sort((p1, p2) => {
            float diff = Mathf.Abs(p1.transform.position.x - transform.position.x) - Mathf.Abs(p2.transform.position.x - transform.position.x);

            if (diff < 0)
            {
                return -1;
            }

            if(diff > 0)
            {
                return 1;
            }

            return 0;
        });

        GameMaster.INSTANCE.minigameScoreboard = new List<PlayerData>();

        for(int i = 0; i < master.players.Count; ++i)
        {
            GameMaster.INSTANCE.minigameScoreboard.Add(master.players[i].data);
        }

        gameEnded = true;

        TaleExtra.MinigameScoreboard();
    }
}
