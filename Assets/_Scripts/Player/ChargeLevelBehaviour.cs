using UnityEngine;
using static SharedLogics;


public class ChargeLevelBehaviour : MonoBehaviour
{
    private PlayerProjectileEmitterBehaviour playerProjectileEmitterBehaviour;
    private Animator animator;

    //private long playerProjectileEmitterBehaviour.BulletsAccumulator => playerProjectileEmitterBehaviour.BulletsAccumulator;
    private long oldBulletsAccumulatorValue = 0;
    private long shootingValue = 0;
    private long tempCalculations;

    private float segmentDivisionValue;
    private int displayIndexMaxValue = 16;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        segmentDivisionValue = 1f / displayIndexMaxValue;
    }

    private void Start()
    {
        playerProjectileEmitterBehaviour = GameInfo.Player.GetComponentInChildren<PlayerProjectileEmitterBehaviour>();
        UpdateSegments();
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerProjectileEmitterBehaviour.BulletsAccumulator != oldBulletsAccumulatorValue)
        {
            UpdateSegments();
        }
    }

    private void UpdateSegments()
    {
        oldBulletsAccumulatorValue = playerProjectileEmitterBehaviour.BulletsAccumulator;
        if (!playerProjectileEmitterBehaviour.IsShooting)
        {
            shootingValue = 0;
            tempCalculations = (long)(Mathf.Ceil((playerProjectileEmitterBehaviour.BulletsAccumulator * segmentDivisionValue)));
            SetAnimatorValue(ref animator, AnimatorStrings.HealthSegment, tempCalculations > displayIndexMaxValue ? displayIndexMaxValue : (int)tempCalculations);
        }
        else
        {
            if (shootingValue == 0 && playerProjectileEmitterBehaviour.BulletsAccumulator > playerProjectileEmitterBehaviour.CurrentMaxProjectileCharge)
                shootingValue = playerProjectileEmitterBehaviour.BulletsAccumulator;

            SetAnimatorValue(ref animator, AnimatorStrings.HealthSegment, ConvertToRange());
        }
    }

    private int ConvertToRange()
    {
        if (playerProjectileEmitterBehaviour.BulletsAccumulator <= playerProjectileEmitterBehaviour.CurrentMaxProjectileCharge && shootingValue == 0)
            return (int)((playerProjectileEmitterBehaviour.BulletsAccumulator * displayIndexMaxValue) / playerProjectileEmitterBehaviour.CurrentMaxProjectileCharge);
        else
            return (int)((playerProjectileEmitterBehaviour.BulletsAccumulator * displayIndexMaxValue) / shootingValue);
    }
}
