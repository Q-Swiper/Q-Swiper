using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyField : MapObject
{
    public override AssignedMapObjects Type => AssignedMapObjects.EmptyField;

    public EmptyField(MapPosition mapPosition) : base(mapPosition)
    {
        NeedPlace = false;
    }
}
