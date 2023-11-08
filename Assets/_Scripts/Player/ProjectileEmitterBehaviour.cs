using ND_VariaBULLET;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileEmitterBehaviour : MonoBehaviour
{
    public bool ShotNow = false;
    public int shotShot = 0;

    private PowerInputActions playerInput;
    private InputAction playerAttack;

    private SpreadPattern spreadController;
    private float timer = 0; //temp
    private float shotTimer = 0; //temp

    private int multiWavesRepeat = 0;
    private int singleWaveProjectiles = 0;
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
        if(shotShot == spreadController.EmitterAmount)
        {
            ShotNow = false;
            shotShot = 0;
        }
     
        //{
        //    spreadController.TriggerAutoFire = !spreadController.TriggerAutoFire;
        //    shotTimer = 0;
        //}

        //timer += Time.deltaTime;
        //if (timer > 1)
        //{

        //    if (spreadController.EmitterAmount > 17)
        //    {
        //        multiWavesRepeat++;
        //        singleWaveProjectiles = 1;
        //    }
        //    else
        //    {
        //        singleWaveProjectiles++;
        //    }
        //    timer = 0;

        //}
    }

    private void OnDisable()
    {
        playerAttack.Disable();
    }


    #endregion

    #region PlayerInput subscribed actions
    private void OnAttack(InputAction.CallbackContext context)
    {
        ShotNow = true;
    }
    #endregion

}
