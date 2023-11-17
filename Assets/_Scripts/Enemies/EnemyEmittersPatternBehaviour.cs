using ND_VariaBULLET;
using UnityEngine;
using static SharedLogics;

[RequireComponent(typeof(Oscillator))]
public class EnemyEmittersPatternBehaviour : MonoBehaviour
{
    [Tooltip("Set the oscillator used for this specific behaviour. If not set, automatically get the first available")]
    [SerializeField] private Oscillator oscillator;

    [Tooltip("Set the range offset, applied to the raw output value of the oscillator")]
    [SerializeField] private float RangeOffset = 0;

    [Tooltip("Specify which variable from SpreadPattern should oscillate")]
    [SerializeField] private OscillatingEmitterParameter oscillatingParameter;
    private SpreadPattern spreadPattern;
    private float selectedParameter;

    private void Awake()
    {
        spreadPattern = GetComponent<SpreadPattern>();

        if (oscillator == null)
            oscillator = GetComponent<Oscillator>();
    }

    private void Update()
    {
        SetOscillatingParameter();
    }

    private void SetOscillatingParameter()
    {
        switch (oscillatingParameter)
        {
            case OscillatingEmitterParameter.PITCH:
                spreadPattern.Pitch = oscillator.progress + RangeOffset;
                break;

            case OscillatingEmitterParameter.SPREAD_DEGREES:
                spreadPattern.SpreadDegrees = oscillator.progress + RangeOffset;
                break;

            case OscillatingEmitterParameter.SPREAD_RADIUS:
                spreadPattern.SpreadRadius = oscillator.progress + RangeOffset;
                break;

            case OscillatingEmitterParameter.ROTATION:
                spreadPattern.ParentRotation = oscillator.progress + RangeOffset;
                break;
        }
    }

    //private void SetReference(ref float original)
    //{
    //    selectedParameter = original;
    //}
}

