using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static SharedMethods;

public class PlayerShieldBehaviour : MonoBehaviour
{
    //private Animator shieldAnimator;
    private SpriteRenderer shieldSpriteRenderer;
    private CapsuleCollider2D shieldCollider;

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
            shieldSpriteRenderer.enabled = value;
            shieldCollider.enabled = value;
            //SetAnimatorValue(ref shieldAnimator, AnimatorStrings.IsParrying, value);
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

    #endregion

    #region PlayerInput subscribed actions
    private void OnParryStarted(InputAction.CallbackContext context)
    {
        IsParrying = true;
    }

    private void OnParryEnded(InputAction.CallbackContext context)
    {
        IsParrying = false;
    }

    #endregion

}
