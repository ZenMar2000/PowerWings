using UnityEngine;
using static SharedLogics;


public class ChargeLevelBehaviour : MonoBehaviour
{
    private PlayerProjectileEmitterBehaviour playerProjectileEmitterBehaviour;
    private Animator animator;

    //private long playerProjectileEmitterBehaviour.BulletsAccumulator => playerProjectileEmitterBehaviour.BulletsAccumulator;
    private long oldBulletsAccumulatorValue = 0;
    private long shootingValue = 0;
    private int displayIndexMaxValue = 16;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerProjectileEmitterBehaviour = GameManager.Player.GetComponentInChildren<PlayerProjectileEmitterBehaviour>();
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
            SetAnimatorValue(ref animator, AnimatorStrings.HealthSegment, (int)(Mathf.Ceil((playerProjectileEmitterBehaviour.BulletsAccumulator * 0.0625f))));
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
