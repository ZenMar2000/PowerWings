using UnityEngine;
using UnityEngine.Splines;

public class EnemySingleSpawnBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject EnemyShipPrefab;
    public GameObject SplinePathPrefab;
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

            splineAnimate.Duration = splineBehaviour.MovementSpeed;
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
