using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputAction moveAction;

    Rigidbody rigidbody3D;

    private void OnEnable()
    {
        moveAction.Enable();
    }


    private void OnDisable()
    {
        moveAction.Disable();
    }


    void Start()
    {
        rigidbody3D = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = moveAction.ReadValue<Vector3>();

        rigidbody3D.linearVelocity = position;




    }
}
