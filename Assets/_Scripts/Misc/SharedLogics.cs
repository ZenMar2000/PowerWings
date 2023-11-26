using System;
using UnityEditor;
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

        [Tooltip("Set true to start on a random position along the spline. Override SplineAnimationStartOffset")]
        public bool StartWithRandomSplinePosition;
        
        [Space(10)]
        [Tooltip("Spawn position of the enemy ship")]
        public Vector3 SpawnPositionOffset;
        
        [Space(10)]
        [Tooltip("If true, the whole Spline will move a single time, from the spawn point to the offset specified.")]
        public bool HasEnterMove;
        [Tooltip("Speed at which the whole spline is moved. Smaller is slower, higher is faster")]
        /*[ShowIf("HasEnterMove", true, true)]*/ public float EnterMovementSpeed;

        [Tooltip("Y offset from the original spawn point")]
        /*[ShowIf("HasEnterMove", true, true)]*/ public float EnterOffsetValue;

        [HideInInspector]
        public bool IsSpawned;
    }

}

///// <summary>
///// Atttribute for show a field if other field is true or false.
///// </summary>
//[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
//public sealed class ShowIfAttribute : PropertyAttribute
//{
//    public string ConditionalSourceField;
//    public bool expectedValue;
//    public bool HideInInspector;

//    /// <summary>
//    /// Create the attribute for show a field x if field y is true or false.
//    /// </summary>
//    /// <param name="ConditionalSourceField">name of field y type boolean </param>
//    /// <param name="expectedValue"> what value should have the field y for show the field x</param>
//    /// <param name="HideInInspector"> if should hide in the inspector or only disable</param>
//    public ShowIfAttribute(string ConditionalSourceField, bool expectedValue, bool HideInInspector = false)
//    {
//        this.ConditionalSourceField = ConditionalSourceField;
//        this.expectedValue = expectedValue;
//        this.HideInInspector = HideInInspector;
//    }
//}

//[CustomPropertyDrawer(typeof(ShowIfAttribute))]
//public class ConditionalHidePropertyDrawer : UnityEditor.PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//#if UNITY_EDITOR
//        ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;
//        bool enabled = GetConditionalSourceField(property, condHAtt);
//        GUI.enabled = enabled;

//        // if is enable draw the label
//        if (enabled)
//            EditorGUI.PropertyField(position, property, label, true);
//        // if is not enabled but we want not hide it, then draw it disabled
//        else if (!condHAtt.HideInInspector)
//            EditorGUI.PropertyField(position, property, label, false);
//        // else hide it ,dont draw it
//        else return;
//#endif
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//#if UNITY_EDITOR
//        ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;
//        bool enabled = GetConditionalSourceField(property, condHAtt);

//        // if is enable draw the label
//        if (enabled)
//        {
//            return EditorGUI.GetPropertyHeight(property, label, true);
//        }
//        // if is not enabled but we want not hide it, then draw it disabled
//        else
//        {
//            if (!condHAtt.HideInInspector)
//                return EditorGUI.GetPropertyHeight(property, label, false);
//            // else hide it
//            else
//                return -EditorGUIUtility.standardVerticalSpacing; // Oculta el campo visualmente.
//        }
//#else
//        return 0f;
//#endif
//    }

//    /// <summary>
//    /// Get if the conditional what expected is true.
//    /// </summary>
//    /// <param name="property"> is used for get the value of the property and check if return enable true or false </param>
//    /// <param name="condHAtt"> is the attribute what contains the values what we need </param>
//    /// <returns> only if the field y is same to the value expected return true</returns>
//    private bool GetConditionalSourceField(SerializedProperty property, ShowIfAttribute condHAtt)
//    {
//#if UNITY_EDITOR
//        bool enabled = false;
//        string propertyPath = property.propertyPath;
//        string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField);
//        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

//        if (sourcePropertyValue != null)
//        {
//            enabled = sourcePropertyValue.boolValue;
//            if (enabled == condHAtt.expectedValue) enabled = true;
//            else enabled = false;
//        }
//        else
//        {
//            string warning = "ConditionalHideAttribute: No se encuentra el campo booleano [" + condHAtt.ConditionalSourceField + "] en " + property.propertyPath;
//            warning += " Asegúrate de especificar correctamente el nombre del campo condicional.";
//            Debug.LogWarning(warning);
//        }

//        return enabled;
//#else
//        return false;
//#endif
    //}
//}
