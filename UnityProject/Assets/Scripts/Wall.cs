public class Wall : MapObject
{
    public override AssignedMapObjects Type => AssignedMapObjects.Wall;
    
    public Wall(MapPosition mapPosition) : base(mapPosition)
    {
        NeedPlace = true;
    }
}
