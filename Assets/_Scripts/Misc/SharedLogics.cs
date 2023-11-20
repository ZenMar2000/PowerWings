using System;
using UnityEngine;

public static class SharedLogics
{
    public static void SetAnimatorValue(ref Animator animator, string stringName, float value)
    {
        animator.SetFloat(stringName, value);
    }
    public static void SetAnimatorValue(ref Animator animator, string stringName, int value)
    {
        animator.SetInteger(stringName, value);
    }
    public static void SetAnimatorValue(ref Animator animator, string stringName, bool value)
    {
        animator.SetBool(stringName, value);
    }


    public enum MovementDirection
    {
        HORIZONTAL,
        VERTICAL,
    }

    public enum LoopType
    {
        INVERT_SPEED,
        CONTINUE,
        STOP_ALL_MOVEMENTS,
        STOP_HORIZONTAL_MOVEMENT,
        STOP_VERTICAL_MOVEMENT,
    }

    public enum OscillatingEmitterParameter
    {
        PITCH,
        SPREAD_DEGREES,
        SPREAD_RADIUS,
        ROTATION,
    }

    [Serializable]
    public struct EnemySpawnContainer
    {
        [Tooltip("Enemy ship spawner gameobject")]
        public GameObject EnemyShipSpawner;

        [Tooltip("Spline path game object")]
        public GameObject SplinePathPrefab;
        [Space(10)]
        [Tooltip("Spawn enemy ship with a delay")]
        public float SpawnDelay;

        [Tooltip("Override the movement speed. 0 = no override wil occur")]
        public float MovementSpeedOverride;

        [Tooltip("Set the position along the spline where it will start")]
        [Range(0, 1)]
        public float SplineAnimationStartOffset;
        [Tooltip("Spawn position of the enemy ship")]

        [Space(10)]
        public Vector3 SpawnPositionOffset;
        public bool IsSpawned;
    }

}