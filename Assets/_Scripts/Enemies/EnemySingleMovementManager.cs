using UnityEngine;
using static SharedLogics;

public class EnemySingleMovementManager : MonoBehaviour
{
    public MovementInfo[] MovementTargets;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {

    }

}
