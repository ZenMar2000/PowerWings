using UnityEngine;
using UnityEngine.InputSystem;
using static SharedMethods;

public class PlayerMovementBehaviour : MonoBehaviour
{
    public float MovementSpeed = 5f;

    [SerializeField] private float maxPlayerHeightOffset = 0.5f;
    private Vector2 screenBounds;
    private float objWidth;
    private float objHeight;

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

        //Camera boundaries
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    private void Update()
    {
        moveDirection = playerMove.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    private void LateUpdate()
    {
        ClampPlayerToCamera();
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

    #region Private functions
    private void ClampPlayerToCamera()
    {
        Vector2 clampPos = transform.position;
        clampPos.x = Mathf.Clamp(clampPos.x, (screenBounds.x * -1) + objWidth, screenBounds.x - objWidth);
        clampPos.y = Mathf.Clamp(clampPos.y, (screenBounds.y * -1) + objHeight, ((screenBounds.y) - objHeight) * maxPlayerHeightOffset);
        transform.position = clampPos;
    }

    #endregion

}
