using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    public GameObject GamePrefab;
    public GameObject HeaderLevelText;

    public GameObject ViewsContainer;
    public GameObject LevelCompletedText;
    public GameObject SolutionButtons;
    public GameObject SolutionMenu;

    private ViewController _viewController;
    private GameObject _gameView;
    private SwitchScene _switchScene;
    private Animator _levelCompletedAnimator;
    private bool _headerLevelTextIsWaitigForReload = false;

    public LevelUi Level { get; private set; }
    private void Awake()
    {
        _switchScene = gameObject.GetComponent<SwitchScene>();
        if (PersistentManager.Instance == null)
        {
            _switchScene.GoToMainMenu();
        }
    }
    private void Start()
    {        
        _viewController = ViewsContainer.GetComponent<ViewController>();
        ReloadHeaderLevelText();
        LoadLevelFirstTime(PersistentManager.Instance.levelToPlay);
        _levelCompletedAnimator = LevelCompletedText.GetComponent<Animator>();        
    }

    private void Update()
    {        
        if(Level != null)
        {
            if (Level.IsLevelCompleted() && !Level.AnyGamepieceIsMoving())
            {
                SwitchToNextLevel();
            }
        }
        if(_headerLevelTextIsWaitigForReload && _viewController.ViewsAreNotChanging())
        {
            ReloadHeaderLevelText();
            _headerLevelTextIsWaitigForReload = false;
        }
    }
    public void ReloadHeaderLevelText()
    {
        HeaderLevelText.GetComponent<Text>().text = "Level " + PersistentManager.Instance.levelToPlay;
    }
    public void LoadLevelFirstTime(int levelNumber)
    {
        if (DoesLevelExists(levelNumber))
        {
            _gameView = Instantiate(GamePrefab);
            Level = _gameView.GetComponent<LevelUi>();
            _viewController.AddGameObjectToActiveView(_gameView);
            Level.LoadLevel(Configurations.levelMapsFolder, levelNumber, SolutionButtons.GetComponent<SolutionsButtonsUiCode>());
        }
        else
        {
            _switchScene.GoToNoMoreLevelsView();
        }

    }
    public void LoadLevelAgain(int levelNumber, Direction direction)
    {
        if (DoesLevelExists(levelNumber))
        {
            _gameView = Instantiate(GamePrefab);
            Level = _gameView.GetComponent<LevelUi>();
            _viewController.AddGameObjectToNextView(_gameView, direction);
            Level.LoadLevel(Configurations.levelMapsFolder, levelNumber, SolutionButtons.GetComponent<SolutionsButtonsUiCode>());
            _viewController.SwitchToNextView();
            _headerLevelTextIsWaitigForReload = true;
            SolutionMenu.GetComponent<SolutionMenuUiCode>().DisableLightBulb();
        }
        else
        {
            _switchScene.GoToNoMoreLevelsView();
        }

    }
    public static bool DoesLevelExists(int levelNumber)
    {
        if (levelNumber > Configurations.lastLevel)
        {
            return false; 
        }
        return true;
    }
    public void SwitchToNextLevel()
    {
        _levelCompletedAnimator.SetTrigger("playFadeInFadeOut");

        if (PersistentManager.Instance.levelToPlay == SaveProgress.Instance.LastFinishedLevel + 1)
        {
            SaveProgress.Instance.LastFinishedLevel = PersistentManager.Instance.levelToPlay;
        }
        PersistentManager.Instance.levelToPlay += 1;
        SaveProgress.Save();

        LoadLevelAgain(PersistentManager.Instance.levelToPlay, Level.LastDirection.GetOppositeDirection());
    }

    public void ActivateSolution()
    {
        LoadLevelFirstTime(PersistentManager.Instance.levelToPlay);
        Level.ActivateSolutionMode();
        CloseSolutionMenu();
        ActivateLightBulb();
    }
    public void OpenSolutionMenu()
    {
        Level.GamePaused = true;
        SolutionMenu.SetActive(true);
        SolutionMenu.GetComponent<Animator>().SetTrigger("IsActive");
    }
    public void CloseSolutionMenu()
    {
        Level.GamePaused = false;
        SolutionMenu.SetActive(false);
        SolutionMenu.GetComponent<Animator>().ResetTrigger("IsActive");
    }
    private void ActivateLightBulb() 
    {
        SolutionMenu.GetComponent<SolutionMenuUiCode>().EnableLightBulb();
    }
}
