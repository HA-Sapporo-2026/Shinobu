using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public float coolTimer = 0f;

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
        alterEgoTime = stepTime / playerAlterEgoManager.maxPlayerAlterEgoNum;
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
                stepPosition = rigidbody3d.position + stepDirection * stepDistance;

                stepFlag = true;
                coolTimer = 0f;

                if (stepDirection.sqrMagnitude > 0f)
                {
                    playerObject.rotation = Quaternion.LookRotation(stepDirection);
                }

                StartCoroutine(MoveRoutine(stepPosition));
                return;
            }
        }
    }

    private IEnumerator MoveRoutine(Vector3 targetPosition)
    {
        Vector3 startPosition = rigidbody3d.position;

        if(invincibilityFlag)
        {
           // gameObject.tag = "player";
        }

        if (stepTime <= 0f)
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

                playerAlterEgoManager.AlterEgoGenerate(rigidbody3d.position);
            }

            elapsed += Time.fixedDeltaTime;
            alterEgoTimer += Time.fixedDeltaTime;
        }

        rigidbody3d.MovePosition(targetPosition);

        stepFlag = false;
    }
}
