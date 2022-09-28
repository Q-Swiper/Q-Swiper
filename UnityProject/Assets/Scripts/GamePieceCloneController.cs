namespace Assets.Scripts
{
    public class GamePieceCloneController : AbstractGamePieceController
    {
        public bool CanMoveIfNoGamePieceComes { get; set; } = false;
        public int RemainingMoves { get; set; } = Configurations.MaximumCloneMoves;
        public bool IsDestroyable { get; set; } = false;
        public bool MovesInOpositeDirection { get; set; } = true;
        public GamePieceController SpawnedBy { get; }
        public MapPosition SpawnedOn { get; }
        public bool IsColidingWithGamePiece { get; set; } = false;
        public AbstractGamePieceController ColidingGamePiece { get; set; }

        public GamePieceCloneController(MapPosition mapPosition, GamePieceController spawnedBy) : base(mapPosition) 
        {
            SpawnedBy = spawnedBy;
            SpawnedOn = mapPosition;
        }
        public void ReduceRemainingMoves()
        {
            RemainingMoves -= 1;
        }
    }
}