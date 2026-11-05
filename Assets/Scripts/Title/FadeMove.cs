using System;
using UnityEngine;

public class FadeMove : MonoBehaviour
{
    public float startMoveSpeed;

    public float stopPosition;

    public float startPosition;

    public bool fadeInOut;

    public bool stop = false;

    RectTransform rectTransform;

    float moveSpeed;

    bool leftAndRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnStart()
    {
        moveSpeed = startMoveSpeed;

        gameObject.SetActive(true);

        stop = false;

        rectTransform = GetComponent<RectTransform>();

        Vector2 anchored = rectTransform.anchoredPosition;

        if (fadeInOut)
        {
            // IN
            anchored.x = startPosition;
        }
        else
        {
            // OUT
            anchored.x = stopPosition;
        }

        Debug.Log("Hallo");

        rectTransform.anchoredPosition = anchored;

    }
    private void Start()
    {
        if(moveSpeed < 0)
        {
            leftAndRight = true;
        }
        else
        {
            leftAndRight = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stop)
        {
            return;
        }

        if(leftAndRight)
        {
            moveSpeed -= 5.0f;
        }
        else
        {
            moveSpeed += 5.0f;
        }

        Vector2 anchored = rectTransform.anchoredPosition;

        if (fadeInOut)
        {
            anchored.x += moveSpeed * Time.deltaTime;

            if(Mathf.Abs(anchored.x) <= Mathf.Abs(stopPosition))
            {
                stop = true;
                anchored.x = stopPosition;
            }
        }
        else
        {
            anchored.x -= moveSpeed * Time.deltaTime;

            if (Mathf.Abs(anchored.x) >= Mathf.Abs(startPosition))
            {
                stop = true;
                anchored.x = startPosition;
            }
        }

        rectTransform.anchoredPosition = anchored;
    }
}
