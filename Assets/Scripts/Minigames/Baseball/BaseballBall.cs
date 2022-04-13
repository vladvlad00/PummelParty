using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBall : MonoBehaviour
{
    private bool shouldSmall;
    private float modifier;
    private new Transform transform;
    public bool hit = false;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        shouldSmall = false;
        modifier = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldSmall)
        {
            float newScale = transform.localScale.x - modifier;
            if (newScale < 0.1f)
            {
                newScale = 0.1f; 
            }
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    public void startSmall(float modif)
    {
        shouldSmall = true;
        modifier = modif;
    }

    public void stopSmall()
    {
        shouldSmall = false;
        transform.localScale = new Vector3(5, 5, 5);
    }
}
