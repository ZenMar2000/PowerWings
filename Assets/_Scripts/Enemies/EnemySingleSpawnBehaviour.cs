using UnityEngine;
using UnityEngine.Splines;

public class EnemySingleSpawnBehaviour : MonoBehaviour
{
    [Tooltip("The enemy ship that will be instantiated")]
    [SerializeField] private GameObject EnemyShipPrefab;

    [HideInInspector]
    [Tooltip("Spline that will be used as path for the enemy ship")]
    public GameObject SplinePathPrefab;

    [HideInInspector]
    [Tooltip("Override movement speed set in splineBehaviour")]
    public float overrideMovementSpeed = -1;

    [HideInInspector]
    [Tooltip("Set true to start on a random position along the spline")]
    public bool StartWithRandomSplinePosition = false;

    [HideInInspector]
    [Tooltip("Set the position along the spline where it will start")]
    [Range(0, 1)]
    public float SplineAnimationStartOffset = 0;

    [HideInInspector]
    [Tooltip("If true, create an oscillator, used for spawning the ship off screen and moving them inside the camera view range")]
    public bool HasEnterMove = false;

    [Tooltip("Speed at which the whole spline is moved. Smaller is slower, higher is faster")]
    /* [ShowIf("HasEnterMove", true, true)]*/
    public float SplineMovementSpeed;

    [Tooltip("Y offset from the original spawn point")]
    /*[ShowIf("HasEnterMove", true, true)]*/
    public float OnEnterSplineOffsetValue;

    private Oscillator SplineOscillatorMovement;

    [HideInInspector]
    public EnemyGroupHandler GroupHandler;

    private SplineContainer container;
    private SplineAnimate splineAnimate;
    private EnemySplineAnimationBehaviour splineBehaviour;

    private GameObject spline;
    private GameObject ship;
    private Vector3 startingPosition;

    private Transform followedTarget;
    private void Start()
    {
        startingPosition = transform.position;
        if (HasEnterMove == true)
        {
            SplineOscillatorMovement = gameObject.AddComponent<Oscillator>();
            SplineOscillatorMovement.Frequency = SplineMovementSpeed;
            SplineOscillatorMovement.MaxValue = OnEnterSplineOffsetValue * -1;
            SplineOscillatorMovement.TimerCutOut = 0.25f;
            SplineOscillatorMovement.IsLoopable = false;
        }

        if (SplinePathPrefab != null && EnemyShipPrefab != null)
        {
            spline = Instantiate(SplinePathPrefab, transform.position, Quaternion.identity, transform);
            container = GetComponentInChildren<SplineContainer>();
            splineBehaviour = spline.GetComponent<EnemySplineAnimationBehaviour>();


            ship = Instantiate(EnemyShipPrefab, transform.position, Quaternion.identity, transform);
            splineAnimate = ship.GetComponentInChildren<SplineAnimate>();

            splineAnimate.MaxSpeed = overrideMovementSpeed == 0 ? splineBehaviour.MovementSpeed : overrideMovementSpeed;
            splineAnimate.Easing = splineBehaviour.EasingMode;
            splineAnimate.Loop = splineBehaviour.LoopMode;
            splineAnimate.Container = container;
            splineAnimate.StartOffset = StartWithRandomSplinePosition == false ? SplineAnimationStartOffset : Random.Range(0f, 1f);
            splineAnimate.enabled = true;


        }
        else if (EnemyShipPrefab != null)
        {
            ship = Instantiate(EnemyShipPrefab, transform.position, Quaternion.identity, transform);
            splineAnimate = ship.GetComponentInChildren<SplineAnimate>();
            Destroy(splineAnimate);
        }
    }

    private void FixedUpdate()
    {
        if (followedTarget != null)
            FollowTarget();
        else
            EnterMove();
    }

    private void EnterMove()
    {
        if (HasEnterMove)
        {
            if (spline != null)
            {
                spline.transform.position = new Vector3(spline.transform.position.x,
                   startingPosition.y + SplineOscillatorMovement.Progress,
                   spline.transform.position.z);
            }
            else
            {
                ship.transform.position = new Vector3(ship.transform.position.x,
                    startingPosition.y + SplineOscillatorMovement.Progress,
                    ship.transform.position.z);
            }
        }
    }

    private void FollowTarget()
    {
        if(followedTarget != null)
        {
            spline.transform.position = followedTarget.transform.position;
        }
    }
    private void OnDestroy()
    {
        if (GroupHandler != null)
            GroupHandler.EnemiesAlive--;
    }

    public void SetTargetToFollow(Transform target)
    {
        followedTarget = target;
    }
}
