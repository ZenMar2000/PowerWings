using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class SharedMethods
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

    //SET
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
        VERTICAL
    }
}

