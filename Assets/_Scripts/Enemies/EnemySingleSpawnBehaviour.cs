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

    [Tooltip("Spawn position of the enemy ship")]
    public Vector3 spawnPosition;

    private SplineContainer container;
    private SplineAnimate splineAnimate;
    private EnemySplineAnimationBehaviour splineBehaviour;
    private void Start()
    {
        if (SplinePathPrefab != null)
        {
            GameObject spline = Instantiate(SplinePathPrefab, spawnPosition, Quaternion.identity, transform);
            container = GetComponentInChildren<SplineContainer>();
            splineBehaviour = spline.GetComponent<EnemySplineAnimationBehaviour>();

            GameObject ship = Instantiate(EnemyShipPrefab, spawnPosition, Quaternion.identity, transform);
            splineAnimate = ship.GetComponentInChildren<SplineAnimate>();

            splineAnimate.MaxSpeed = overrideMovementSpeed != -1 ? splineBehaviour.MovementSpeed : overrideMovementSpeed;
            splineAnimate.Easing = splineBehaviour.EasingMode;
            splineAnimate.Loop = splineBehaviour.LoopMode;
            splineAnimate.Container = container;
        }
        else
        {
            GameObject ship = Instantiate(EnemyShipPrefab, spawnPosition, Quaternion.identity, transform);
        }
    }
}
