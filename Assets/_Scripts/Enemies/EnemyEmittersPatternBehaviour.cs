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

    [Tooltip("If true, emitters will rotate toward player")]
    [SerializeField] private bool turnTowardPlayer = false;

    private SpreadPattern spreadPattern;
    private Transform playerPosition;

    private void Awake()
    {
        spreadPattern = GetComponent<SpreadPattern>();
        if (oscillator == null)
            oscillator = GetComponent<Oscillator>();

        playerPosition = GameInfo.Player.GetComponentInChildren<PlayerMovementBehaviour>().transform;

    }

    private void FixedUpdate()
    {
        SetOscillatingParameter();
        AimAtPlayer();
    }

    private void SetOscillatingParameter()
    {
        switch (oscillatingParameter)
        {
            case OscillatingEmitterParameter.PITCH:
                spreadPattern.Pitch = oscillator.Progress + RangeOffset;
                break;

            case OscillatingEmitterParameter.SPREAD_DEGREES:
                spreadPattern.SpreadDegrees = oscillator.Progress + RangeOffset;
                break;

            case OscillatingEmitterParameter.SPREAD_RADIUS:
                spreadPattern.SpreadRadius = oscillator.Progress + RangeOffset;
                break;

            case OscillatingEmitterParameter.ROTATION:
                spreadPattern.ParentRotation = oscillator.Progress + RangeOffset + (turnTowardPlayer == false ? 0 : GetPlayerAngle());
                break;
        }
    }

    private void AimAtPlayer()
    {
        if (turnTowardPlayer && oscillatingParameter != OscillatingEmitterParameter.ROTATION)
        {
            spreadPattern.ParentRotation = GetPlayerAngle();
        }
    }

    private float GetPlayerAngle()
    {
        Vector2 targetDir = playerPosition.position - transform.position;
        return Vector2.Angle(Vector2.right, targetDir) * -1;
    }
}

