public class CloneField : MapObject
{
    public override AssignedMapObjects Type => AssignedMapObjects.CloneField;
    public CloneField(MapPosition mapPosition) : base(mapPosition)
    {
        NeedPlace = false;
    }
}
