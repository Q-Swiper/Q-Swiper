using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletedMessageUIConfigurations : MonoBehaviour
{
    public GameObject LevelCommpletedMessage;

    void Start()
    {
        HelperMethods.SetHeightRelativeToWidth(LevelCommpletedMessage, (float)0.25);
    }
}