using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    ItemManager itemManager;

    public AudioClip get_se;

    public SoundManager soundManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        itemManager = FindAnyObjectByType<ItemManager>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        itemManager.itemCount++;

        if(soundManager != null ) 
        {
            soundManager.PlaySe(get_se);
        }

        gameObject.SetActive(false);
    }
}
