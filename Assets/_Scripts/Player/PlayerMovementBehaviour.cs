using UnityEngine;
using UnityEngine.InputSystem;
using static SharedLogics;

public class PlayerMovementBehaviour : MonoBehaviour
{
    #region Variables
    public float CurrentMovementSpeed;
    public float MovementSpeed = 15f;
    public float PreciseMovementSpeed;

    [Space(10)]

    [SerializeField] private float maxReachablePlayerVerticalOffset = 0f;

    [Space(10)]

    [SerializeField] private Animator shipAnimator;
    [SerializeField] private SpriteRenderer shipSprite;
    private Rigidbody2D rb;

    private Vector2 screenBounds;
    private Vector2 moveDirection = Vector2.zero;

    private float objWidth;
    private float objHeight;

    #endregion

    #region Unity functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        //Camera boundaries
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objWidth = shipSprite.bounds.size.x / 2;
        objHeight = shipSprite.bounds.size.y / 2;

        CurrentMovementSpeed = MovementSpeed;
        PreciseMovementSpeed = MovementSpeed / 2;
    }

    private void Update()
    {
        moveDirection = InputManager.PlayerMove.ReadValue<Vector2>();
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
        InputManager.PlayerDodge.performed += OnDodge;
        InputManager.PreciseMovement.started += OnPreciseMovementStart;
        InputManager.PreciseMovement.canceled += OnPreciseMovementEnd;
    }

    private void OnDestroy()
    {
        InputManager.PlayerDodge.performed -= OnDodge;
        InputManager.PreciseMovement.started -= OnPreciseMovementStart;
        InputManager.PreciseMovement.canceled -= OnPreciseMovementEnd;
    }
    #endregion

    #region PlayerInput subscribed actions
    private void OnMove()
    {
        SetAnimatorValue(ref shipAnimator, AnimatorStrings.HorizontalMovingDirection, moveDirection.x);
        rb.velocity = new Vector2(moveDirection.x * CurrentMovementSpeed, moveDirection.y * CurrentMovementSpeed);
    }

    private void OnDodge(InputAction.CallbackContext context)
    {
        Debug.Log("Dodged!");
    }

    private void OnPreciseMovementStart(InputAction.CallbackContext context)
    {
        CurrentMovementSpeed = PreciseMovementSpeed;
    }

    private void OnPreciseMovementEnd(InputAction.CallbackContext context)
    {
        CurrentMovementSpeed = MovementSpeed;
    }
    #endregion

    #region Utility private functions
    private void ClampPlayerToCamera()
    {
        Vector2 clampPos = transform.position;
        clampPos.x = Mathf.Clamp(clampPos.x, (screenBounds.x * -1) + objWidth, screenBounds.x - objWidth);
        clampPos.y = Mathf.Clamp(clampPos.y, (screenBounds.y * -1) + objHeight, ((screenBounds.y) - objHeight) * maxReachablePlayerVerticalOffset);
        transform.position = clampPos;
    }

    #endregion

}
