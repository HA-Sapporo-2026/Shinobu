using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //public TextMeshPro textMeshPro;

    public Item itemPrefab;

    Item[] items;

    public Vector3[] position;

    public int itemCount;

    public AudioClip get_se;

    SoundManager soundManager;

    private void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();

        if(itemPrefab == null)
        {
            return;
        }
        // 指定された最大弾分の配列の生成
        items = new Item[position.Length];

        for (int i = 0; i < position.Length; i++)
        {
            // Obuject.Instantiateを使用して弾オブジェクトを動的に生成する
            items[i] = Instantiate(itemPrefab);

            // サウンド関連を割り当て
            if (soundManager != null)
            {
                items[i].soundManager = soundManager;
                items[i].get_se = get_se;
            }

            // 弾オブジェクトを非アクティブにする
            items[i].gameObject.SetActive(false);
        }

        for(int i = 0;i < position.Length; i++)
        {
            ItemGenerate(position[i]);
        }
    }

  
    public Item ItemGenerate(Vector3 pos)
    {
        if (itemPrefab == null)
        {
            return null;
        }

        if (items == null)
        {
            return null;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].gameObject.activeSelf)
            {
                continue;
            }

            items[i].transform.position = pos;
            items[i].gameObject.SetActive(true);
            return items[i];
        }

        return null;
    }



    
}
