using UnityEngine;
using UnityEngine.Splines;

public class EnemySingleSpawnBehaviour : MonoBehaviour
{
    [Tooltip("The enemy ship that will be instantiated")]
    [SerializeField] private GameObject EnemyShipPrefab;

    [Tooltip("Spline that will be used as path for the enemy ship")]
    public GameObject SplinePathPrefab;

    [Tooltip("Override movement speed set in splineBehaviour")]
    public float overrideMovementSpeed = -1;

    [Tooltip("Set the position along the spline where it will start")]
    [Range(0, 1)]
    public float SplineAnimationStartOffset = 0;

    public EnemyGroupHandler GroupHandler;

    private SplineContainer container;
    private SplineAnimate splineAnimate;
    private EnemySplineAnimationBehaviour splineBehaviour;

    private void Start()
    {
        //Vector3 spawnPosition = new Vector3(transform.position.x + spawnPositionOffset.x, transform.position.y + spawnPositionOffset.y, transform.position.z + spawnPositionOffset.z);

        if (SplinePathPrefab != null)
        {
            GameObject spline = Instantiate(SplinePathPrefab, transform.position, Quaternion.identity, transform);
            container = GetComponentInChildren<SplineContainer>();
            splineBehaviour = spline.GetComponent<EnemySplineAnimationBehaviour>();

            GameObject ship = Instantiate(EnemyShipPrefab, transform.position, Quaternion.identity, transform);
            splineAnimate = ship.GetComponentInChildren<SplineAnimate>();

            splineAnimate.MaxSpeed = overrideMovementSpeed == 0 ? splineBehaviour.MovementSpeed : overrideMovementSpeed;
            splineAnimate.Easing = splineBehaviour.EasingMode;
            splineAnimate.Loop = splineBehaviour.LoopMode;
            splineAnimate.Container = container;
            splineAnimate.StartOffset = SplineAnimationStartOffset;

        }
        else
        {
            GameObject ship = Instantiate(EnemyShipPrefab, transform.position, Quaternion.identity, transform);
            splineAnimate = ship.GetComponentInChildren<SplineAnimate>();
            Destroy(splineAnimate);
        }
    }

    private void OnDestroy()
    {
        GroupHandler.EnemiesAlive--;
    }
}
