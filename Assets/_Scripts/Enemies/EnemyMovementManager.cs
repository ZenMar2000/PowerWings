using System.Collections.Generic;
using UnityEngine;
using static SharedLogics;

public class EnemyMovementManager : MonoBehaviour
{
    public List<MovementInfo> MovementTargets;
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float movementDuration = 5;

    [Range (0f, 1f)]
    [SerializeField] private float targetInterpolation = 0.5f;

    private Animator animator;
    [SerializeField] private int currentTarget = 0;

    [SerializeField] private float progress = 0;
    [SerializeField] private float timer = 0;

    private bool firstTargetPass = true;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        SetMovementTargetsOffsets();
    }

    private void FixedUpdate()
    {
        if (timer < movementDuration)
        {
            PerformMovement();
        }
        else
        {
            
        }
    }

    private void PerformMovement()
    {
        SmoothProgress();
        transform.position = Vector3.Lerp(transform.position, MovementTargets[currentTarget].MovementPositionOffset, progress);
        timer += Time.deltaTime;
    }

    private void SetNextMovementTarget()
    {
        if (CheckIfMovementPerformable())
        {
            timer = 0;
        }
        currentTarget = (currentTarget + 1) % MovementTargets.Count;

        if (firstTargetPass == true && currentTarget == MovementTargets.Count)
            firstTargetPass = false;
    }

    private bool CheckIfMovementPerformable()
    {
        if (firstTargetPass) return true;
        return MovementTargets[currentTarget].LoopablePosition;
    }

    private void SmoothProgress()
    {
        progress = timer / movementSpeed;
        progress = Mathf.Lerp(-Mathf.PI / 2, Mathf.PI / 2, progress);
        progress = Mathf.Sin(progress);
        progress = (progress / 2f) + 0.5f;
    }

    private void SetMovementTargetsOffsets()
    {
        for(int i = 0; i < MovementTargets.Count; i++)
        {
            MovementInfo mi = MovementTargets[i];
            mi.MovementPositionOffset.x += transform.position.x;
            mi.MovementPositionOffset.y += transform.position.y;
            mi.MovementPositionOffset.z += transform.position.z;

            MovementTargets[i] = mi;

        }
    }
}
