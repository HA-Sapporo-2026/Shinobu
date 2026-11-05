using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManader : MonoBehaviour
{

    public FadeMove[] fadeBox;

    public bool fade;

    string nextSceneName;

    public float screenFadeDuration = 1.0f;

    public RawImage[] leftPicture;

    public RawImage[] rightPicture;

    bool fadeStop;
    bool isFading;

    bool isLoading;

    SoundManager soundManager;

    public AudioClip openClose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 保持対象は GameObject にする
        DontDestroyOnLoad(gameObject);

        fadeStop = false;
        isFading = false;
        isLoading = false;

        Debug.Log("FadeManader生成: " + gameObject.GetInstanceID());

        soundManager = FindAnyObjectByType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {



        if (!isFading) return;

        if (fadeBox == null || fadeBox.Length == 0) return;

        // 全てのアクティブな FadeMove が stop=true になったら次のシーンへ
        bool allStopped = true;
        for (int i = 0; i < fadeBox.Length; i++)
        {
            var fb = fadeBox[i];
            if (fb == null) continue;
            // 非表示のものは判定に含めない
            if (!fb.isActiveAndEnabled) continue;
            if (!fb.stop)
            {
                allStopped = false;
                break;
            }
        }

        if (allStopped && !fadeStop && !isLoading)
        {
            fadeStop = true; // 再実行防止ガード
            isFading = false;

            isLoading = true;
            

            Debug.Log("シーン読み込み開始: " + nextSceneName);


            if (string.IsNullOrEmpty(nextSceneName))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }
            else
            {
                
              StartCoroutine(LoadSceneAsyncAndCleanup(nextSceneName));
                
            }
        }
    }

    private IEnumerator LoadSceneAsyncAndCleanup(string sceneName)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone)
        {
            yield return null;
        }

        // シーン切り替え完了
        isLoading = false;
        fadeStop = false;



        yield return Resources.UnloadUnusedAssets();
        System.GC.Collect();

        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }
        
        OnFadeOut();

    }
    public void OnFadeIn(string SceneName)
    {
        if (isLoading)
            return;

        int a = Random.Range(0,leftPicture.Length - 1);

        for(int i = 0; i < leftPicture.Length; i++)
        {
            leftPicture[i].gameObject.SetActive(false);
            rightPicture[i].gameObject.SetActive(false);
        }

        leftPicture [a].gameObject.SetActive(true);
        rightPicture[a].gameObject.SetActive(true);


        // フェード開始フラグを立てる
        isFading = true;
        fadeStop = false;

        nextSceneName = SceneName;

        for (int i  = 0; i < fadeBox.Length; i++)
        {
            // 先にモードを設定してからアクティブ化する
            fadeBox[i].fadeInOut = true;
            fadeBox[i].OnStart();

            soundManager.PlaySe(openClose);
        }
    }

    public void OnFadeOut()
    {
        // シーンロードはしない
        isFading = false;
        fadeStop = true;

        for (int i = 0; i < fadeBox.Length; i++)
        {
            if (fadeBox[i] == null)
                continue;

            fadeBox[i].fadeInOut = false;
            fadeBox[i].OnStart();

            soundManager.PlaySe(openClose);
        }
    }


}
