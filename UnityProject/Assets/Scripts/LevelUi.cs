using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUi : MonoBehaviour
{
    public GameObject EmptyFieldPrefab;
    public GameObject WallFieldPrefab;
    public GameObject CollectableFieldPrefab;
    public GameObject PlayerFieldPrefab;
    public GameObject CloneFieldPrefab;
    public GameObject GamePiecePrefab;
    public GameObject GamePieceClonePrefab;

    public GameObject MapContainer;
    public GameObject TileContainer;

    public float TileSize { get; private set; }
    private float _heightCorrection;
    private float _widthCorrection;
    private float _startBoostStartTime;
    private SolutionsButtonsUiCode _solutionsButtonsUiScript;

    private int _levelNumber;
    private bool _solutionModeOn = false;
    private List<Direction> _solution;
    private int _currentSolutionMove;

    public bool GamePaused { get; set; } = false;
    public int StandardGameSpeed { get; private set; }
    private List<List<GameObject>> MapFields { get; set; }
    private LevelController LevelController { get; set; }
    private List<GamePieceUi> GamePieceUis { get; set; } = new List<GamePieceUi>();
    private List<GamePieceCloneActivator> GamePieceCloneActivators { get; set; } = new List<GamePieceCloneActivator>();

    public Direction LastDirection { get 
        { return LevelController.lastDirection; } 
    }
    public List<GamePieceController> GamePieceControllers { get
        { return LevelController.GamePieceControllers; }
    }
    public List<GamePieceCloneController> GamePieceCloneControllers { get
        { return LevelController.GamePieceCloneControllers; }
    }
    public List<List<MapObject>> Map { get
        { return LevelController.map; }
    }
    public List<DestroyableGameobject> DestroyableGameobjects{ get
        { return LevelController.destroyableGameobjects; }
    }

    public void Update()
    {
        RecalculateGamePieceSpeed();
        if (GamePieceUi.HaveAllFinishedMovement(GamePieceUis))
        {
            GamePieceUi.ReactivateMovementOfAll(GamePieceUis);
            GamePieceCloneActivator.ReduceMovesUntilActive(GamePieceCloneActivators);
        }
        RemoveDestroyableClones();
        RemoveDestroyableGameobjectsIfCollides();
        RemoveGamePieceCloneIfColidesWithGamePiece();
        ShowClonesIfMovesOverCloneField();
    }
    public void LoadLevel(string folderName, int levelNumber)
    {
        _levelNumber = levelNumber;
        LevelController = new LevelController(folderName, levelNumber);

        float tileContainerWidth = HelperMethods.GetWidth(TileContainer);
        float tileContainerHeight = HelperMethods.GetHeight(TileContainer);
        TileSize = CalcTileSize(tileContainerWidth, tileContainerHeight, LevelController.TileRowCount, LevelController.TileColCount);
        _heightCorrection = (tileContainerHeight - TileSize * Map.Count) / 2;

        PlaceTileContainerCentered(LevelController.TileRowCount);
        LoadGamePieceUiPositions();
        DrawMapOnUI();

        CalculateStandardGameSpeed();
    }
    public void LoadLevel(string folderName, int levelNumber, SolutionsButtonsUiCode solutionsButtonsUiScript)
    {
        _solutionsButtonsUiScript = solutionsButtonsUiScript;
        LoadLevel(folderName, levelNumber);
    }

    private void RemoveGamePieceCloneIfColidesWithGamePiece()
    {        
        foreach (var gamePieceUi in GamePieceUis) if(gamePieceUi.AbstractGamePieceController is GamePieceCloneController)
            {
                var cloneController = gamePieceUi.AbstractGamePieceController as GamePieceCloneController;
                if (cloneController.IsColidingWithGamePiece)
                {
                    var otherGamePiece = GamePieceUi.GetGamePieceUiByAbstractGamePieceController(GamePieceUis, cloneController.ColidingGamePiece);
                    if (IsColidingWith(gamePieceUi, otherGamePiece))
                    {
                        cloneController.RemainingMoves = 0;
                        gamePieceUi.Destroy();
                    }                   
                }
            }   
    }
    private bool IsColidingWith(GamePieceUi gamePieceUi, GamePieceUi otherGamePieceUi)
    {
        var gamePieceSize = gamePieceUi.GetSize();

        var positionToCheck = gamePieceUi.GetAnchoredPosition();
        var otherPosition = otherGamePieceUi.GetAnchoredPosition();
        
        var distanceX = Math.Abs(positionToCheck.x - otherPosition.x);
        var distanceY = Math.Abs(positionToCheck.y - otherPosition.y);
        if (distanceX < gamePieceSize.x -5 && distanceY < gamePieceSize.y -5)
        {
            return true;
        }
        return false;
    }

    private void RemoveDestroyableClones()
    {
        foreach(var gamePieceCloneUi in GamePieceUis.ToArray()) if (gamePieceCloneUi.IsDestroyable)
            {
                GamePieceUis.Remove(gamePieceCloneUi);
            }
    }

    public bool AnyGamepieceIsMoving()
    {
        return GamePieceUi.GetLargestCountOfTargetedPositions(GamePieceUis) != 0;
    }

    public void ReactOnUserMovesRight()
    {
        var direction = new Direction(Direction.DirectionLetter.Right);
        ReactOnUserMove(direction);
    }

    public void ReactOnUserMovesLeft()
    {
        var direction = new Direction(Direction.DirectionLetter.Left);
        ReactOnUserMove(direction);
    }
    public void ReactOnUserMovesUp()
    {
        var direction = new Direction(Direction.DirectionLetter.Up);
        ReactOnUserMove(direction);
    }
    public void ReactOnUserMovesDown()
    {
        var direction = new Direction(Direction.DirectionLetter.Down);
        ReactOnUserMove(direction);
    }
    private void ReactOnUserMove(Direction direction)
    {
        if (!GamePaused)
        {
            if (!_solutionModeOn)
            {
                LevelController.ReactOnUserMove(direction);
                RecalculateMovementOfGamePieces();
            }
            else
            {
                if (_solution[_currentSolutionMove].X == direction.X && _solution[_currentSolutionMove].Y == direction.Y)
                {
                    if(_currentSolutionMove < _solution.Count - 1)
                    {
                        _currentSolutionMove += 1;
                        ShowNextSolutionDirection();
                    }
                    else
                    {
                        HideSolutionDirection();
                    }
                    LevelController.ReactOnUserMove(direction);
                    RecalculateMovementOfGamePieces();
                }
                else
                {
                    //To Do
                    //exit solution menu
                }
            }
        }
    }
    private void RecalculateMovementOfGamePieces()
    {
        foreach (var gamePieceCloneController in GamePieceCloneControllers)
        {
            if(gamePieceCloneController.RemainingMoves == Configurations.MaximumCloneMoves)
            {
                var gamePiece = GamePieceUi.GetGameObjectByGamePieceController(GamePieceUis, gamePieceCloneController.SpawnedBy);
                var cloneField = MapFields[gamePieceCloneController.SpawnedOn.Y][gamePieceCloneController.SpawnedOn.X];
                GamePieceCloneActivators.Add(new GamePieceCloneActivator(gamePiece, cloneField, gamePieceCloneController, GamePieceUi.GetLargestCountOfTargetedPositions(GamePieceUis)));
            }
        }
        foreach (var gamePieceUi in GamePieceUis)
        {
            gamePieceUi.SetTargetPosition(CalcPixelPositionFromMapPosition(gamePieceUi.AbstractGamePieceController.MapPosition.X, gamePieceUi.AbstractGamePieceController.MapPosition.Y));
        }
        foreach (var gamePieceCloneActivator in GamePieceCloneActivators)
        {
            gamePieceCloneActivator.AddCurrentPositionToTargetPositions();
        }

        _startBoostStartTime = Time.time;
    }

    private void AddNewGamePieceUiToGamePieceUis(GamePieceController gamePieceController)
    {
        var gamePieceUi = gameObject.AddComponent<GamePieceUi>();
        var pixelPosition = CalcPixelPositionFromMapPosition(gamePieceController.MapPosition.X, gamePieceController.MapPosition.Y);
        gamePieceUi.Initialize(MapContainer, GamePiecePrefab, pixelPosition, TileSize, gamePieceController);
        GamePieceUis.Add(gamePieceUi);
    }
    private void AddNewGamePieceUiToGamePieceUis(GamePieceCloneController gamePieceCloneController, List<MapPosition> targetPositions) // clone spawnt am SwpawnedOn
    {
        var gamePieceUi = gameObject.AddComponent<GamePieceUi>();
        var pixelPosition = CalcPixelPositionFromMapPosition(gamePieceCloneController.SpawnedOn.X, gamePieceCloneController.SpawnedOn.Y);
        gamePieceUi.Initialize(MapContainer, GamePieceClonePrefab, pixelPosition, TileSize, gamePieceCloneController);

        foreach (var targetPosition in targetPositions)
        {
            gamePieceUi.SetTargetPosition(CalcPixelPositionFromMapPosition(targetPosition.X, targetPosition.Y));
        }
        
        gamePieceUi.ReactivateMovement();
        GamePieceUis.Add(gamePieceUi);
    }

    public float CalcTileSize(float width, float height, int rowCount, int colCount)
    {
        var smallerSize = Mathf.Min(width / rowCount, height / colCount);
        return smallerSize;
    }
    private void LoadGamePieceUiPositions()
    {
        foreach(var gamePieceController in GamePieceControllers)
        {
            AddNewGamePieceUiToGamePieceUis(gamePieceController);
        }
    }
    private void DrawMapOnUI()
    {
        MapFields = new List<List<GameObject>>();
        for (var y = 0; y < Map.Count; y++)
        {
            var row = Map[y];
            MapFields.Add(new List<GameObject>());
            foreach (var field in row)
            {
                GameObject prefab;
                switch (field.Type)
                {
                    case MapObject.AssignedMapObjects.EmptyField:
                        prefab = Instantiate(EmptyFieldPrefab) as GameObject;
                        break;
                    case MapObject.AssignedMapObjects.Wall:
                        prefab = Instantiate(WallFieldPrefab) as GameObject;
                        break;
                    case MapObject.AssignedMapObjects.Collectable:
                        prefab = Instantiate(CollectableFieldPrefab) as GameObject;
                        break;
                    case MapObject.AssignedMapObjects.Player:
                        prefab = Instantiate(PlayerFieldPrefab) as GameObject;
                        break;
                    case MapObject.AssignedMapObjects.CloneField:
                        prefab = Instantiate(CloneFieldPrefab) as GameObject;
                        break;
                    default:
                        Debug.Log("Error: unexpected field.Type . Replaced with emty");
                        prefab = Instantiate(EmptyFieldPrefab) as GameObject;
                        break;
                }
                prefab.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TileSize);
                prefab.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TileSize);
                prefab.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(field.MapPosition.X * TileSize, -field.MapPosition.Y * TileSize - _heightCorrection), new Quaternion());
                prefab.transform.SetParent(TileContainer.transform, false);
                MapFields[y].Add(prefab);
            }
        }        
    }
    private void PlaceTileContainerCentered(int tileRowCount) {
        var parentsWidth = HelperMethods.GetWidth(TileContainer.transform.parent.gameObject);
        var width = tileRowCount*TileSize;
        _widthCorrection = (parentsWidth - width) / 2;
        TileContainer.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        TileContainer.transform.GetComponent<RectTransform>().offsetMin = new Vector2(_widthCorrection, 0);
        TileContainer.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
    }

    public Vector3 CalcPixelPositionFromMapPosition(int x, int y)
    {
        return new Vector3(x * TileSize + TileSize / 2 + _widthCorrection, -y * TileSize - TileSize / 2 - _heightCorrection);
    }

    public void RemoveDestroyableGameobjectsIfCollides()
    {
        foreach (var destroyableGameobject in DestroyableGameobjects)
        {
            if (DestroyableGameobjectIsCollidedWithGamePiece(MapFields[destroyableGameobject.MapPosition.Y][destroyableGameobject.MapPosition.X])) //DateTime.Now > destroyableGameobject.MaxLifeTime || 
            {
                HelperMethods.RemoveAllChildsFromGameobject(MapFields[destroyableGameobject.MapPosition.Y][destroyableGameobject.MapPosition.X]);
            }
        }
    }
    public bool DestroyableGameobjectIsCollidedWithGamePiece(GameObject gameObject)
    {
        foreach (var gamePieceUi in GamePieceUis)
        {
            //Debug.Log(gamePiece.gamePiece.GetComponent<RectTransform>().anchoredPosition.x);
            var gamePiecePosition = gamePieceUi.GetAnchoredPosition();
            var xDistance = Math.Abs(gamePiecePosition.x - _widthCorrection - gameObject.GetComponent<RectTransform>().anchoredPosition.x - TileSize / 2);
            var yDistance = Math.Abs(gamePiecePosition.y - gameObject.GetComponent<RectTransform>().anchoredPosition.y + TileSize / 2);
            if (xDistance < TileSize / 2 && yDistance < TileSize / 2)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsLevelCompleted()
    {
        var NoCollectables = true;
        foreach (var row in Map)
        {
            foreach (var field in row)
            {
                if (field.HasCollectable)
                {
                    NoCollectables = false;
                }
            }
        }
        return NoCollectables;
    }
    private void CalculateStandardGameSpeed()
    {
        var tileSize = TileSize;
        var tileContainerSize = HelperMethods.GetWidth(TileContainer);
        StandardGameSpeed = (int)((tileContainerSize/2 + 2*tileSize) * Configurations.swipeSpeed);
    }
    private void RecalculateGamePieceSpeed() {
        var openMoves = GamePieceUi.GetLargestCountOfTargetedPositions(GamePieceUis);
        var speed = (int)(Math.Sqrt(openMoves) * StandardGameSpeed) * CalculateStartBoost();
        GamePieceUi.SetSpeedOfAll(speed, GamePieceUis);
    }
    private float CalculateStartBoost()
    {
        float boost;
        var now = Time.time;
        float timeDifference = now - _startBoostStartTime;
        if(timeDifference < .2)
        {
            boost =  (float)(1 + Math.Pow(2 - (10 * timeDifference), 3)/4);
        }
        else {
            boost = 1;
        }
        return boost;
    }
    private void ShowClonesIfMovesOverCloneField()
    {
        foreach(var gamePieceCloneActivator in GamePieceCloneActivators.ToArray())
        {
            if (gamePieceCloneActivator.MinimumDistanceIsPassed())
            {
                AddNewGamePieceUiToGamePieceUis(gamePieceCloneActivator.GamePieceCloneController, gamePieceCloneActivator.CloneTargetPositions);

                GamePieceCloneActivators.Remove(gamePieceCloneActivator);
            }
        }
    }
    public void ActivateSolutionMode()
    {
        var solution = SolutionFileLoader.LoadSolutionFromFile(Configurations.solutionsFolder, _levelNumber);
        if (solution.Count == 0)
        {
            Debug.Log("Aktivierte Lösung ist nicht vorhanden: " + _levelNumber);
            return;
        }
        _solution = solution;
        _currentSolutionMove = 0;
        _solutionModeOn = true;
        ShowNextSolutionDirection();        
    }

    private void ShowNextSolutionDirection()
    {
        SolutionsButtonsUiCode.Direction direction;
        switch (_solution[_currentSolutionMove].GetDirectionLetter())
        {
            case Direction.DirectionLetter.Up:
                direction = SolutionsButtonsUiCode.Direction.Up;
                break;
            case Direction.DirectionLetter.Down:
                direction = SolutionsButtonsUiCode.Direction.Down;
                break;
            case Direction.DirectionLetter.Right:
                direction = SolutionsButtonsUiCode.Direction.Right;
                break;
            case Direction.DirectionLetter.Left:
                direction = SolutionsButtonsUiCode.Direction.Left;
                break;
            default:
                direction = SolutionsButtonsUiCode.Direction.InvalidDirection;
                break;
        }
        _solutionsButtonsUiScript.ShowSolutionDirection(direction);
    }

    private void HideSolutionDirection()
    {
        _solutionsButtonsUiScript.DeactivateAllSolutionArrows();
    }
}
