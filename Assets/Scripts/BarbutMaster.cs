using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using TMPro;

public class BarbutMaster : MonoBehaviour
{
    public enum State
    {
        WAIT_FOR_INPUT,
        RESULTS
    }

    class PlayerRoll
    {
        public TextMeshProUGUI name;
        public TextMeshProUGUI dice1;
        public TextMeshProUGUI dice2;

        public PlayerRoll(TextMeshProUGUI name, TextMeshProUGUI dice1, TextMeshProUGUI dice2)
        {
            this.name = name;
            this.dice1 = dice1;
            this.dice2 = dice2;
        }
    }

    private const float PLAYER_ROLL_Y_DIFF = 120f;

    [NonSerialized]
    public GameMaster master;

    [SerializeField]
    GameObject promptCanvas;

    [SerializeField]
    GameObject playAreaCanvas;
    Transform playAreaCanvasTransform;

    [SerializeField]
    GameObject playerRollPrefab;

    State state;

    List<PlayerRoll> playerRolls;

    void Awake()
    {
        master = GameMaster.INSTANCE;
        playerRolls = new List<PlayerRoll>();

        playAreaCanvasTransform = playAreaCanvas.GetComponent<Transform>();

        Vector3 basePos = new Vector3(-860f, 460f, 0f);

        for(int i = 0; i < master.minigameTriggerPlayers.Count; ++i)
        {
            GameObject obj = Instantiate(playerRollPrefab, basePos, Quaternion.identity);

            RectTransform transform = obj.GetComponent<RectTransform>();
            transform.SetParent(playAreaCanvasTransform, false);

            playerRolls.Add(new PlayerRoll(
                transform.Find("Name").GetComponent<TextMeshProUGUI>(),
                transform.Find("Dice1").Find("Number").GetComponent<TextMeshProUGUI>(),
                transform.Find("Dice2").Find("Number").GetComponent<TextMeshProUGUI>())

            );

            playerRolls[i].name.text = string.Format("Player {0}", master.minigameTriggerPlayers[i].id);

            basePos.y -= PLAYER_ROLL_Y_DIFF;
        }
    }

    void Update()
    {
        switch(state)
        {
            case State.WAIT_FOR_INPUT:
                WaitForInput();
                break;
            case State.RESULTS:
                Results();
                break;
        }
    }

    void WaitForInput()
    {
        if(GameMaster.InputEnabled() && Input.GetKeyDown(KeyCode.Space))
        {
            promptCanvas.SetActive(false);

            List<int> rollSums = new List<int>();
            int maxIndex = -1;

            for (int j = 0; j < master.minigameTriggerPlayers.Count; ++j)
            {
                int roll1 = UnityEngine.Random.Range(1, 7);
                int roll2 = UnityEngine.Random.Range(1, 7);
                rollSums.Add(roll1 + roll2);

                if (maxIndex == -1 || rollSums[maxIndex] < roll1 + roll2)
                {
                    maxIndex = rollSums.Count - 1;
                }

                playerRolls[j].dice1.text = roll1.ToString();
                playerRolls[j].dice2.text = roll2.ToString();
            }

            List<Player> winners = new List<Player>();

            for (int j = 0; j < master.minigameTriggerPlayers.Count; ++j)
            {
                if (rollSums[j] == rollSums[maxIndex])
                {
                    playerRolls[j].name.text = string.Format("<color=#008800>{0}</color>", playerRolls[j].name.text);
                    winners.Add(master.minigameTriggerPlayers[j]);
                }
            }

            state = State.RESULTS;
        }
    }

    void Results()
    {
        if (GameMaster.InputEnabled() && Input.GetKeyDown(KeyCode.Space))
        {
            master.OnSpotMinigameEnd();

            TaleExtra.DisableInput();
            TaleExtra.RipOut();
            TaleExtra.ReturnToGame();
            TaleExtra.RipIn();
            TaleExtra.EnableInput();
        }
    }
}
