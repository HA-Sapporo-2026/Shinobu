using UnityEngine;
using System.Collections;

public class PlayerAlterEgo: MonoBehaviour
{
    [SerializeField] Renderer[] rend;

    [SerializeField] float duration = 2.0f;

    Material[] mat;
    Color[] originalColors;

    private void Awake()
    {
        mat = new Material[rend.Length];
        originalColors = new Color[rend.Length];

        for (int i = 0; i < rend.Length; i++)
        {
            mat[i] = rend[i].material;
            originalColors[i] = mat[i].color;
        }
    }

    // 生成されたら色を元に戻して、フェードアウトする
    private void OnEnable()
    {
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i].color = originalColors[i];
        }

        StartCoroutine(Fade());
    }


    public IEnumerator Fade()
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;

            for (int i = 0; i < mat.Length; i++)
            {
                Color color = originalColors[i];
                color.a = Mathf.Lerp(originalColors[i].a, 0f, time / duration);
                mat[i].color = color;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
