using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static SharedMethods;

public class PlayerShieldBehaviour : MonoBehaviour
{
    private PowerInputActions playerInput;
    private InputAction playerParry;
    private Animator shieldAnimator;

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
            SetAnimatorValue(ref shieldAnimator, AnimatorStrings.IsParrying, value);
        }
    }

    #endregion

    #region Unity functions
    private void Awake()
    {
        shieldAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        playerInput = GetComponentInParent<PlayerMovementBehaviour>().PlayerInput;
        playerParry = playerInput.Player.Parry;
        playerParry.Enable();
        playerParry.started += OnParryStarted;
        playerParry.canceled += OnParryEnded;
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        playerParry.Disable();
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