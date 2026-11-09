using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


public class EnemyMove : MonoBehaviour
{

    [SerializeField] Vector3[] position;
    public Vector3[] Position => position;

    [SerializeField] Rigidbody rigidbody3d;

    int counter = 0;

    int roundTrip = 1;

    [SerializeField] float moveSpeed;

    private void Start()
    {
        StartCoroutine(MoveRoutine(position[counter]));
    }

    private IEnumerator MoveRoutine(Vector3 targetPosition)
    {
        if(counter == position.Length - 1)
        {
            roundTrip = roundTrip * (-1);
        }

        counter += roundTrip;

        if(counter == 0)
        {
            roundTrip = roundTrip * (-1);
        }

        Vector3 direction = targetPosition - rigidbody3d.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0f)
        {
            rigidbody3d.rotation = Quaternion.LookRotation(direction);
        }

        Debug.Log(rigidbody3d.position);

        if (moveSpeed <= 0f)
        {
            rigidbody3d.position = targetPosition;
            yield break;
        }

        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        while ((targetPosition - rigidbody3d.position).sqrMagnitude > 0.0001f)
        {
            rigidbody3d.MovePosition(Vector3.MoveTowards(
                rigidbody3d.position,
                targetPosition,
                moveSpeed * Time.fixedDeltaTime));
            yield return waitForFixedUpdate;
        }

        rigidbody3d.MovePosition(targetPosition);

        

        StartCoroutine(MoveRoutine(position[counter]));
    }

}
