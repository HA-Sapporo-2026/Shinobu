using UnityEngine;

public class EndCredits : MonoBehaviour
{
    public float moveSpeed = 10.0f;

    public float stopPosition;

    RectTransform endCreditsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endCreditsText = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = endCreditsText.position;

        position.y = Mathf.Min(position.y + moveSpeed * Time.deltaTime,stopPosition);

        endCreditsText.position = position;
    }
}
