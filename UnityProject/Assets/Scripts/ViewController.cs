using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    public GameObject ActiveView { get; set; }
    public GameObject NextView { get; set; }
    public Direction directionToMove;
    private bool IsSwitching = false;
    public float swipeSpeed;
    private Vector2 moveDistance;
    private int StartSwitchingIn = -1;
    private readonly Vector2 _minVisiblePosition = new Vector2((float)(1.0 / 3), (float)(1.0 / 3));
    private readonly Vector2 _maxVisiblePosition = new Vector2((float)(2.0 / 3), (float)(2.0 / 3));
    void Update()
    {
        if (IsSwitching)
        {
            ActiveView.GetComponent<RectTransform>().anchorMin += moveDistance;
            ActiveView.GetComponent<RectTransform>().anchorMax += moveDistance;
            NextView.GetComponent<RectTransform>().anchorMin += moveDistance;
            NextView.GetComponent<RectTransform>().anchorMax += moveDistance;
            if (NextViewIsInView())
            {
                NextView.GetComponent<RectTransform>().anchorMin = _minVisiblePosition;
                NextView.GetComponent<RectTransform>().anchorMax = _maxVisiblePosition;
                IsSwitching = false;
                Destroy(ActiveView);
                ActiveView = NextView;
            }
        }
        else if(StartSwitchingIn == 0) 
        {
            IsSwitching = true;
            StartSwitchingIn--;
        }
        if (StartSwitchingIn >= 0)
        {
            StartSwitchingIn--;
        }
    }
    public void AddGameObjectToNextView(GameObject newView, Direction direction)
    {
        Vector2 min = new Vector2(0, 0);
        Vector2 max = new Vector2(1 / 3, 1 / 3);
        if(direction.X == 0)
        {
            if(direction.Y == 1) //bottom
            {
                min = new Vector2((float)1 / 3, 0);
                max = new Vector2((float)2 / 3, (float)1 / 3);
            }
            else if(direction.Y == -1) //top
            {
                min = new Vector2((float)1 / 3, (float)2 / 3);
                max = new Vector2((float)2 / 3, 1);
            }
        }else if(direction.X == 1) //right
        {
            min = new Vector2((float)2 / 3, (float)1 / 3);
            max = new Vector2(1, (float)2 / 3);
        }
        else if (direction.X == -1) //left
        {
            min = new Vector2(0, (float)1 / 3);
            max = new Vector2((float)1 / 3, (float)2 / 3);
        }
        newView.GetComponent<RectTransform>().anchorMin = min;
        newView.GetComponent<RectTransform>().anchorMax = max;
        newView.transform.SetParent(gameObject.transform, false);
        NextView = newView;
        directionToMove = direction;
    }
    public void AddGameObjectToActiveView(GameObject newView)
    {
        if(ActiveView != null)
        {
            Destroy(ActiveView);
        }
        newView.transform.SetParent(gameObject.transform, false);
        ActiveView = newView;
    }
    public void SwitchToNextView()
    {
        moveDistance = new Vector2(
            -directionToMove.X / (800 / Assets.Scripts.Configurations.swipeSpeed),
            directionToMove.Y / (600 / Assets.Scripts.Configurations.swipeSpeed)
        );
        StartSwitchingIn = 60;
    }
    private bool NextViewIsInView()
    {
        var allowedInaccuracy = Math.Abs(moveDistance.x + moveDistance.y);

        var xMaxDifference = 1.0 / 3.0 + allowedInaccuracy;
        var xMinDifference = 1.0 / 3.0 - allowedInaccuracy;
        var yMaxDifference = 1.0 / 3.0 + allowedInaccuracy;
        var yMinDifference = 1.0 / 3.0 - allowedInaccuracy;

        if (NextView.GetComponent<RectTransform>().anchorMin.x < xMaxDifference &&
            NextView.GetComponent<RectTransform>().anchorMin.x > xMinDifference &&
            NextView.GetComponent<RectTransform>().anchorMin.y < yMaxDifference &&
            NextView.GetComponent<RectTransform>().anchorMin.y > yMinDifference)
        {
            return true;
        }
        return false;
    }
    public bool ViewsAreNotChanging()
    {
        return !IsSwitching && StartSwitchingIn == -1;
    }
}
