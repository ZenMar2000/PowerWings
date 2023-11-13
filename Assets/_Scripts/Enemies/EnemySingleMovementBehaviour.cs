using UnityEngine;
using static SharedLogics;

[RequireComponent(typeof(MovementDirection))]
public class EnemySingleMovementBehaviour : MonoBehaviour
{
    #region inspector variables
    /// <summary>
    /// Title of the movement type. Not used in the script, but useful for remembering what it does.
    /// </summary>
    [SerializeField] private string Title;

    /// <summary>
    /// Animator of the ship
    /// </summary>
    [SerializeField] private Animator shipAnimator;

    /// <summary>
    /// Select if movement handled by script is horizontal or vertical
    /// </summary>
    [SerializeField] private MovementDirection movementDirection;

    /// <summary>
    /// Indicates how fast is the movement
    /// </summary>
    [SerializeField] private float distanceToMove;
    [Space(20)]
    /// <summary>
    /// Indicates if the movement is a loop
    /// </summary>
    [SerializeField] private bool isLoopingMovement;
    [SerializeField] private LoopType loopType;
    [SerializeField] private float lerpValue;

    [Space(10)]
    /// <summary>
    /// Set after how much time the speed will be reversed
    /// </summary>
    [SerializeField] private float loopingTime;
    #endregion

    #region private internal variables
    private Rigidbody2D rb;
    private float loopTimer = 0;
    private Animator animator;

    private bool _isLerping;
    private float _timeStartedLerping;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    #endregion

    #region Unity functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    
    }
    // Update is called once per frame
    private void Update()
    {
       
    }

    private void FixedUpdate()
    {
   
    }
    #endregion


    #region Utility private functions
   

    #endregion
}
