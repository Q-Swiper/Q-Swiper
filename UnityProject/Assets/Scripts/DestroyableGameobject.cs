using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableGameobject
{
    public MapPosition MapPosition { get; private set; }
    public GameObject ObjectToDestroy { get; private set; }
    public DateTime MaxLiveTime { get; private set; }
    public void Initialize(MapPosition mapPosition, int maxLifeSeconds)
    {
        MapPosition = mapPosition;
        var StartTime = DateTime.Now;
        MaxLiveTime = StartTime.AddSeconds(maxLifeSeconds);
    }
}
