using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VoluemManager : MonoBehaviour
{
    public Scrollbar main;

    public Scrollbar bgm;

    public Scrollbar se;

    public SoundManager soundManager;

    

    void Awake()
    {
        soundManager = FindAnyObjectByType<SoundManager>();

        main.value = soundManager.mainVolume;

        bgm.value = soundManager.bgmVolume;

        se.value = soundManager.seVolume;
    }

    // Update is called once per frame
    void Update()
    {
        soundManager.mainVolume = main.value;

        soundManager.bgmVolume = bgm.value;

        soundManager.seVolume = se.value;
    }

    
    
}
