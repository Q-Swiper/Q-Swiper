using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordSwiping : MonoBehaviour
{
    LevelUi level;

    Vector2 startPosition;
    float startTime;

    private void Awake()
    {
        level = GetComponent<LevelUi>();
    }
    void Update()
    {
        //Debug.Log(Input.touchCount);
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Vector2 endPosition = Input.GetTouch(0).position;
            Vector2 delta = endPosition - startPosition;

            float dist = Mathf.Sqrt(Mathf.Pow(delta.x, 2) + Mathf.Pow(delta.y, 2));
            float angle = Mathf.Atan(delta.y / delta.x) * (180.0f / Mathf.PI);
            float duration = Time.time - startTime;
            float speed = dist / duration;
            
            if (angle < 0)
            {
                angle *= 1.0f;
            }
            //Debug.Log("Distance: " + dist + " Angle: " + angle + " Speed: " + speed);
            var absAngel = Mathf.Abs(angle);
            if(dist > 60)
            {
                if (absAngel < 45) //Horizontal x
                {
                    if ( startPosition.x - endPosition.x < 0) //right
                    {
                        level.ReactOnUserMovesRight();
                    }
                    else //left
                    {
                        level.ReactOnUserMovesLeft();
                    }
                }else if (absAngel > 45) //Vertikal y
                {
                    if (startPosition.y - endPosition.y < 0) //up
                    {
                        level.ReactOnUserMovesUp();
                    }
                    else //down
                    {
                        level.ReactOnUserMovesDown();
                    }
                }
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startPosition = Input.GetTouch(0).position;
            startTime = Time.time;
        }
    }
}
