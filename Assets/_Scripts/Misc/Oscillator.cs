using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [Tooltip("Frequency of the Sine oscillation")]
    [SerializeField] private float _frequency = 0.1f;
    public float Frequency {  
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
    [SerializeField] private float maxValue = 3f;
    //private float timerDuration = 5;

    [Tooltip("Set if Progress should be repeated as loop or only one time")]
    [SerializeField] private bool isLoopable = true;
    private bool loopCompleted = false;

    [Tooltip("Set the starting position of the oscillation")]
    [SerializeField]
    [Range(0f, 1f)]
    private float startingPeriodOffset = 0;

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
        period = (Mathf.PI * 2) / Frequency;
        timer = period * startingPeriodOffset;
    }

    private void FixedUpdate()
    {
        PerformOscillation();
    }

    private void PerformOscillation()
    {
        if (timer < period && loopCompleted == false)
        {
            progress = maxValue * (Mathf.Sin(timer * Frequency));
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
