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

    [SerializeField] float roundTripTime;

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


        Vector3 startPosition = rigidbody3d.position;

        Vector3 direction = targetPosition - rigidbody3d.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0f)
        {
            rigidbody3d.rotation = Quaternion.LookRotation(direction);
        }

        Debug.Log(rigidbody3d.position);

        if (roundTripTime <= 0f)
        {
            rigidbody3d.position = targetPosition;
            yield break;
        }

        float elapsed = 0f;
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        while (elapsed < roundTripTime)
        {
            float t = Mathf.Clamp01(elapsed / roundTripTime);
            rigidbody3d.MovePosition(Vector3.Lerp(startPosition, targetPosition, t));
            yield return waitForFixedUpdate;

            elapsed += Time.fixedDeltaTime;
        }

        rigidbody3d.MovePosition(targetPosition);

        

        StartCoroutine(MoveRoutine(position[counter]));
    }

}
