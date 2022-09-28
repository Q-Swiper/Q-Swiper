using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionMenuUiCode : MonoBehaviour
{
    public GameObject Popup;
    public GameObject CloseButton;
    public GameObject LightBulbLight;
    private void Start()
    {
        ResizePopup();
        CloseButton.GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
        gameObject.SetActive(false);
    }

    private void ResizePopup()
    {
        float width = HelperMethods.GetWidth(Popup);
        float height = HelperMethods.GetHeight(Popup);
        if (width > height * 1.3)
        {
            Popup.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(height * 1.3));
        }
        else
        {
            Popup.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)(width / 1.3));
        }
    }

    public void EnableLightBulb()
    {
        LightBulbLight.SetActive(true);
    }
    public void DisableLightBulb()
    {
        LightBulbLight.SetActive(false);
    }
}
