using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

namespace Assets.Scripts
{
    public class LevelController
    {
        public Direction lastDirection;
        public List<GamePieceController> GamePieceControllers { get; set; } = new List<GamePieceController>();
        public List<GamePieceCloneController> GamePieceCloneControllers { get; set; } = new List<GamePieceCloneController>();
        public List<List<MapObject>> map;
        public List<DestroyableGameobject> destroyableGameobjects;

        public int TileRowCount { get; private set; }
        public int TileColCount { get; private set; }

        public LevelController(string folderName, int levelNumber)
        {            
            var mapTextRows = LevelFileLoader.LoadLevelFromFile(folderName, levelNumber);

            TileRowCount = HelperMethods.ExtractInt(mapTextRows[0]);
            TileColCount = HelperMethods.ExtractInt(mapTextRows[1]);

            mapTextRows = mapTextRows.Skip(2).ToArray();
            map = TranslateTextIntoMap(mapTextRows, TileRowCount);
            AddGamePiecesOnStartFields();
            destroyableGameobjects = new List<DestroyableGameobject>();
        }

        public void ReactOnUserMove(Direction direction)
        {
            MoveGamePieces(direction);
            lastDirection = direction;
        }
        public bool AreAllCollectablesCollected()
        {
            foreach (var row in map)
            {
                foreach(var mapObject in row)
                {
                    if (mapObject.HasCollectable)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void DestroyGamePieceClone(GamePieceCloneController gamePieceClone)
        {
            GamePieceCloneControllers.Remove(gamePieceClone);
        }

        private void AddGamePiecesOnStartFields()
        {
            foreach (var row in map)
            {
                foreach (var startField in row.OfType<StartField>())
                {
                    var mapPosition = new MapPosition() { X = startField.MapPosition.X, Y = startField.MapPosition.Y };
                    var gamePiece = new GamePieceController(mapPosition);
                    GamePieceControllers.Add(gamePiece);
                }
            }
        }

        private void MoveGamePieces(Direction direction)
        {
            ReduceRemainingCloneMoves();
            bool pieceMoved = true;
            while (pieceMoved)
            {
                foreach (var gamePiece in GamePieceControllers) if (GamePieceIsGoingToMove(gamePiece.MapPosition, direction))
                    {
                        SpawnGamePieceCloneIfShould(gamePiece.MapPosition, gamePiece, direction);
                    }
                foreach (var gamePieceClone in GamePieceCloneControllers) if (!gamePieceClone.IsDestroyable && gamePieceClone.MovesInOpositeDirection && GamePieceCloneCanMove(gamePieceClone, direction))
                    {
                        gamePieceClone.CanMoveIfNoGamePieceComes = true;
                    }
                // move all normal directions
                pieceMoved = MoveAllAbstractGamePiecesOneFieldIfPossible(direction);
                // move all oposite directions
                foreach (var gamePieceClone in GamePieceCloneControllers) if(!gamePieceClone.IsDestroyable && gamePieceClone.MovesInOpositeDirection)
                    {
                        if (GamePieceCloneCanMove(gamePieceClone, direction))
                        {
                            pieceMoved = true;
                            MoveGamePiece(gamePieceClone, direction.GetOppositeDirection());
                            CollectCollectableIfHas(gamePieceClone.MapPosition);
                        }
                        else if (gamePieceClone.CanMoveIfNoGamePieceComes)
                        {
                            var crashPosition = new MapPosition() { X = gamePieceClone.MapPosition.X - direction.X, Y = gamePieceClone.MapPosition.Y - direction.Y };
                            var colidingGamePiece = GetGamePieceByMapPosition(crashPosition);
                            gamePieceClone.IsDestroyable = true;
                            
                            gamePieceClone.IsColidingWithGamePiece = true;
                            gamePieceClone.ColidingGamePiece = colidingGamePiece;
                            
                            MoveGamePieceClone(gamePieceClone, direction);
                        }
                        gamePieceClone.CanMoveIfNoGamePieceComes = false;
                    }
            }
            foreach(var gamePieceClone in GamePieceCloneControllers) if (gamePieceClone.MovesInOpositeDirection)
                {
                    gamePieceClone.MovesInOpositeDirection = false;
                }
        }

        private bool GamePieceIsGoingToMove(MapPosition mapPosition, Direction direction)
        {
            if(FieldInDirectionIsFree(mapPosition, direction))
            {
                return true;
            }
            bool gamePieceFound;
            MapPosition movingPosition = mapPosition;
            do
            {
                movingPosition = new MapPosition() { X = movingPosition.X + direction.X, Y = movingPosition.Y + direction.Y };
                if (GetGamePieceByMapPosition(movingPosition) != null)
                {
                    gamePieceFound = true;
                }
                else
                {
                    gamePieceFound = false;
                }
            } while (gamePieceFound);
            if (MapFieldIsFree(movingPosition.X, movingPosition.Y))
            {
                return true;
            }
            return false;
        }

        private bool MoveAllAbstractGamePiecesOneFieldIfPossible(Direction direction)
        {
            var pieceMoved = false;
            var unmovedAbstractGamePieceControllers = GetAbstractGamePieceControllers();
            int count;
            do
            {
                count = unmovedAbstractGamePieceControllers.Count;
                foreach (var abstractGamePieceController in unmovedAbstractGamePieceControllers.ToArray())
                {
                    if(GamePieceMovesNormally(abstractGamePieceController) && FieldInDirectionIsFree(abstractGamePieceController.MapPosition, direction))
                    {
                        pieceMoved = true;
                        MoveGamePiece(abstractGamePieceController, direction);
                        CollectCollectableIfHas(abstractGamePieceController.MapPosition);
                        unmovedAbstractGamePieceControllers.Remove(abstractGamePieceController);
                    }
                }
            } while (count > unmovedAbstractGamePieceControllers.Count);
            return pieceMoved;
        }

        private bool GamePieceMovesNormally(AbstractGamePieceController abstractGamePieceController)
        {
            if (!(abstractGamePieceController is GamePieceCloneController))
            {
                return true;
            }else
            {
                var clone = abstractGamePieceController as GamePieceCloneController;
                if (!clone.MovesInOpositeDirection && !clone.IsDestroyable)
                {
                    return true;
                }
            }
            return false;
        }

        private AbstractGamePieceController GetGamePieceByMapPosition(MapPosition mapPosititon)
        {
            var abstractGamePieceControllers = GetAbstractGamePieceControllers();
            foreach (var abstractGamePieceController in abstractGamePieceControllers)
            {
                if (abstractGamePieceController.MapPosition.X == mapPosititon.X && abstractGamePieceController.MapPosition.Y == mapPosititon.Y)
                {
                    return abstractGamePieceController;
                }
            }
            return null;
        }
        private List<AbstractGamePieceController> GetAbstractGamePieceControllers()
        {
            var abstractGamePieceControllers = new List<AbstractGamePieceController>();
            foreach(var gamePieceCloneController in GamePieceCloneControllers)
            {
                if (!gamePieceCloneController.IsDestroyable)
                {
                    abstractGamePieceControllers.Add(gamePieceCloneController);
                }
            }
            abstractGamePieceControllers.AddRange(GamePieceControllers);
            return abstractGamePieceControllers;
        }
        private void ReduceRemainingCloneMoves()
        {
            foreach(var gamePieceClone in GamePieceCloneControllers) if (!gamePieceClone.IsDestroyable)
                {
                    if(gamePieceClone.RemainingMoves > 0)
                    {
                        gamePieceClone.ReduceRemainingMoves();
                    }
                    else
                    {
                        MakeGamePiceCloneDestroyable(gamePieceClone);
                    }
                }
        }

        private void MakeGamePiceCloneDestroyable(GamePieceCloneController gamePieceClone)
        {
            map[gamePieceClone.MapPosition.Y][gamePieceClone.MapPosition.X].NeedPlace = false;
            gamePieceClone.IsDestroyable = true;
        }

        private void SpawnGamePieceCloneIfShould(MapPosition mapPosition, GamePieceController spawnedBy, Direction normalDirection)
        {
            if (map[mapPosition.Y][mapPosition.X].Type == MapObject.AssignedMapObjects.CloneField && FieldInDirectionIsFree(mapPosition, normalDirection.GetOppositeDirection()))
            {
                SpawnGamePieceClone(mapPosition, spawnedBy);
            }
        }
        private void SpawnGamePieceClone(MapPosition mapPosition, GamePieceController spawnedBy)
        {
            var newGamePieceClone = new GamePieceCloneController(mapPosition, spawnedBy);
            GamePieceCloneControllers.Add(newGamePieceClone);
        }
        private void MoveGamePiece(AbstractGamePieceController gamePiece, Direction direction)
        {
            map[gamePiece.MapPosition.Y][gamePiece.MapPosition.X].NeedPlace = false;
            int newX = gamePiece.MapPosition.X + direction.X;
            int newY = gamePiece.MapPosition.Y + direction.Y;
            gamePiece.MapPosition = new MapPosition { X = newX, Y = newY };
            map[gamePiece.MapPosition.Y][gamePiece.MapPosition.X].NeedPlace = true;
        }
        private void MoveGamePieceClone(GamePieceCloneController gamePieceClone, Direction direction)
        {
            if (gamePieceClone.MovesInOpositeDirection)
            {
                direction = direction.GetOppositeDirection();
            }
            map[gamePieceClone.MapPosition.Y][gamePieceClone.MapPosition.X].NeedPlace = false;
            int newX = gamePieceClone.MapPosition.X + direction.X;
            int newY = gamePieceClone.MapPosition.Y + direction.Y;
            gamePieceClone.MapPosition = new MapPosition { X = newX, Y = newY };
            map[gamePieceClone.MapPosition.Y][gamePieceClone.MapPosition.X].NeedPlace = true;
        }
        private void CollectCollectableIfHas(MapPosition position)
        {
            if (map[position.Y][position.X].HasCollectable)
            {
                map[position.Y][position.X].HasCollectable = false;
                var destroyableGameobject = new DestroyableGameobject();
                //make collectables destroyable
                destroyableGameobject.Initialize(position, 5);
                destroyableGameobjects.Add(destroyableGameobject);
            }
        }

        private bool FieldInDirectionIsFree(MapPosition position, Direction direction)
        {
            int xToTest = position.X + direction.X;
            int yToTest = position.Y + direction.Y;
            if (MapFieldIsFree(xToTest, yToTest))
            {
                return true;
            }
            return false;
        }

        private bool GamePieceCloneCanMove(GamePieceCloneController gamePieceClone, Direction direction)
        {
            var position = gamePieceClone.MapPosition;
            if (gamePieceClone.MovesInOpositeDirection)
            {
                direction = direction.GetOppositeDirection();
            }
            int xToTest = position.X + direction.X;
            int yToTest = position.Y + direction.Y;
            if (MapFieldIsFree(xToTest, yToTest))
            {
                return true;
            }
            return false;
        }

        private bool MapFieldIsFree(int x, int y)
        {
            if (y < map.Count && y >= 0)
            {
                if (x < map[y].Count && x >= 0)
                {
                    return !map[y][x].NeedPlace;
                }
            }
            return false;
        }

        private List<List<MapObject>> TranslateTextIntoMap(string[] mapTextRows, int tileRowCount)
        {
            var map = new List<List<MapObject>>();
            {
                int y = 0;
                foreach (var mapTextRow in mapTextRows)
                {
                    map.Add(new List<MapObject>());
                    for (var x = 0; x < tileRowCount; x++)
                    {
                        var letter = mapTextRow[x];
                        map[map.Count - 1].Add(MapObject.GetAssignedMapObject(letter, x, y));
                    }
                    y++;
                }
            }
            return map;
        }
    }
}
