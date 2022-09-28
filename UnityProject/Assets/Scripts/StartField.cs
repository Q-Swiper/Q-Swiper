using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartField : MapObject
{
    public override AssignedMapObjects Type => AssignedMapObjects.Player;

    public StartField(MapPosition mapPosition) : base(mapPosition)
    {
        NeedPlace = true;
    }
}
