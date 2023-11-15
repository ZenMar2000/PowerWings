using System.Collections.Generic;
using UnityEngine;
using static SharedLogics;

public class EnemyMovementManager : MonoBehaviour
{
    private Animator animator;

    private float interpolationAmount;
    private float previousInterpolationAmount;

    private Rigidbody2D rb;


    [SerializeField] Vector2 currentSpeed;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        //SetMovementTargetsOffsets();
    }

    private void Update()
    {
        currentSpeed = rb.velocity;
    }
}
