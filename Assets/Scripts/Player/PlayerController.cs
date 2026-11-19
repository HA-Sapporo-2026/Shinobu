using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction moveAction;

    [SerializeField] InputAction hideAction;

    [SerializeField] InputAction stepAction;

    [SerializeField] float moveSpeed = 5.0f;

    [SerializeField] float stepDistance = 7.0f;

    [SerializeField] float coolTime = 2f;

    [SerializeField] bool invincibilityFlag = true;

    [SerializeField] float stepTime = 0.5f;

    [SerializeField] Rigidbody rigidbody3d;

    [SerializeField] Transform playerObject;

    [SerializeField] PlayerAlterEgoManager playerAlterEgoManager;
    float alterEgoTime;

    [SerializeField] float wallCollisionToleranceRange = 0.5f; // 壁判定の範囲

    float coolTimer = 0f;

    Vector3 stepPosition;

    bool stepFlag = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        moveAction.Enable();
        hideAction.Enable();
        stepAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        hideAction.Disable();
        stepAction.Disable();
    }

    private void Start()
    {
        alterEgoTime = stepTime / playerAlterEgoManager.MaxPlayerAlterEgoNum;
    }

    // Update is called once per frame
    void Update()
    {
        if(hideAction.IsPressed())
        {
            playerObject.gameObject.SetActive(false);

            return;
        }

        playerObject.gameObject.SetActive(true);

        Step();

        if(!stepFlag)
        {
            Move();
        }
    }

    void Move()
    {
        Vector3 moveDistance = new Vector3(moveAction.ReadValue<Vector2>().x, 
            0.0f,
            moveAction.ReadValue<Vector2>().y);

        // 入力が0なら処理を終わる
        if (moveDistance == Vector3.zero)
        {
            return;
        }

        playerObject.rotation = Quaternion.LookRotation(moveDistance);

        rigidbody3d.MovePosition(
        rigidbody3d.position + moveDistance * moveSpeed * Time.deltaTime
        );
    }

    void Step()
    {
        if (coolTimer < coolTime)
        {
            coolTimer += Time.deltaTime;
        }
        else
        {
            if (stepAction.WasPressedThisFrame())
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();

                Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

                Vector2 direction = (mousePosition - screenCenter).normalized;

                Vector3 stepDirection = new Vector3(direction.x, 0f, direction.y);


                Vector3 position = transform.position;

                position.y += 0.5f;

                RaycastHit[] hits = Physics.RaycastAll(position, stepDirection, stepDistance);
                float nearestHitDistance = float.PositiveInfinity;

                foreach (RaycastHit hit in hits)
                {
                    Collider hitCollider = hit.collider;
                    // 自分自身や子オブジェクトのColliderは検知対象から除外する
                    if (hitCollider.transform == transform || hitCollider.transform.IsChildOf(transform))
                    {
                        continue;
                    }

                    // Rayの終点を最も近いColliderのヒット位置にする
                    if (hit.distance < nearestHitDistance)
                    {
                        nearestHitDistance = hit.distance;
                    }
                }
                
                if(nearestHitDistance != float.PositiveInfinity)
                {
                    if (nearestHitDistance <= wallCollisionToleranceRange)
                    {
                        stepPosition = rigidbody3d.position;
                    }
                    else
                    {
                        stepPosition = rigidbody3d.position + stepDirection *
                        (stepDistance - (stepDistance - nearestHitDistance));
                    }
                }
                else
                {
                    stepPosition = rigidbody3d.position + stepDirection * stepDistance;
                }

                stepFlag = true;
                coolTimer = 0f;

                if (stepDirection.sqrMagnitude > 0f)
                {
                    playerObject.rotation = Quaternion.LookRotation(stepDirection);
                }

                StartCoroutine(MoveRoutine(stepPosition, stepDirection));
                return;
            }
        }
    }

    private IEnumerator MoveRoutine(Vector3 targetPosition,Vector3 stepDirection)
    {
        Vector3 startPosition = rigidbody3d.position;

        if (stepTime <= 0f　|| transform.position == targetPosition)
        {
            rigidbody3d.position = targetPosition;
            stepFlag = false;
            yield break;
        }

        float alterEgoTimer = 0.0f;

        float elapsed = 0f;
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        while (elapsed < stepTime)
        {
            float t = Mathf.Clamp01(elapsed / stepTime);
            rigidbody3d.MovePosition(Vector3.Lerp(startPosition, targetPosition, t));
            yield return waitForFixedUpdate;

            if(alterEgoTime < alterEgoTimer)
            {
                alterEgoTimer = 0.0f;

                playerAlterEgoManager.AlterEgoGenerate(rigidbody3d.position,stepDirection);
            }

            elapsed += Time.fixedDeltaTime;
            alterEgoTimer += Time.fixedDeltaTime;
        }

        rigidbody3d.MovePosition(targetPosition);

        stepFlag = false;
    }
}
