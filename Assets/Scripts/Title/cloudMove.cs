using UnityEngine;

public class cloudMove : MonoBehaviour
{
    RectTransform rectTransform;

    public float moveSpeed = 10.0f;

    public float startPosition;

    public float endPosition;

    // 開始時のアンカード位置を保持
    private float startAnchoredX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        startAnchoredX = rectTransform.anchoredPosition.x;

        
    }

    // Update is called once per frame
    void Update()
    {
       Vector2 anchored = rectTransform.anchoredPosition;

       anchored.x -= moveSpeed * Time.deltaTime;

       if (anchored.x >= endPosition)
       {
           anchored.x = startPosition;
       }

       rectTransform.anchoredPosition = anchored;
    }
}
