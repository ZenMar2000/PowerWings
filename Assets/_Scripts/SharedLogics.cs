using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class SharedLogics
{
    //SET
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

    //GET
    //public static float GetAnimatorFloatValue(ref Animator animator, string stringName)
    //{
    //    return animator.GetFloat(stringName);
    //}

    //public static int GetAnimatorIntValue(ref Animator animator, string stringName)
    //{
    //    return animator.GetInteger(stringName);
    //}
    //public static bool GetAnimatorBoolValue(ref Animator animator, string stringName)
    //{
    //    return animator.GetBool(stringName);
    //}

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

    [System.Serializable]
    public struct MovementInfo
    {
        public Vector3 MovementPositionOffset;
        public bool LoopablePosition;
    }
}

