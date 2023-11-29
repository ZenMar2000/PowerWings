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

    public static void TurnOffMusic()
    {
        GameObject[] musicPlayers = GameObject.FindGameObjectsWithTag("Music");
        foreach (GameObject musicPlayer in musicPlayers)
        {
            GameObject.Destroy(musicPlayer);
        }
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
        X_SPREAD, 
        Y_SPREAD,
    }

    public enum RotationDirection
    {
        CLOCKWISE,
        COUNTER_CLOCKWISE,
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

        [Tooltip("Set true to start on a random position along the spline. Override SplineAnimationStartOffset")]
        public bool StartWithRandomSplinePosition;
        
        [Space(10)]
        [Tooltip("Spawn position of the enemy ship")]
        public Vector3 SpawnPositionOffset;
        
        [Space(10)]
        [Tooltip("If true, the whole Spline will move a single time, from the spawn point to the offset specified")]
        public bool HasEnterMove;
        [Tooltip("Speed at which the whole spline is moved. Smaller is slower, higher is faster")]
        /*[ShowIf("HasEnterMove", true, true)]*/ public float EnterMovementSpeed;

        [Tooltip("Y offset from the original spawn point")]
        /*[ShowIf("HasEnterMove", true, true)]*/ public float EnterOffsetValue;

        [Space(10)]
        [Tooltip("Spline of this ship will follow the transform of another ship. Override HasEnterMove until the target dies, then it will trigger HasEnterMove if true")]
        public bool SplineFollowTarget;
        [Tooltip("Set the List index of the target to follow. NOTICE It will check the currently spawn ships, not the EnemySpawnContainer position")]
        public int FollowTargetIndex;

        [HideInInspector]
        public bool IsSpawned;
    }

    public enum SliderType
    {
        MUSIC,
        EFFECTS,
    }

}
