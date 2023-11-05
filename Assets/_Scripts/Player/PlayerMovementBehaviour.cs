using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static SharedMethods;

public class PlayerMovementBehaviour : MonoBehaviour
{

    public float MovementSpeed = 5f;
    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody2D rb;

    private PowerInputActions playerInput;
    private InputAction playerMove;
    private InputAction playerFire;
    private Animator animator;

    #region Unity functions
    private void Awake()
    {
        Application.targetFrameRate = 120;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerInput = new PowerInputActions();
    }

    private void Update()
    {
        moveDirection = playerMove.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        OnMove();
    }
    private void OnEnable()
    {
        playerMove = playerInput.Player.Move;
        playerMove.Enable();

        playerFire = playerInput.Player.Fire;
        playerFire.Enable();
    }

    private void OnDisable()
    {
        playerMove.Disable();
        playerFire.Disable();
    }
    #endregion

    public void OnMove()
    {
        SetAnimatorValue(ref animator, AnimatorStrings.HorizontalMovingDirection, moveDirection.x);
        rb.velocity = new Vector2(moveDirection.x * MovementSpeed, moveDirection.y * MovementSpeed);
    }
}
