using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicLevels : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject levelButtonsContainer;

    private void Start()
    {
        var levelButtonsContainerWidth = levelButtonsContainer.GetComponent<RectTransform>().rect.width;
        SetDynamicSizeOfGridContent(levelButtonsContainer, (int)Math.Round(levelButtonsContainerWidth/300));
        var levels = Resources.LoadAll<TextAsset>(Configurations.levelMapsFolder);
        var switchScene = gameObject.GetComponent<SwitchScene>();
        foreach (var level in levels)
        {
            var levelButton = Instantiate(levelButtonPrefab) as GameObject;
            levelButton.GetComponentInChildren<Text>().text = ConvertLevelFileNameToLevelName(level.name);
            if(int.Parse(level.name) > SaveProgress.Instance.LastFinishedLevel + 1)
            {
                levelButton.GetComponent<Image>().color = new Color32(0, 0, 0, 40);
            }
            else
            {
                levelButton.GetComponent<Button>().onClick.AddListener(delegate { switchScene.GoToSelectedLevel(int.Parse(level.name)); });
            }
            levelButton.transform.SetParent(levelButtonsContainer.transform, false);
        }
    }
    public void SetDynamicSizeOfGridContent(GameObject gameObject, int numberOfRows)
    {
        var containerWidth = gameObject.GetComponent<RectTransform>().rect.width;
        var containerContentPadding = gameObject.GetComponent<GridLayoutGroup>().padding.left + gameObject.GetComponent<GridLayoutGroup>().padding.right;
        var newContentWidth = containerWidth / numberOfRows - containerContentPadding;
        var newContentHeight = newContentWidth / 4 * 3;
        gameObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(newContentWidth, newContentHeight);
        gameObject.GetComponent<GridLayoutGroup>().constraintCount = numberOfRows;
    }
    private string ConvertLevelFileNameToLevelName(string fileName)
    {
        fileName = fileName.TrimStart('0');
        var newFileName = "Level " + fileName;
        return newFileName;
    }
}
