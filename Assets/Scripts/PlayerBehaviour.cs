using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float speed, maxSpeed, jumpForce;
    [SerializeField] private Transform raycastStart;
    [SerializeField] private LayerMask raycastMask;

    private Controls controls;
    private Rigidbody2D myRigidbody2D;
    private float stickDirection;
    private Vector2 raycastDirection;

    private void OnEnable()
    {
        controls = new Controls();
        controls.Enable();
        controls.Main.Jump.performed += OnJumpPerformed;
        controls.Main.Move.performed += OnMovePerformed;
        controls.Main.Move.canceled += OnMoveCanceled;
        controls.Main.ChangeGravity.performed += OnChangeGravityPerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
        stickDirection = obj.ReadValue<float>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        stickDirection = 0;
    }

    private void OnChangeGravityPerformed(InputAction.CallbackContext obj)
    {
        var gravityDirection = obj.ReadValue<Vector2>();
        Physics2D.gravity = gravityDirection * 9.81f;
        transform.up = -gravityDirection;
        raycastDirection = gravityDirection;
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        if (Physics2D.Raycast(raycastStart.position, raycastDirection, 0.1f, raycastMask))
            myRigidbody2D.AddForce(jumpForce * transform.up, ForceMode2D.Impulse);
    }

    // Start is called before the first frame update
    private void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        raycastDirection = Vector2.down;
    }

    private void FixedUpdate()
    {
        if (myRigidbody2D.velocity.magnitude < maxSpeed)
        {
            var forceToAdd = transform.right * (stickDirection * speed);
            myRigidbody2D.AddForce(forceToAdd);
        }
    }
}
