using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceAndMove : MonoBehaviour
{

    public GameObject board;
    public GameObject player;
    public TextMeshProUGUI diceText;
    List<GameObject> positions;
    int currentPosition;
    // Start is called before the first frame update
    void Start()
    {
        positions = new List<GameObject>();
        foreach(Transform child in board.transform)
        {
            if(child.transform.name.StartsWith("pos"))
            {
                positions.Add(child.gameObject);
            }
        }
        currentPosition = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            int dice = Random.Range(1, 7);
            diceText.text = dice.ToString();
            currentPosition += dice;
            currentPosition %= positions.Count;
            Vector3 targetPosition = positions[currentPosition].transform.position;
            player.transform.position = new Vector3(targetPosition.x, player.transform.position.y, targetPosition.z);
        }
    }
}
