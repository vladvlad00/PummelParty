using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorRainMeteor : MonoBehaviour
{
    private const float Y_LIMIT = -220f;
    void Update()
    {
        if (GetComponent<Rigidbody2D>().position.y <= Y_LIMIT)
        {
            Debug.Log("Destroyed");
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Meteor"))
            return;
        GameObject masterGameObject = GameObject.Find("MeteorRainMaster");
        MeteorRainMaster master = masterGameObject.GetComponent<MeteorRainMaster>();
        master.destroyPlayer(collision.gameObject);
        Destroy(gameObject);
    }
}
