using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [Tooltip("Frequency of the Sine oscillation")]
    [SerializeField] private float frequency = 0.1f;

    [Tooltip("Max value reached during the oscillation")]
    [SerializeField] private float maxValue = 3f;
    //private float timerDuration = 5;

    [Tooltip("Set if Progress should be repeated as loop or only one time")]
    [SerializeField] private bool isLoopable = true;
    private bool loopCompleted = false;

    [Space(10)]
    [Tooltip("Current value of the oscillation")]
    [SerializeField]
    private float _progress;
    public float progress
    {
        get
        {
            return _progress;
        }
        private set
        {
            _progress = value;
        }
    }

    private float timer;
    private float period;

    private void Awake()
    {
        period = (Mathf.PI * 2) / frequency;
    }

    private void FixedUpdate()
    {
        PerformOscillation();
    }

    private void PerformOscillation()
    {
        if (timer < period && loopCompleted == false)
        {
            progress = maxValue * (Mathf.Sin(timer * frequency));
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            if (isLoopable == false)
            {
                loopCompleted = true;
                progress = 0;
            }
        }
    }
}
