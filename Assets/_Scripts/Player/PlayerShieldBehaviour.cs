using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShieldBehaviour : MonoBehaviour
{
    public bool CanParry = true;

    //private Animator shieldAnimator;
    private SpriteRenderer shieldSpriteRenderer;
    private PolygonCollider2D shieldCollider;
    private bool reenableShieldASAP = false;
    private PlayerProjectileEmitterBehaviour playerProjectileEmitter;

    [SerializeField] private float shieldDischargeValue = 0.03f;
    [SerializeField] private float shieldRechargeValue = 0.01f;
    [SerializeField] private PlayerFullShieldBehaviour fullShieldBehaviour;

    #region Properties
    [SerializeField] private float _shieldCharge = 1f;
    public float ShieldCharge
    {
        get
        {
            return _shieldCharge;
        }
        private set
        {
            _shieldCharge = value;
        }
    }

    [SerializeField] private bool _shieldDepleted = false;
    public bool ShieldDepleted
    {
        get
        {
            return _shieldDepleted;
        }
        private set
        {
            _shieldDepleted = value;
        }
    }

    [SerializeField] private bool _isParrying = false;
    public bool IsParrying
    {
        get
        {
            return _isParrying;
        }
        private set
        {
            _isParrying = value;
            if (shieldSpriteRenderer != null)
            {
                shieldSpriteRenderer.enabled = value;
                shieldCollider.enabled = value;
                //SetAnimatorValue(ref shieldAnimator, AnimatorStrings.IsParrying, value);
            }
        }
    }

    #endregion

    #region Unity functions
    private void Awake()
    {
        //shieldAnimator = GetComponent<Animator>();
        shieldSpriteRenderer = GetComponent<SpriteRenderer>();
        shieldCollider = GetComponent<PolygonCollider2D>();
        playerProjectileEmitter = GameInfo.PlayerEmitterBehaviour;
    }
    private void Start()
    {
        InputManager.PlayerParry.started += OnParryStarted;
        InputManager.PlayerParry.canceled += OnParryEnded;
    }

    private void FixedUpdate()
    {
        if (CheckParryAvailability() && IsParrying)
            ShieldCharge -= shieldDischargeValue;

        if (reenableShieldASAP)
            Parry();

        ShieldRecovery();
    }

    private void OnDestroy()
    {
        InputManager.PlayerParry.started -= OnParryStarted;
        InputManager.PlayerParry.canceled -= OnParryEnded;
    }
    #endregion

    #region PlayerInput subscribed actions
    private void OnParryStarted(InputAction.CallbackContext context)
    {
        reenableShieldASAP = true;
        Parry();
    }

    private void Parry()
    {
        if (CheckParryAvailability() && !fullShieldBehaviour.FullShieldEnabled)
        {
            IsParrying = true;
            shieldCollider.enabled = true;
        }
    }

    private void OnParryEnded(InputAction.CallbackContext context)
    {
        DisableParry();
    }

    public void DisableParry(bool shieldDepleted = false)
    {
        reenableShieldASAP = false;
        if (IsParrying)
        {
            shieldCollider.enabled = false;
            IsParrying = false;
            ShieldDepleted = shieldDepleted;
        }
    }

    public bool CheckParryAvailability()
    {
        if (ShieldCharge <= 0 && ShieldDepleted == false)
        {
            ShieldCharge = 0;
            ShieldDepleted = true;
            DisableParry(true);
        }

        if (ShieldDepleted == true || CanParry == false)
        {
            return false;
        }
        return true;
    }

    private void ShieldRecovery()
    {
        if (!IsParrying)
        {
            if (ShieldCharge < 1f)
            {
                ShieldCharge += shieldRechargeValue;
            }
            else if (ShieldCharge >= 1f && ShieldDepleted == true)
            {
                ShieldDepleted = false;
            }
        }
    }
    #endregion

}
