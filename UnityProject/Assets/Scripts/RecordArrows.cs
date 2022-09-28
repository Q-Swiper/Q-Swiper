using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordArrows : MonoBehaviour
{
    LevelUi level;
    public void Awake()
    {
        level = GetComponent<LevelUi>();
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            level.ReactOnUserMovesRight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            level.ReactOnUserMovesLeft();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            level.ReactOnUserMovesUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            level.ReactOnUserMovesDown();
        }
    }
}
