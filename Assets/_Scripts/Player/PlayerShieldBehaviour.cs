using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static SharedLogics;

public class PlayerShieldBehaviour : MonoBehaviour
{
    public bool CanParry = true;
    //private Animator shieldAnimator;
    private SpriteRenderer shieldSpriteRenderer;
    private CapsuleCollider2D shieldCollider;
    [SerializeField] private PlayerProjectileEmitterBehaviour playerProjectileEmitter;

    #region Properties
    private bool _isParrying = false;
    public bool IsParrying 
    { 
        get 
        {
            return _isParrying;
        }
        private set 
        {
            _isParrying = value;
            if(shieldSpriteRenderer != null)
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
        shieldCollider = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        InputManager.PlayerParry.started += OnParryStarted;
        InputManager.PlayerParry.canceled += OnParryEnded;
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
        if (CanParry)
        {
            IsParrying = true;
            shieldCollider.enabled = true;
        }
    }

    private void OnParryEnded(InputAction.CallbackContext context)
    {
        DisableParry();
    }

    public void DisableParry()
    {
        shieldCollider.enabled = false;
        IsParrying = false;
    }
    #endregion

}
