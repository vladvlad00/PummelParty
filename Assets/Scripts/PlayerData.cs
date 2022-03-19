using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public int id;
    public string name;
    [NonSerialized]
    public int spot = -1;

    public Spot GetSpot()
    {
        Debug.Assert(spot != -1, "GetSpot was called, but the spot wasn't assigned to");
        return GameGuard.INSTANCE.boardSpots[spot];
    }
}
