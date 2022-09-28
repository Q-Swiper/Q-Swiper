using UnityEngine;
public class SaveProgress
{
    //serialized
    public int LastFinishedLevel;
    public int Coins;
    public bool InitialCoinBalanceHasBeenSet;
    public bool ArrowButtonsActive;

    private static string _gameDataFileName = "data.json";

    private static SaveProgress _instance;
    public static SaveProgress Instance
    {
        get
        {
            if (_instance == null)
                Load();
            return _instance;
        }

    }

    public static void Save()
    {
        FileManager.Save(_gameDataFileName, _instance);
    }

    public static void Load()
    {
        _instance = FileManager.Load<SaveProgress>(_gameDataFileName);
        if(_instance == null)
        {
            // default werte, falls das File leer vorhanden war
            _instance = new SaveProgress();
            _instance.Coins = Assets.Scripts.Configurations.startCoins;
            _instance.LastFinishedLevel = 0;
            _instance.ArrowButtonsActive = false;
            Save();
        }
        // replaces the default value 0 the first time when Load()
        if(!_instance.InitialCoinBalanceHasBeenSet)
        {
            _instance.Coins = Assets.Scripts.Configurations.startCoins;
            _instance.InitialCoinBalanceHasBeenSet = true;
            Save();
        }
    }
}