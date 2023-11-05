using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class SharedMethods
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
        VERTICAL
    }
}

