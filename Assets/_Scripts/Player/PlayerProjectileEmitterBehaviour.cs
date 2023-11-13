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
    
    [SerializeField] private float shootingRateCooldown = 0.01f;
    /// <summary>
    /// Timer used toghether with shootingRateCooldown for repeating waves
    /// </summary>
    private float shootingtTimer = 0;

    /// <summary>
    /// Store all bullets accumulated. Will be subdivided in MultiWavesRepeat and SingleWaveProjectiles as soon as the player perform an attack.
    /// </summary>
    public long BulletsAccumulator;

    /// <summary>
    /// Store how many waves with the full CurrentEmittersAmountPerWaves must be performed.
    /// </summary>
    public int MultiWavesRepeat = 0;

    /// <summary>
    /// Used for perform a single wave after all MultiWavesRepeat reached 0. Shoots remaining bullets.
    /// </summary>
    public int SingleWaveProjectiles = 0;

    private int maxStandardProjectileCharge = 512;
    private int maxOverloadProjectileCharge = 1024;
    private int currentMaxProjectileCharge;

    [SerializeField] private PlayerShieldBehaviour playerShieldBehaviour;

    #region Properties
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
            if (!value)
            {
                BulletsAccumulator = 0;
            }
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
        BulletsAccumulator = 8;
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
        }
    }

    #endregion

    #region PlayerInput subscribed actions
    private void OnAttack(InputAction.CallbackContext context)
    {
        if (CanShoot())
        {
            playerShieldBehaviour.DisableParry();
            playerShieldBehaviour.CanParry = false;
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

            if (MultiWavesRepeat > 0)
            {
                spreadController.SetAndCloneEmitters(CurrentEmittersAmountPerWave);
                MultiWavesRepeat--;
                ShotNow = true;
            }
            else if (SingleWaveProjectiles > 0)
            {
                spreadController.SetAndCloneEmitters(SingleWaveProjectiles);
                SingleWaveProjectiles = 0;
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
        if (MultiWavesRepeat <= 0 && SingleWaveProjectiles <= 0 && IsShooting)
        {
            IsShooting = false;
            DamageMultiplier = 1;
            playerShieldBehaviour.CanParry = true;
        }
    }

    public bool CanShoot()
    {
        if (!IsShooting && BulletsAccumulator > 0) return true;
        return false;
    }

    private void SubdivideBullets()
    {
        if (BulletsAccumulator > CurrentEmittersAmountPerWave)
        {
            if (BulletsAccumulator > currentMaxProjectileCharge)
            {
                BulletsAccumulator = currentMaxProjectileCharge;
            }
        }
       
        SingleWaveProjectiles = (int)(BulletsAccumulator % CurrentEmittersAmountPerWave);
        MultiWavesRepeat = (int)(BulletsAccumulator / CurrentEmittersAmountPerWave);
        //BulletsAccumulator = 0;
    }

    private void CalculateShootingRate()
    {
        shootingRateCooldown = (0.5f * CurrentEmittersAmountPerWave) / maxEmittersAmountPerWave;
    }

    private void CheckIfAccumulatorOverloaded()
    {
        if (BulletsAccumulator < 0)
        {
            BulletsAccumulator = long.MaxValue;
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

    public void CalculateDamageMultiplier()
    {
        if (BulletsAccumulator > currentMaxProjectileCharge)
        {
            DamageMultiplier = 2;
            DamageMultiplier += Mathf.Sqrt(Mathf.Log10(BulletsAccumulator / currentMaxProjectileCharge)) / 2;
        }
    }
    #endregion

}
