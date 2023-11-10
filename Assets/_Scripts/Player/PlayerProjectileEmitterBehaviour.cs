using ND_VariaBULLET;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProjectileEmitterBehaviour : MonoBehaviour
{
    //Consts
    public readonly int maxEmittersAmountPerWave = 31;

    //Publics
    public bool ShotNow = false;
    public int ShotsEmitted = 0;
    public SpreadPattern spreadController;

    //Privates
    private float tempTimer = 0; //temp, used for testing bulletAccumulator logic

    [SerializeField] private float shootingRateCooldown = 0.01f;
    /// <summary>
    /// Timer used toghether with shootingRateCooldown for repeating waves
    /// </summary>
    private float shootingtTimer = 0;

    /// <summary>
    /// Store all bullets accumulated. Will be subdivided in multiWavesRepeat and singleWaveProjectiles as soon as the player perform an attack.
    /// </summary>
    [SerializeField] private long bulletsAccumulator;

    /// <summary>
    /// Store how many waves with the full CurrentEmittersAmountPerWaves must be performed.
    /// </summary>
    [SerializeField] private int multiWavesRepeat = 0;

    /// <summary>
    /// Used for perform a single wave after all multiWavesRepeat reached 0. Shoots remaining bullets.
    /// </summary>
    [SerializeField] private int singleWaveProjectiles = 0;

    private int maxStandardProjectileCharge = 512;
    private int maxOverloadProjectileCharge = 1024;
    private int currentMaxProjectileCharge;

    private bool _isOverloaded = false;
    public bool isOverloaded
    {
        get
        {
            return _isOverloaded;
        }
        private set
        {
            _isOverloaded = value;
            SetCurrentMaxProjectileCharge();
        }
    }

    #region Properties
    private bool _isShooting = false;
    public bool IsShooting
    {
        get
        {
            return _isShooting;
        }
        private set
        {
            _isShooting = value;
        }
    }

    [SerializeField] private float _damageMultiplier = 1;
    public float DamageMultiplier
    {
        get
        {
            return _damageMultiplier;
        }
        private set
        {
            _damageMultiplier = value;
        }
    }

    [SerializeField] private int _currentEmittersAmountPerWave;
    public int CurrentEmittersAmountPerWave
    {
        get
        {
            return _currentEmittersAmountPerWave;
        }
        set
        {
            _currentEmittersAmountPerWave = value;
            CalculateShootingRate();
        }
    }

    #endregion

    #region Unity functions
    private void Awake()
    {
        CurrentEmittersAmountPerWave = 15;
        bulletsAccumulator = 8;
        currentMaxProjectileCharge = maxStandardProjectileCharge;
    }
    private void Start()
    {
        spreadController = GetComponentInChildren<SpreadPattern>();
        InputManager.PlayerAttack.started += OnAttack;

    }

    private void Update()
    {
        CheckIfAccumulatorOverloaded();
        CheckEmittedShots();
        CheckShootingStatus();

        if (IsShooting)
        {
            shootingtTimer += Time.deltaTime;
            tempTimer = 0;
        }
        //else
        //{
        //    tempTimer += Time.deltaTime;
        //    if (tempTimer > 1)
        //    {
        //        tempTimer = 0;
        //        bulletsAccumulator = bulletsAccumulator == 0 ? 1 : bulletsAccumulator * 2;
        //    }
        //}
    }

    #endregion

    #region PlayerInput subscribed actions
    private void OnAttack(InputAction.CallbackContext context)
    {
        if (CanShoot())
        {
            ReleaseAttack();
        }
    }
    #endregion

    #region Utility private functions

    private void HandlesProjectileWavesRelease()
    {
        if (!ShotNow)
        {
            shootingtTimer = 0;

            if (multiWavesRepeat > 0)
            {
                spreadController.SetAndCloneEmitters(CurrentEmittersAmountPerWave);
                multiWavesRepeat--;
                ShotNow = true;
            }
            else if (singleWaveProjectiles > 0)
            {
                spreadController.SetAndCloneEmitters(singleWaveProjectiles);
                singleWaveProjectiles = 0;
                ShotNow = true;
            }
        }
    }
    private void CheckEmittedShots()
    {
        if (ShotsEmitted == spreadController.EmitterAmount)
        {
            ShotNow = false;
            ShotsEmitted = 0;
        }

        if (shootingtTimer > shootingRateCooldown && IsShooting)
        {
            HandlesProjectileWavesRelease();
        }
    }

    private void CheckShootingStatus()
    {
        if (multiWavesRepeat <= 0 && singleWaveProjectiles <= 0)
        {
            IsShooting = false;
            DamageMultiplier = 1;
        }
    }

    public bool CanShoot()
    {
        if (!IsShooting && bulletsAccumulator > 0) return true;
        return false;
    }

    private void SubdivideBullets()
    {
        if (bulletsAccumulator > CurrentEmittersAmountPerWave)
        {
            if (bulletsAccumulator > currentMaxProjectileCharge)
            {
                bulletsAccumulator = currentMaxProjectileCharge;
            }
        }
        //else
        //{
        //    DamageMultiplier = 1;
        //}
        singleWaveProjectiles = (int)(bulletsAccumulator % CurrentEmittersAmountPerWave);
        multiWavesRepeat = (int)(bulletsAccumulator / CurrentEmittersAmountPerWave);
        bulletsAccumulator = 0;
    }

    private void CalculateShootingRate()
    {
        shootingRateCooldown = (0.5f * CurrentEmittersAmountPerWave) / maxEmittersAmountPerWave;
    }

    private void CheckIfAccumulatorOverloaded()
    {
        if (bulletsAccumulator < 0)
        {
            bulletsAccumulator = long.MaxValue;
            shootingRateCooldown = 0.01f;

            isOverloaded = true;
            ReleaseAttack();
            isOverloaded = false;
        }
    }

    private void ReleaseAttack()
    {
        IsShooting = true;
        CalculateDamageMultiplier();
        SubdivideBullets();
        HandlesProjectileWavesRelease();
    }

    private void SetCurrentMaxProjectileCharge()
    {
        if (isOverloaded)
            currentMaxProjectileCharge = maxOverloadProjectileCharge;
        else
            currentMaxProjectileCharge = maxStandardProjectileCharge;
    }

    private void CalculateDamageMultiplier()
    {
        if (bulletsAccumulator > currentMaxProjectileCharge)
        {
            DamageMultiplier = 2;
            DamageMultiplier += Mathf.Sqrt(Mathf.Log10(bulletsAccumulator / currentMaxProjectileCharge)) / 2;
        }
    }
    #endregion

}
