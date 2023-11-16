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
    private AudioSource shotSound;
    private float SoundTimer = 1;

    [SerializeField] private float shootingRateCooldown = 0.01f;
    /// <summary>
    /// Timer used toghether with shootingRateCooldown for repeating waves
    /// </summary>
    private float shootingtTimer = 0;


    /// <summary>
    /// Store how many waves with the full CurrentEmittersAmountPerWaves must be performed.
    /// </summary>
    public int MultiWavesRepeat = 0;

    /// <summary>
    /// Used for perform a single wave after all MultiWavesRepeat reached 0. Shoots remaining bullets.
    /// </summary>
    public int SingleWaveProjectiles = 0;

    private int maxStandardProjectileCharge = 256;
    private int maxOverloadProjectileCharge = 512;


    private long subtractiveBulletsAccumulator = 0;
    [SerializeField] private PlayerShieldBehaviour playerShieldBehaviour;

    #region Properties
    /// <summary>
    /// Maximum amount of projectiles actually shot. Excess bullets are converted in damage multiplier bonus.
    /// </summary>
    public int CurrentMaxProjectileCharge { get; private set; }

    /// <summary>
    /// Store all bullets accumulated. Will be subdivided in MultiWavesRepeat and SingleWaveProjectiles as soon as the player perform an attack.
    /// </summary>
    [SerializeField] private long _bulletsAccumulator;
    public long BulletsAccumulator
    {
        get
        {
            return _bulletsAccumulator;
        }
        set
        {
            _bulletsAccumulator = value;

        }
    }

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
                SoundTimer = 1;
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
        shotSound = GetComponent<AudioSource>();
        CurrentEmittersAmountPerWave = 15;
        BulletsAccumulator = 8;
        CurrentMaxProjectileCharge = maxStandardProjectileCharge;
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
            SoundTimer += Time.deltaTime;
            shootingtTimer += Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        InputManager.PlayerAttack.started -= OnAttack;
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
            HandleShotSound();
            UpdateBulletAccumulator();

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
    private void HandleShotSound()
    {
        if (SoundTimer > 0.07)
        {
            shotSound.Play();
            SoundTimer = 0;
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
        long tempBullet = BulletsAccumulator;
        if (tempBullet > CurrentEmittersAmountPerWave)
        {
            if (tempBullet > CurrentMaxProjectileCharge)
            {
                tempBullet = CurrentMaxProjectileCharge;
            }
        }

        SingleWaveProjectiles = (int)(tempBullet % CurrentEmittersAmountPerWave);
        MultiWavesRepeat = (int)(tempBullet / CurrentEmittersAmountPerWave);
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
            DamageMultiplier = 3;
            isOverloaded = true;
            ReleaseAttack();
            isOverloaded = false;
        }
    }
     
    private void ReleaseAttack()
    {
        IsShooting = true;
        playerShieldBehaviour.DisableParry();
        playerShieldBehaviour.CanParry = false;

        CalculateDamageMultiplier();
        SubdivideBullets();

        CalculateBulletsSubtractionValue();
        HandlesProjectileWavesRelease();

    }

    private void SetCurrentMaxProjectileCharge()
    {
        if (isOverloaded)
            CurrentMaxProjectileCharge = maxOverloadProjectileCharge;
        else
            CurrentMaxProjectileCharge = maxStandardProjectileCharge;
    }

    public void CalculateDamageMultiplier()
    {
        if (BulletsAccumulator > CurrentMaxProjectileCharge)
        {
            DamageMultiplier += DamageMultiplier * 0.02f;
        }
        else
        {
            DamageMultiplier = 1;
        }
    }
   
    private void UpdateBulletAccumulator()
    {
        if ((BulletsAccumulator - subtractiveBulletsAccumulator) < 0)
            BulletsAccumulator = 0;
        else
            BulletsAccumulator -= subtractiveBulletsAccumulator;
    }

    private void CalculateBulletsSubtractionValue()
    {
        subtractiveBulletsAccumulator = (long)Mathf.Ceil(BulletsAccumulator / (CurrentEmittersAmountPerWave == 1 ? MultiWavesRepeat : MultiWavesRepeat + 1));

        if (subtractiveBulletsAccumulator == 0)
            subtractiveBulletsAccumulator = 1;
    }
    #endregion

}
