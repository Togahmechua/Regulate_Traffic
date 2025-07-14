using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private PoliceCar player;
    [SerializeField] private Transform[] movePos;
    [SerializeField] private Transform curPos;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    [SerializeField] private int curIndex = 0;
    [SerializeField] private float minSwipeDistance = 50f;
    private bool isSwiping = false;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                isSwiping = true;
            }

            if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                isSwiping = false;
                endTouchPosition = touch.position;

                Vector2 inputVector = endTouchPosition - startTouchPosition;

                if (inputVector.magnitude < minSwipeDistance) return;

                if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
                {
                    if (inputVector.x > 0)
                        RightSwipe();
                    else
                        LeftSwipe();
                }
                else
                {
                    if (inputVector.y > 0)
                        UpSwipe();
                    else
                        DownSwipe();
                }
            }
        }
    }

    private void DownSwipe()
    {
        //print("down");
    }

    private void UpSwipe()
    {
        //print("up");
    }

    private void LeftSwipe()
    {
        //print("left");

        if (curIndex > 0)
        {
            curIndex--;
            player.transform.position = movePos[curIndex].position;
            player.TurnLeft();
            curPos = movePos[curIndex];
        }
    }
    private void RightSwipe()
    {
        //print("right");
       
        if (curIndex < movePos.Length - 1)
        {
            curIndex++;
            player.transform.position = movePos[curIndex].position;
            player.TurnRight();
            curPos = movePos[curIndex];
        }
    }
}
