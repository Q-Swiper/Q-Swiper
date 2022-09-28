namespace Assets.Scripts
{
    public abstract class AbstractGamePieceController
    {
        public MapPosition MapPosition { get; set; }
        public AbstractGamePieceController(MapPosition mapPosition)
        {
            MapPosition = mapPosition;
        }
    }
}
