using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void GoToNextLevel()
    {
        if(InGame.DoesLevelExists(SaveProgress.Instance.LastFinishedLevel + 1))
        {
            PersistentManager.Instance.levelToPlay = SaveProgress.Instance.LastFinishedLevel + 1;
            SceneManager.LoadScene(2);
        }
        else
        {
            GoToNoMoreLevelsView();
        }
    }
    public void GoToSelectedLevel(int levelToPlay)
    {
        PersistentManager.Instance.levelToPlay = levelToPlay;
        SceneManager.LoadScene(2);
    }
    public void GoToLevelSelection()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToNoMoreLevelsView()
    {
        SceneManager.LoadScene(3);
    }
    public void GoToSettings()
    {
        SceneManager.LoadScene(4);
    }
    public void GoToShop()
    {
        SceneManager.LoadScene(5);
    }
}