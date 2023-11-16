using UnityEngine;
using static SharedLogics;
public class EnemyMovementManager : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float previousXPosition;
    [SerializeField] private float currentXPosition => transform.position.x;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        previousXPosition = transform.position.x;
    }

    private void Update()
    {
        CheckAndSetAnimatorValue();

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
}
