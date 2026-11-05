using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class VolumeSlider: MonoBehaviour
{
    Camera mainCamera;

    bool click = false;
    // 正規化されたボリューム値 (0.0f ~ 1.0f)
    public float Volume { get; private set; } = 0f;
    // 外部から操作中か確認するためのプロパティ
    public bool IsDragging => click;

    // 外部からスライダー位置・正規化値を設定する
    public void SetNormalizedVolume(float t)
    {
        t = Mathf.Clamp01(t);
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(-2.3f, 3.3f, t);
        transform.position = pos;
        Volume = t;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Renewal(float pos)
    {
        Volume = Mathf.InverseLerp(-2.3f, 3.3f, pos);
    }
}
