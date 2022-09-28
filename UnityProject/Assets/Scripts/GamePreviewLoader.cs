using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePreviewLoader : MonoBehaviour
{
    public GameObject GamePrefab;
    public GameObject GamePreviewContainer;

    private GameObject _gameContainer;
    private LevelUi _level;
    private float _timeOfNextMovement;
    private int _lastRandomDirection = 0;
    private void Start()
    {
        LoadActivePreviewLevel();
        RecalculateRandomTimeOfNextMovement(2, (float)2.1);
    }
    private void Update()
    {
        if(_timeOfNextMovement < Time.time)
        {
            MoveInRandomDirection();
            RecalculateRandomTimeOfNextMovement((float).6, (float)1.2);
        }
    }
    private void LoadActivePreviewLevel()
    {
        _gameContainer = Instantiate(GamePrefab);
        _level = _gameContainer.GetComponent<LevelUi>();
        var recordArrows = _gameContainer.GetComponent<RecordArrows>();
        var recordSwiping = _gameContainer.GetComponent<RecordSwiping>();
        recordArrows.enabled = false;
        recordSwiping.enabled = false;
        AddGameToGamePreview(_gameContainer);
        _level.LoadLevel(Configurations.previewMapsFolder, GetActivePreviewNumber());
    }
    private void AddGameToGamePreview(GameObject gameContainer)
    {
        gameContainer.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        gameContainer.transform.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

        gameContainer.transform.SetParent(GamePreviewContainer.transform, false);
    }
    private int GetActivePreviewNumber()
    {
        return Math.Abs(SaveProgress.Instance.LastFinishedLevel / 20) + 1;
    }
    private void RecalculateRandomTimeOfNextMovement(float min, float max)
    {
        _timeOfNextMovement = Time.time + UnityEngine.Random.Range(min, max);
    }
    private void MoveInRandomDirection()
    {
        int randomNumber;
        do
        {
            randomNumber = UnityEngine.Random.Range(1, 5);
        } while (randomNumber == _lastRandomDirection);
        _lastRandomDirection = randomNumber;
        //Debug.Log(_lastRandomDirection);
        switch (randomNumber)
        {
            case 1:
                _level.ReactOnUserMovesUp();
                break;
            case 2:
                _level.ReactOnUserMovesDown();
                break;
            case 3:
                _level.ReactOnUserMovesLeft();
                break;
            case 4:
                _level.ReactOnUserMovesRight();
                break;
        }
    }
}
