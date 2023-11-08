using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileEmitterBehaviour : MonoBehaviour
{
    private PowerInputActions playerInput;
    private InputAction playerAttack;

    #region Unity functions
    private void Start()
    {
        playerInput = GetComponentInParent<PlayerMovementBehaviour>().PlayerInput;
        playerAttack = playerInput.Player.Attack;
        playerAttack.Enable();
        playerAttack.performed += OnAttack;

    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        playerAttack.Disable();
    }


    #endregion

    #region PlayerInput subscribed actions
    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack released!");
    }
    #endregion

}
