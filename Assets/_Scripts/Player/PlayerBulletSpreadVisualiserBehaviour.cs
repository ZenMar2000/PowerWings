using ND_VariaBULLET;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBulletSpreadVisualiserBehaviour : MonoBehaviour
{
    #region Variables
    //Guides prefab
    public GameObject LeftGuide;
    private SpriteRenderer LeftRenderer;
    public GameObject RightGuide;
    private SpriteRenderer RightRenderer;

    public bool AlwaysVisibleSpreadGuides = true;
    public SpreadPattern spreadController;

    private PlayerProjectileEmitterBehaviour playerProjectileEmitterBehaviour;
    private float projectileSpreadAngle;

    //Guides movement variables
    private float autoRepeatRate = 0.01f;
    private float autoRepeatTimer = 0;

    //Guides visibility variables
    private float hideGuidesTimer = 0;
    private float hideGuidesRate = 1f;
    private bool guidesAlreadyHidden = false;

    /// <summary>
    /// Value read from playerInput action performed
    /// </summary>
    private float inputValue = 0;
    #endregion

    #region Unity functions
    private void Awake()
    {
        LeftRenderer = LeftGuide.GetComponentInChildren<SpriteRenderer>();
        RightRenderer = RightGuide.GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        //playerInput = GetComponentInParent<PlayerMovementBehaviour>().PlayerInput;
        playerProjectileEmitterBehaviour = GetComponentInParent<PlayerProjectileEmitterBehaviour>();
        projectileSpreadAngle = spreadController.SpreadDegrees / 2;

        LeftGuide.transform.Rotate(0, 0, projectileSpreadAngle * (playerProjectileEmitterBehaviour.CurrentEmittersAmountPerWave - 1) * -1);
        RightGuide.transform.Rotate(0, 0, projectileSpreadAngle * (playerProjectileEmitterBehaviour.CurrentEmittersAmountPerWave - 1));

        InputManager.PlayerChangeSpreadAngle.started += OnChangeSpreadAngleStart;
        InputManager.PlayerChangeSpreadAngle.canceled += OnChangeSpreadAngleEnd;
    }

    void Update()
    {
        SyncSpreadAngle();
        handleTimers();
        ChangeSpreadAngle();
    }

    #endregion

    #region PlayerInput subscribed actions
    private void OnChangeSpreadAngleStart(InputAction.CallbackContext context)
    {
        inputValue = context.ReadValue<float>();
    }

    private void OnChangeSpreadAngleEnd(InputAction.CallbackContext context)
    {
        inputValue = 0;
    }

    #endregion

    #region Utility private functions
    private void ChangeSpreadAngle()
    {
        if (autoRepeatTimer > autoRepeatRate && inputValue != 0)
        {
            ShowOrHideHideGuides(true);

            autoRepeatTimer = 0;
            hideGuidesTimer = 0;

            if (!playerProjectileEmitterBehaviour.IsShooting)
            {

                if (inputValue > 0 && (playerProjectileEmitterBehaviour.CurrentEmittersAmountPerWave < playerProjectileEmitterBehaviour.maxEmittersAmountPerWave))
                {
                    LeftGuide.transform.Rotate(0, 0, projectileSpreadAngle * -1);
                    RightGuide.transform.Rotate(0, 0, projectileSpreadAngle);
                    playerProjectileEmitterBehaviour.CurrentEmittersAmountPerWave++;
                }
                else if (inputValue < 0 && (playerProjectileEmitterBehaviour.CurrentEmittersAmountPerWave-1 > 0))
                {
                    LeftGuide.transform.Rotate(0, 0, projectileSpreadAngle);
                    RightGuide.transform.Rotate(0, 0, projectileSpreadAngle * -1);
                    playerProjectileEmitterBehaviour.CurrentEmittersAmountPerWave--;
                }
            }
        }
    }

    private void SyncSpreadAngle()
    {
        if (playerProjectileEmitterBehaviour.spreadController != null && projectileSpreadAngle != playerProjectileEmitterBehaviour.spreadController.SpreadDegrees)
        {
            projectileSpreadAngle = playerProjectileEmitterBehaviour.spreadController.SpreadDegrees / 2;
        }
    }

    private void handleTimers()
    {
        if (autoRepeatTimer < autoRepeatRate) autoRepeatTimer += Time.deltaTime;

        if (hideGuidesTimer < hideGuidesRate)
        {
            hideGuidesTimer += Time.deltaTime;
        }
        else if (!guidesAlreadyHidden)
        {
            ShowOrHideHideGuides(false);
        }
    }

    private void ShowOrHideHideGuides(bool isShown)
    {
        if (!AlwaysVisibleSpreadGuides)
        {
            LeftRenderer.enabled = isShown;
            RightRenderer.enabled = isShown;
            guidesAlreadyHidden = !isShown;
        }
    }

    #endregion
}
