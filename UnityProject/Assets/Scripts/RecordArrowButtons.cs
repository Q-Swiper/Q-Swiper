using UnityEngine;

public class RecordArrowButtons : MonoBehaviour
{
    LevelUi level;

    public void ArrowOnClickRight()
    {
        if (LevelHasValue())
        {
            level.ReactOnUserMovesRight();
        }
    }

    public void ArrowOnClickLeft()
    {
        if (LevelHasValue())
        {
            level.ReactOnUserMovesLeft();
        }
    }

    public void ArrowOnClickUp()
    {
        if (LevelHasValue())
        {
            level.ReactOnUserMovesUp();
        }
    }

    public void ArrowOnClickDown()
    {
        if (LevelHasValue())
        {
            level.ReactOnUserMovesDown();
        }
    }

    private bool LevelHasValue()
    {
        if (level == null)
        {
            level = GetComponent<InGame>().Level;
            if(level == null)
            {
                return false;
            }
        }
        return true;
    }
}