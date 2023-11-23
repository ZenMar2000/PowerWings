using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [Tooltip("Frequency of the Sine oscillation")]
    [SerializeField] private float _frequency = 0.1f;
    public float Frequency
    {
        get
        {
            return _frequency;
        }
        set
        {
            _frequency = value;
            period = (Mathf.PI * 2) / value;
        }
    }

    [Tooltip("Max value reached during the oscillation")]
    public float MaxValue = 3f;
    //private float timerDuration = 5;

    [Tooltip("Set if Progress should be repeated as loop or only one time")]
    public bool IsLoopable = true;
    private bool loopCompleted = false;

    [Tooltip("Set the starting position of the oscillation")]
    [Range(0f, 1f)]
    public float StartingPeriodOffset = 0;

    [Tooltip("Set if progress should stop at a certain point before reaching the end")]
    [Range(0f, 1f)]
    public float TimerCutOut = 0;

    [Space(10)]
    [Tooltip("Current value of the oscillation")]
    [SerializeField]
    private float _progress;
    public float Progress
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
        period = (Mathf.PI * 2) / Frequency;
        timer = period * StartingPeriodOffset;
    }

    private void FixedUpdate()
    {
        PerformOscillation();
    }

    private void PerformOscillation()
    {
        CheckTimerCutOut();
        CheckOscillationTimers();
    }

    private void CheckTimerCutOut()
    {
        if (TimerCutOut > 0)
        {
            if (timer >= period * TimerCutOut)
            {
                if (IsLoopable)
                {
                    timer = 0;
                }
                else
                {
                    loopCompleted = true;
                }
            }
        }
    }

    private void CheckOscillationTimers()
    {
        if (timer < period && loopCompleted == false)
        {
            Progress = MaxValue * (Mathf.Sin(timer * Frequency));
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            if (IsLoopable == false && !loopCompleted)
            {
                loopCompleted = true;
                Progress = 0;
            }
        }
    }
}
