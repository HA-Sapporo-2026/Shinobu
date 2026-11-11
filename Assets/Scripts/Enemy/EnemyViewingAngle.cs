using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FanMeshGenerator : MonoBehaviour
{
    [SerializeField] EnemyViewing enemyViewing;

    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;
    private Material runtimeMaterial;

    private void Start()
    {
        if (enemyViewing == null)
        {
            Debug.Log("enemyViewingがありません");
            return;
        }

        // 共有マテリアルを変更しないよう複製し、色と透明描画を設定する
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Material sharedMaterial = meshRenderer.sharedMaterial;
        if (sharedMaterial != null)
        {
            runtimeMaterial = new Material(sharedMaterial);
            meshRenderer.sharedMaterial = runtimeMaterial;

            runtimeMaterial.SetColor("_BaseColor", enemyViewing.Color);
            runtimeMaterial.SetTexture("_BaseMap", Texture2D.whiteTexture);

            ConfigureTransparentMaterial(runtimeMaterial);
        }

        mesh = new Mesh(); 

        GetComponent<MeshFilter>().mesh = mesh;
    }

    // EnemyViewingのUpdate()でレイが更新された「後」にメッシュを構築するためLateUpdateを使用
    private void LateUpdate()
    {
        if (enemyViewing == null || enemyViewing.RayEndPoints == null)
        {
            return;
        }

        Vector3[] endPoints = enemyViewing.RayEndPoints;
        int pointCount = endPoints.Length;
        if (pointCount < 2)
        {
            return;
        }

        int segments = pointCount - 1;

        // 配列サイズが未初期化または変更された場合のみ確保（GC発生防止）
        if (vertices == null || vertices.Length != pointCount + 1)
        {
            vertices = new Vector3[pointCount + 1];
            triangles = new int[segments * 3];
            colors = new Color[pointCount + 1];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.white;
            }

            for (int i = 0; i < segments; i++)
            {
                int triIndex = i * 3;
                triangles[triIndex] = 0;         // 中心点
                triangles[triIndex + 1] = i + 1; // 現在のレイの終点
                triangles[triIndex + 2] = i + 2; // 次のレイの終点
            }
        }

        // 0番目は中心点（EnemyViewingの原点と一致させる）
        vertices[0] = transform.InverseTransformPoint(enemyViewing.transform.position);

        // 1. EnemyViewingが計算した各レイの終点（ワールド座標）をローカル座標に変換して頂点にする
        for (int i = 0; i < pointCount; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(endPoints[i]);
        }

        // 2. メッシュにデータを適用
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private static void ConfigureTransparentMaterial(Material material)
    {
        bool supportsSurfaceMode = material.HasProperty("_Surface");
        bool supportsStandardMode = material.HasProperty("_Mode");

        if (supportsSurfaceMode)
        {
            material.SetFloat("_Surface", 1f);
            material.SetFloat("_Blend", 0f);
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.DisableKeyword("_ALPHAMODULATE_ON");
        }
        else if (supportsStandardMode)
        {
            material.SetFloat("_Mode", 3f);
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        if (supportsSurfaceMode || supportsStandardMode)
        {
            material.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            material.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
            if (material.HasProperty("_SrcBlendAlpha"))
            {
                material.SetFloat("_SrcBlendAlpha", (float)BlendMode.One);
            }
            if (material.HasProperty("_DstBlendAlpha"))
            {
                material.SetFloat("_DstBlendAlpha", (float)BlendMode.OneMinusSrcAlpha);
            }
            material.SetFloat("_ZWrite", 0f);
            material.SetOverrideTag("RenderType", "Transparent");
            material.renderQueue = (int)RenderQueue.Transparent;
        }
    }

    private void OnDestroy()
    {
        if (runtimeMaterial != null)
        {
            Destroy(runtimeMaterial);
        }
    }
}
