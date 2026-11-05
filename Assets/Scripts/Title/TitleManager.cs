using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputSettings;

public class TitleManager : MonoBehaviour
{
    public string StartSceneName;


    public float startFadeTime;

    public float endFadeTime;

    PlayerInput playerInput;


    public AudioClip bgmClip;

    SoundManager soundManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        soundManager = FindAnyObjectByType<SoundManager>();

        soundManager.PlayBgm(bgmClip);
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }


}
