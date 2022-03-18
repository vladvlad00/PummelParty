using UnityEngine;
using System;

public class SelectSpotArrow : MonoBehaviour
{
    [NonSerialized]
    public Spot spot;

    [SerializeField]
    GameObject glow;

    public void OnMouseOver()
    {
        glow.SetActive(true);
    }

    public void OnMouseExit()
    {
        glow.SetActive(false);
    }

    public void OnMouseDown()
    {
        GameMaster.INSTANCE.OnPlayerSelectedSpot(spot);
    }
}
