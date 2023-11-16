using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySplineAnimationBehaviour : MonoBehaviour
{
    public SplineAnimate.LoopMode LoopMode;
    public SplineAnimate.EasingMode EasingMode;
    public float MovementSpeed = 10f;
}
