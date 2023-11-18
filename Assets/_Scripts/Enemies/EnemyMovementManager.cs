using UnityEngine;
using static SharedLogics;
public class EnemyMovementManager : MonoBehaviour
{
    [SerializeField] private bool rotateTowardPlayer = false;
    [SerializeField] private Transform player;
    
    private Animator animator;
    private float currentXPosition => transform.position.x;
    private float previousXPosition;
    private Vector3 targetRotationAngle;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        previousXPosition = transform.position.x;
        player = GameManager.Player.GetComponentInChildren<PlayerMovementBehaviour>().transform;
    }

    private void Update()
    {
        CheckAndSetAnimatorValue();
    }

    private void FixedUpdate()
    {
        CheckAndRotateTowardPlayer();
    }

    private void CheckAndSetAnimatorValue()
    {
        if (currentXPosition != previousXPosition)
        {
            SetAnimatorValue(ref animator, AnimatorStrings.HorizontalMovingDirection, (currentXPosition - previousXPosition) * 25f);
            previousXPosition = currentXPosition;
        }
        else
            SetAnimatorValue(ref animator, AnimatorStrings.HorizontalMovingDirection, 0f);

    }

    private void CheckAndRotateTowardPlayer()
    {
        if (rotateTowardPlayer && player != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, (player.position - transform.position) * -1);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 4);
        }
    }
}
