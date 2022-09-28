using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject ShopButton;
    public GameObject SettingsButton;
    public GameObject CoinDisplay;

    private float HalfCameraHeight { get; set; }
    private float HalfCameraWidth { get; set; }
    private GamePreviewLoader _gamePreviewLoader;

    void Start()
    {
        CalculateResponsiveAttributes();
        LoadCoinDisplay();
        _gamePreviewLoader = GetComponent<GamePreviewLoader>();
    }
    private void CalculateResponsiveAttributes()
    {
        Camera mainCamera = Camera.main;
        HalfCameraHeight = mainCamera.orthographicSize;
        HalfCameraWidth = HalfCameraHeight * mainCamera.aspect;

        CalculateResponsiveShopButton();
        CalculateResponsiveSettingsButton();
    }

    public void ResetProgress()
    {
        SaveProgress.Instance.LastFinishedLevel = 0;
        SaveProgress.Save();
    }
    public void CompleteAllLevels()
    {
        SaveProgress.Instance.LastFinishedLevel = Configurations.lastLevel;
        SaveProgress.Save();
    }
    private void LoadCoinDisplay()
    {
        string amountOfCoins = SaveProgress.Instance.Coins.ToString();
        string spaceBeforeText = "";
        if(amountOfCoins.Length == 1)
        {
            spaceBeforeText += "  ";
        }        
        CoinDisplay.GetComponentInChildren<Text>().text = spaceBeforeText + amountOfCoins;
    }
    private void CalculateResponsiveShopButton()
    {
        HelperMethods.SetWidthRelativeToHeight(ShopButton, 1);
        float width = HelperMethods.GetWidth(ShopButton);
        float height = HelperMethods.GetHeight(ShopButton);
        float positionY = ShopButton.transform.position.y;
        float distanceTop = HalfCameraHeight - positionY - height / 2;

        var newShopButtonPosition = new Vector3(+HalfCameraWidth - 2 * distanceTop - width * (float)1.5, positionY, 1); // links neben SettingsButton
        ShopButton.transform.GetComponent<RectTransform>().SetPositionAndRotation(newShopButtonPosition, new Quaternion());

    }
    private void CalculateResponsiveSettingsButton()
    {
        HelperMethods.SetWidthRelativeToHeight(SettingsButton, 1);
        float width = HelperMethods.GetWidth(SettingsButton);
        float height = HelperMethods.GetHeight(SettingsButton);
        float positionY = SettingsButton.transform.position.y;
        float distanceTop = HalfCameraHeight - positionY - height / 2;
        
        var newSettingsButtonPosition = new Vector3(+HalfCameraWidth - distanceTop - width / 2, positionY, 1); // an den rechten Rand
        SettingsButton.transform.GetComponent<RectTransform>().SetPositionAndRotation(newSettingsButtonPosition, new Quaternion());
    }
}
