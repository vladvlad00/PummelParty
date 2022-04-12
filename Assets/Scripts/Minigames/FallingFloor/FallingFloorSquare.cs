using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloorSquare : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject masterGameObject = GameObject.Find("FallingFloorMaster");
        FallingFloorMaster master = masterGameObject.GetComponent<FallingFloorMaster>();
        master.destroyPlayer(collision.gameObject);
    }
}
