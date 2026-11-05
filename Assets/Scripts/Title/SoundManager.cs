using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    // 同時に再生できるBGMの最大数
    public int maxBgmSources = 2;

    // BGM用の再生コンポーネント配列
    AudioSource[] bgmAudioSources;

    // 効果音用の再生コンポーネント
    AudioSource seAudioSource;

    int index = 0;

    public float bgmVolume = 0.5f;

    public float seVolume = 0.5f;

    public float mainVolume = 0.5f;



    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // BGM用の再生コンポーネントの初期化
        bgmAudioSources = new AudioSource[maxBgmSources];
        for(int i = 0; i < maxBgmSources; i++)
        {
            // AudioSoucesを追加しその結果を配列に格納 
            bgmAudioSources[i] = gameObject.AddComponent<AudioSource>();

            // ループを有効化
            bgmAudioSources[i].loop = true;
        }

        seAudioSource = gameObject.AddComponent<AudioSource>();
    }


    public void PlayBgm(AudioClip clip)
    {
        for(int i = 0; i < maxBgmSources; i++)
        {
            if (bgmAudioSources[i].isPlaying)
            {
                continue;
            }

            // 引数で受け取ったクリップを割り当てて再生する
            bgmAudioSources[i].clip = clip;
            // マスター音量とBGM個別音量を掛け合わせて最終音量を設定
            bgmAudioSources[i].volume = Mathf.Clamp01(mainVolume * bgmVolume);
            bgmAudioSources[i].Play();

            index = i;
        }
    }

    public void PlaySe(AudioClip clip)
    {
        seAudioSource.volume = Mathf.Clamp01(mainVolume * seVolume);

        seAudioSource.PlayOneShot(clip);
    }

    private void Update()
    {
        for (int i = 0; i < maxBgmSources; i++)
        {
            bgmAudioSources[i].volume = Mathf.Clamp01(mainVolume * bgmVolume);
        }


    }
}
