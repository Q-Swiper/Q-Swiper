using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MapObject
{
    public override AssignedMapObjects Type => AssignedMapObjects.Collectable;

    public Collectable(MapPosition mapPosition) : base(mapPosition)
    {
        NeedPlace = false;
        HasCollectable = true;
    }

}
