using UnityEngine;
using UnityEngine.UIElements;

public class Gizmo : MonoBehaviour
{

    [SerializeField] EnemyMove enemyMove;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < enemyMove.Position.Length - 1; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(enemyMove.Position[i], enemyMove.Position[i + 1]);
        }
    }
}
