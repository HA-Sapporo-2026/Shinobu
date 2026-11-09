using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EnemyViewing : MonoBehaviour
{
    [SerializeField, Min(1)] int rayCount = 5;

    [SerializeField, Range(0f, 360f)] float angle = 90f;

    [SerializeField] float distance = 1000f;

    [SerializeField] Color rayColor = Color.red;

    LineRenderer[] lineRenderers;
    Material runtimeMaterial;
    HashSet<Collider> previousHitColliders = new HashSet<Collider>();
    HashSet<Collider> currentHitColliders = new HashSet<Collider>();

    private void Awake()
    {
        int count = Mathf.Max(1, rayCount);
        lineRenderers = new LineRenderer[count];
        LineRenderer firstLineRenderer = GetComponent<LineRenderer>();

        Material material = firstLineRenderer.sharedMaterial;
        if (material == null)
        {
            Shader shader = Shader.Find("Sprites/Default");
            if (shader != null)
            {
                runtimeMaterial = new Material(shader);
                material = runtimeMaterial;
            }
        }

        for (int i = 0; i < count; i++)
        {
            LineRenderer lineRenderer = i == 0
                ? firstLineRenderer
                : new GameObject($"ViewingRay_{i}").AddComponent<LineRenderer>();

            if (i > 0)
            {
                lineRenderer.transform.SetParent(transform, false);
            }

            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.startColor = rayColor;
            lineRenderer.endColor = rayColor;
            if (material != null)
            {
                lineRenderer.sharedMaterial = material;
            }

            lineRenderers[i] = lineRenderer;
        }
    }

    void Update()
    {
        Vector3 origin = transform.position;
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            float rayAngle = lineRenderers.Length == 1
                ? 0f
                : Mathf.Lerp(-angle / 2f, angle / 2f, i / (float)(lineRenderers.Length - 1));
            Vector3 direction = Quaternion.AngleAxis(rayAngle, Vector3.up) * transform.forward;

            RaycastHit[] hits = Physics.RaycastAll(origin, direction, distance);
            float nearestHitDistance = float.PositiveInfinity;
            Vector3 endPosition = origin + direction * distance;

            foreach (RaycastHit hit in hits)
            {
                Collider hitCollider = hit.collider;
                if (hitCollider.transform == transform || hitCollider.transform.IsChildOf(transform))
                {
                    continue;
                }

                currentHitColliders.Add(hitCollider);
                if (!previousHitColliders.Contains(hitCollider))
                {
                    Debug.Log($"Ray hit: {hitCollider.name}", hitCollider);
                }

                if (hit.distance < nearestHitDistance)
                {
                    nearestHitDistance = hit.distance;
                    endPosition = hit.point;
                }
            }

            lineRenderers[i].SetPosition(0, origin);
            lineRenderers[i].SetPosition(1, endPosition);
        }

        HashSet<Collider> previousFrameHits = previousHitColliders;
        previousHitColliders = currentHitColliders;
        currentHitColliders = previousFrameHits;
        currentHitColliders.Clear();
    }

    private void OnDestroy()
    {
        if (runtimeMaterial != null)
        {
            Destroy(runtimeMaterial);
        }
    }
}
