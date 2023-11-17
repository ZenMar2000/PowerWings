using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    public enum OscillatingEmitterParameter
    {
        PITCH,
        SPREAD_DEGREES,
        SPREAD_RADIUS,
        ROTATION,
    }

    //public static List<VariableInfo> GetPublicVariables(object obj)
    //{
    //    List<VariableInfo> publicVariables = new List<VariableInfo>();
    //    FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
    //    foreach (FieldInfo field in fields)
    //    {
    //        publicVariables.Add(new VariableInfo { Name = field.Name, type = field.FieldType });
    //    }
    //    return publicVariables;
    //}
}

//public class VariableInfo
//{
//    public string Name { get; set; }
//    public Type type { get; set; }
//}

