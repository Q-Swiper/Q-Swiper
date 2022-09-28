using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GamePieceCloneActivator
{
    private readonly GameObject gamePiece;
    private readonly GameObject cloneField;
    public GamePieceCloneController GamePieceCloneController { get; private set; }
    public List<MapPosition> CloneTargetPositions { get; private set; } = new List<MapPosition>();

    private bool isActive;
    private int movesUntilActive;

    private float lastDistance = 10000;
    private bool minimumDistanceIsPassed = false;
    public GamePieceCloneActivator(GameObject gamePiece, GameObject cloneField, GamePieceCloneController gamePieceCloneController, int movesUntilActive)
    {
        this.gamePiece = gamePiece;
        this.cloneField = cloneField;
        GamePieceCloneController = gamePieceCloneController;
        if(movesUntilActive > 0)
        {
            isActive = false;
            this.movesUntilActive = movesUntilActive;
        }
        else
        {
            isActive = true;
            this.movesUntilActive = 0;
        }
    }
    public bool MinimumDistanceIsPassed()
    {
        if(isActive == false)
        {
            return false;
        }
        if (minimumDistanceIsPassed)
        {
            return true;
        }
        var gamePiecePosition = gamePiece.GetComponent<RectTransform>().anchoredPosition;
        
        var cloneFieldPosition = cloneField.GetComponent<RectTransform>().anchoredPosition;

        var distanceX = Math.Abs(gamePiecePosition.x - cloneFieldPosition.x);
        var distanceY = Math.Abs(gamePiecePosition.y - cloneFieldPosition.y);
        var currentDistance = distanceX + distanceY;
        if(currentDistance < lastDistance)
        {
            lastDistance = currentDistance;
        }
        else
        {
            minimumDistanceIsPassed = true;
            return true;
        }
        return false;
    }
    private void ReduceMovesUntilActive()
    {
        if(movesUntilActive > 1)
        {
            movesUntilActive--;
        }
        else
        {
            isActive = true;
            movesUntilActive = 0;
        }
    }
    public static void ReduceMovesUntilActive(List<GamePieceCloneActivator> gamePieceCloneActivators)
    {
        foreach (var gamePieceCloneActivator in gamePieceCloneActivators) if(!gamePieceCloneActivator.isActive)
            {
                gamePieceCloneActivator.ReduceMovesUntilActive();
            }
    }
    public void AddCurrentPositionToTargetPositions()
    {
        CloneTargetPositions.Add(GamePieceCloneController.MapPosition);
    }
}