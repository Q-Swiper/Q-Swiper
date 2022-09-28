using UnityEngine;

public class SolutionsButtonsUiCode : MonoBehaviour
{
    public GameObject SolutionButtons;
    public GameObject Parent;

    public GameObject ArrowUp;
    public GameObject ArrowDown;
    public GameObject ArrowRight;
    public GameObject ArrowLeft;
     
    public float PercentageSize;

    private bool ArrowButtonsAreActive {
        get {
            return SaveProgress.Instance.ArrowButtonsActive;
        } 
    }

    void Start()
    {
        var size = HelperMethods.GetHeight(Parent)*PercentageSize;
        SolutionButtons.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
        SolutionButtons.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);

        SetArrowSizes();
        ManageVisibility();
    }

    private void SetArrowSizes()
    {
        var arrowSize = HelperMethods.GetWidth(ArrowUp);
        ArrowUp.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, arrowSize);
        ArrowDown.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, arrowSize);
        ArrowRight.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, arrowSize);
        ArrowLeft.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, arrowSize);
    }

    private void ManageVisibility()
    {
        if (ArrowButtonsAreActive)
        {
            ArrowUp.SetActive(true);
            ArrowDown.SetActive(true);
            ArrowRight.SetActive(true);
            ArrowLeft.SetActive(true);
        }
        else
        {
            DeactivateAllArrows();
        }
    }
    private void DeactivateAllArrows()
    {
        ArrowUp.SetActive(false);
        ArrowDown.SetActive(false);
        ArrowRight.SetActive(false);
        ArrowLeft.SetActive(false);
    }
    public void ShowSolutionDirection(Direction direction)
    {
        DeactivateAllSolutionArrows();
        ActivateSolutionArrow(direction);
    }

    private void ActivateSolutionArrow(Direction direction)
    {
        GameObject arrowToActivate;
        switch (direction)
        {
            case Direction.Up:
                arrowToActivate = ArrowUp;
                break;
            case Direction.Down:
                arrowToActivate = ArrowDown;
                break;
            case Direction.Right:
                arrowToActivate = ArrowRight;
                break;
            case Direction.Left:
                arrowToActivate = ArrowLeft;
                break;
            default:
                Debug.LogError("Nicht vorhandene Direction benutzt: " + direction);
                return;
        }
        if (ArrowButtonsAreActive)
        {
            HelperMethods.GetChildByName(HelperMethods.GetChildByName(arrowToActivate, "Image"), "InnerArrow").SetActive(true);
        }
        else
        {
            arrowToActivate.SetActive(true);
        }
    }

    public void DeactivateAllSolutionArrows()
    {
        if (ArrowButtonsAreActive)
        {            
            HelperMethods.GetChildByName(HelperMethods.GetChildByName(ArrowUp, "Image"), "InnerArrow").SetActive(false);
            HelperMethods.GetChildByName(HelperMethods.GetChildByName(ArrowDown, "Image"), "InnerArrow").SetActive(false);
            HelperMethods.GetChildByName(HelperMethods.GetChildByName(ArrowRight, "Image"), "InnerArrow").SetActive(false);
            HelperMethods.GetChildByName(HelperMethods.GetChildByName(ArrowLeft, "Image"), "InnerArrow").SetActive(false);
        }
        else
        {
            DeactivateAllArrows();
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Right,
        Left,
        InvalidDirection
    }
}
