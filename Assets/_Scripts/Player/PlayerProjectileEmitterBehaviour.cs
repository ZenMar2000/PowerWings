using ND_VariaBULLET;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProjectileEmitterBehaviour : MonoBehaviour
{
    public bool ShotNow = false;
    public int ShotsEmitted = 0;

    private bool IsShooting = false;
    private PowerInputActions playerInput;
    private InputAction playerAttack;

    private SpreadPattern spreadController;
    private float tempTimer = 0; //temp


    private int maxEmittersAmountPerWave = 17;
    private float shotTimer = 0;

    #region Properties
    [SerializeField] private int multiWavesRepeat = 0;
    [SerializeField] private int _singleWaveProjectiles = 0;
    private int singleWaveProjectiles
    {
        get
        {
            return _singleWaveProjectiles;
        }
        set
        {
            if (value > maxEmittersAmountPerWave)
            {
                _singleWaveProjectiles = 1;
                multiWavesRepeat++;
            }
            else
            {
                _singleWaveProjectiles = value;
            }
        }
    }
    #endregion

    #region Unity functions
    private void Start()
    {
        spreadController = GetComponentInChildren<SpreadPattern>();
        //spreadController.EmitterAmount = 0;
        //spreadController.SpreadDegrees = 10;

        playerInput = GetComponentInParent<PlayerMovementBehaviour>().PlayerInput;
        playerAttack = playerInput.Player.Attack;
        playerAttack.Enable();
        playerAttack.started += OnAttack;

    }

    private void Update()
    {
        CheckEmittedShots();
        CheckShootingStatus();

        if(IsShooting)
        {
            shotTimer += Time.deltaTime;
            tempTimer = 0;
        }
        else
        {
            tempTimer += Time.deltaTime;
            if(tempTimer > 0.5)
            {
                tempTimer = 0;
                singleWaveProjectiles++;
            }
        }
    }

    private void OnDisable()
    {
        playerAttack.Disable();
    }


    #endregion

    #region PlayerInput subscribed actions
    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!IsShooting)
        {
            IsShooting = true;
            HandlesProjectileWavesRelease();
        }
    }
    #endregion

    #region Utility private functions

    private void HandlesProjectileWavesRelease()
    {
        if (!ShotNow)
        {
            shotTimer = 0;

            if (multiWavesRepeat > 0)
            {
                spreadController.SetAndCloneEmitters(maxEmittersAmountPerWave);
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

        if (shotTimer > 1)
        {
            HandlesProjectileWavesRelease();
        }

    }

    private void CheckShootingStatus()
    {
        if (multiWavesRepeat <= 0 && singleWaveProjectiles <= 0)
        {
            IsShooting = false;
        }
    }
    #endregion

}
