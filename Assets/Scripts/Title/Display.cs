using UnityEngine;

public class Display : MonoBehaviour
{
    public void DisplayDo(bool display)
    {
        gameObject.SetActive(display);
    }
}
