using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public enum ChooseType
{
    Start,Setting, End
}

public enum SettingChooseType
{
    Se, Bgm, Main, SettingEnd,
}



public class PlayerInput : MonoBehaviour
{
    public InputAction playerChoose;

    public InputAction enter;

    public RawImage start;

    public RawImage end;

    public RawImage setting;

    public ChooseType chooseType;

    public GameObject mainText;

    public GameObject bgmText;

    public GameObject seText;

    public GameObject settingEndBackground;

    public SettingChooseType settingChooseType;

    public Display settingDisplay;

    public GameObject mainSlider;

    TitleManager titleManager;

    FadeManader fadeManader;

    public bool settingFlag;

    private void OnEnable()
    {
        playerChoose.Enable();

        
        playerChoose.performed += OnSettingPerformed;

      
        playerChoose.performed += OnTitlePerformed;

        
        
        enter.Enable();
    }

    private void OnDisable()
    {
        if (playerChoose != null)
        {
            playerChoose.performed -= OnSettingPerformed;
            playerChoose.performed -= OnTitlePerformed;
            playerChoose.Disable();
        }

        if (enter != null)
        {
            enter.Disable();
        }
    }

    private void Start()
    {
        chooseType = ChooseType.Start;

        titleManager = FindAnyObjectByType<TitleManager>();

        fadeManader = FindAnyObjectByType<FadeManader>();
    }

    private void Update()
    {
        if (settingFlag)
        {
            Setting();
            return;
        }
        else
        {
            Title();
        }
    }

    void Setting()
    {
        switch (settingChooseType)
        {
            case SettingChooseType.Main:
                mainText.gameObject.SetActive(true);
                bgmText.gameObject.SetActive(false);
                seText.gameObject.SetActive(false);
                settingEndBackground.SetActive(false);
                break;
            case SettingChooseType.Bgm:
                mainText.gameObject.SetActive(false);
                bgmText.gameObject.SetActive(true);
                seText.gameObject.SetActive(false);
                settingEndBackground.SetActive(false);
                break;
            case SettingChooseType.Se:
                mainText.gameObject.SetActive(false);
                bgmText.gameObject.SetActive(false);
                seText.gameObject.SetActive(true);
                settingEndBackground.SetActive(false);
                break;
            case SettingChooseType.SettingEnd:
                mainText.gameObject.SetActive(false);
                bgmText.gameObject.SetActive(false);
                seText.gameObject.SetActive(false);
                settingEndBackground.SetActive(true);
                break;
        }

        if (enter.WasPressedThisFrame())
        {
            switch (settingChooseType)
            {
                case SettingChooseType.Main:
                    //titleManager.SceneTransition(titleManager.StartSceneName);
                    break;
                case SettingChooseType.Bgm:
                    //titleManager.SceneTransition(null);
                    break;
                case SettingChooseType.Se:
                    break;
                case SettingChooseType.SettingEnd:
                    settingDisplay.DisplayDo(false);
                    settingFlag = false;
                    break;
            }
        }
        
    }

    void Title()
    {
        float alpha = start.color.a; 

        switch (chooseType)
        {
            case ChooseType.Start:
                start.color   = Color.HSVToRGB(0.0f, 0.0f, 1.0f);
                end.color     = Color.HSVToRGB(0.0f, 0.0f, 0.5f);
                setting.color = Color.HSVToRGB(0.0f, 0.0f, 0.5f);
                break;
            case ChooseType.End:
                start.color   = Color.HSVToRGB(0.0f, 0.0f, 0.5f);
                end.color     = Color.HSVToRGB(0.0f, 0.0f, 1.0f);
                setting.color = Color.HSVToRGB(0.0f, 0.0f, 0.5f);
                break;
            case ChooseType.Setting:
                start.color   = Color.HSVToRGB(0.0f, 0.0f, 0.5f);
                end.color     = Color.HSVToRGB(0.0f, 0.0f, 0.5f);
                setting.color = Color.HSVToRGB(0.0f, 0.0f, 1.0f);
                break;
        }

        if (enter.WasPressedThisFrame())
        {
            switch (chooseType)
            {
                case ChooseType.Start:
                    fadeManader.OnFadeIn(titleManager.StartSceneName);
                    break;
                case ChooseType.End:
                    fadeManader.OnFadeIn(null);
                    break;
                case ChooseType.Setting:
                    settingDisplay.DisplayDo(true);
                    settingChooseType = SettingChooseType.Main;
                    EventSystem.current.SetSelectedGameObject(mainSlider);
                    settingFlag = true;
                    break;
            }

        }
    }

    private void OnTitlePerformed(InputAction.CallbackContext ctx)
    {
        if(settingFlag)
        {
            return;
        }
        const float threshold = 0.5f;

        Vector2 v = Vector2.zero;
        try
        {
            v = ctx.ReadValue<Vector2>();
        }
        catch { }

        Vector2 move = playerChoose.ReadValue<Vector2>();

        if (move.y > threshold)
        {
            chooseType = chooseType + 1;
        }
        else if (move.y < -threshold)
        {
            chooseType = chooseType + (System.Enum.GetValues(typeof(ChooseType)).Length - 1);
        }

        chooseType = (ChooseType)((int)chooseType % System.Enum.GetValues(typeof(ChooseType)).Length);
    }

    private void OnSettingPerformed(InputAction.CallbackContext ctx)
    {
        if(!settingFlag)
        {
            return;
        }

        const float threshold = 0.5f;

        Vector2 v = Vector2.zero;
        try
        {
            v = ctx.ReadValue<Vector2>();
        }
        catch { }

        Vector2 move = playerChoose.ReadValue<Vector2>();

        if (move.y > threshold)
        {
            settingChooseType = settingChooseType + 1;
        }
        else if (move.y < -threshold)
        {
            settingChooseType = settingChooseType + (System.Enum.GetValues(typeof(SettingChooseType)).Length - 1);
        }

        settingChooseType = (SettingChooseType)((int)settingChooseType % System.Enum.GetValues(typeof(SettingChooseType)).Length);
    }



   
}
