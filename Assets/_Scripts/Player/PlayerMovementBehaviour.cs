using UnityEngine;
using UnityEngine.InputSystem;
using static SharedMethods;

public class PlayerMovementBehaviour : MonoBehaviour
{
    #region Variables
    public float MovementSpeed = 15f;

    //0 = center, 1 = top (all screen), -1 = bottom (no vertical movement)
    [SerializeField] private float maxReachablePlayerVerticalOffset = 0f;

    [SerializeField] private Animator shipAnimator;
    [SerializeField] private SpriteRenderer shipSprite;
    private Rigidbody2D rb;

    private Vector2 screenBounds;
    private Vector2 moveDirection = Vector2.zero;

    private float objWidth;
    private float objHeight;

    //Variables for Input
    private InputAction playerMove;
    private InputAction playerDodge;

    #endregion

    #region Properties
    private PowerInputActions _playerInput;
    public PowerInputActions PlayerInput 
    { get 
        { 
            return _playerInput;
        } 
        private set 
        {
            _playerInput = value;
        }
    }

    private bool _isParrying;
    public bool isParrying
    {
        get
        {
            return _isParrying;
        }
        set
        {
            _isParrying = value;
            //SetAnimatorValue(ref ShieldAnimator, AnimatorStrings.IsParrying, value);
        }
    }

    #endregion

    #region Unity functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerInput = new PowerInputActions();

        //Camera boundaries
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objWidth = shipSprite.bounds.size.x / 2;
        objHeight = shipSprite.bounds.size.y / 2;
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
        playerMove = PlayerInput.Player.Move;
        playerMove.Enable();


        playerDodge = PlayerInput.Player.Dodge;
        playerDodge.Enable();
        playerDodge.performed += OnDodge;
    }

    private void OnDisable()
    {
        playerMove.Disable();
        playerDodge.Disable();
    }

    #endregion

    #region PlayerInput subscribed actions
    private void OnMove()
    {
        SetAnimatorValue(ref shipAnimator, AnimatorStrings.HorizontalMovingDirection, moveDirection.x);
        rb.velocity = new Vector2(moveDirection.x * MovementSpeed, moveDirection.y * MovementSpeed);
    }

    private void OnDodge(InputAction.CallbackContext context)
    {
        Debug.Log("Dodged!");
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
