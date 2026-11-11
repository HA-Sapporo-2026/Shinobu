using System.Collections.Generic;
using UnityEngine;

public class EnemyViewing : MonoBehaviour
{
    // 扇状に飛ばすRayの本数。終点を使ったメッシュ生成のため2本以上にする
    [SerializeField, Min(2)] int rayCount = 30;

    // 敵の正面を中心としたRayの広がり角度
    [SerializeField, Range(0f, 360f)] float angle = 90f;

    // Rayの最大距離
    [SerializeField] float distance = 5f;

    // 扇形メッシュの色
    [SerializeField] Color color = Color.red;

    public Color Color => color;

    // 外部スクリプトから参照する設定値
    public float Angle => angle;
    public float Distance => distance;
    public int RayCount => rayCount;

    // 各レイの終点（ワールド座標）を外部（FanMeshGenerator）から参照できるように公開
    public Vector3[] RayEndPoints { get; private set; }

    // 前フレームと現在フレームのヒット対象を比較し、同じColliderへのログ連続出力を防ぐ
    HashSet<Collider> previousHitColliders = new HashSet<Collider>();
    HashSet<Collider> currentHitColliders = new HashSet<Collider>();

    private void Awake()
    {
        int count = Mathf.Max(2, rayCount);
        RayEndPoints = new Vector3[count]; // 終点配列の初期化
    }

    void Update()
    {
        Vector3 origin = transform.position;
        for (int i = 0; i < RayEndPoints.Length; i++)
        {
            // 正面を中心に、扇の左端から右端まで均等にRayを配置する
            float rayAngle = Mathf.Lerp(-angle / 2f, angle / 2f, i / (float)(RayEndPoints.Length - 1));
            Vector3 direction = Quaternion.AngleAxis(rayAngle, Vector3.up) * transform.forward;

            // 指定距離内でRayに当たったColliderをすべて取得する
            RaycastHit[] hits = Physics.RaycastAll(origin, direction, distance);
            float nearestHitDistance = float.PositiveInfinity;
            Vector3 endPosition = origin + direction * distance;

            foreach (RaycastHit hit in hits)
            {
                Collider hitCollider = hit.collider;
                // 自分自身や子オブジェクトのColliderは検知対象から除外する
                if (hitCollider.transform == transform || hitCollider.transform.IsChildOf(transform))
                {
                    continue;
                }

                currentHitColliders.Add(hitCollider);
                if (!previousHitColliders.Contains(hitCollider))
                {
                    // 新しく検知したColliderだけログに出力する
                    Debug.Log($"Ray hit: {hitCollider.name}", hitCollider);
                }

                if(hitCollider.name == "Player")
                {
                    continue;
                }

                // Rayの終点を最も近いColliderのヒット位置にする
                if (hit.distance < nearestHitDistance)
                {
                    nearestHitDistance = hit.distance;
                    endPosition = hit.point;
                }
            }

            // 計算した終点を配列に保存
            RayEndPoints[i] = endPosition;
        }

        // 次フレームとの比較に備えて、ヒット集合を入れ替える
        HashSet<Collider> previousFrameHits = previousHitColliders;
        previousHitColliders = currentHitColliders;
        currentHitColliders = previousFrameHits;
        currentHitColliders.Clear();
    }

}