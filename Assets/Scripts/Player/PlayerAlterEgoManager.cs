using UnityEngine;

public class PlayerAlterEgoManager : MonoBehaviour
{
    public PlayerAlterEgo playerAlterEgoPrefab;

    public int maxPlayerAlterEgoNum;

    PlayerAlterEgo[] playerAlterEgos;

    private void Start()
    {
        if (playerAlterEgoPrefab == null)
        {
            Debug.Log("分身用のPrefabがない");
            return;
        }
        // 指定された最大弾分の配列の生成
        playerAlterEgos = new PlayerAlterEgo[maxPlayerAlterEgoNum];

        for (int i = 0; i < maxPlayerAlterEgoNum; i++)
        {
            // Obuject.Instantiateを使用して分身オブジェクトを動的に生成する
            playerAlterEgos[i] = Instantiate(playerAlterEgoPrefab);

            // 分身オブジェクトを非アクティブにする
            playerAlterEgos[i].gameObject.SetActive(false);
        }
    }


    public PlayerAlterEgo AlterEgoGenerate(Vector3 position)
    {
        for (int i = 0; i < maxPlayerAlterEgoNum; i++)
        {
            if (playerAlterEgos[i].gameObject.activeSelf)
            {
                continue;
            }

            playerAlterEgos[i].transform.position = position;
            playerAlterEgos[i].gameObject.SetActive(true);


            transform.rotation = Quaternion.LookRotation(position.normalized);
            return playerAlterEgos[i];
        }

        return null;
    }
}
