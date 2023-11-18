using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldChargeBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fullChargedBar;
    [SerializeField] private SpriteRenderer overChargedBar;
    [SerializeField] private PlayerShieldBehaviour playerShieldBehaviour;

    private Vector3 originalScale;
    private float oldShieldCharge = 0;

    private void Awake()
    {
        originalScale = fullChargedBar.transform.localScale;
        oldShieldCharge = playerShieldBehaviour.ShieldCharge;
    }
    private void Update()
    {
        SetOverchargeBarVisibility();
        SetChargedBarScales();
    }

    private void SetOverchargeBarVisibility()
    {
        if (!playerShieldBehaviour.ShieldDepleted && overChargedBar.enabled == true)
        {
            overChargedBar.enabled = false;
            return;
        }
        if (playerShieldBehaviour.ShieldDepleted && overChargedBar.enabled == false)
        {
            overChargedBar.enabled = true;
            return;

        }
    }

    private void SetChargedBarScales()
    {
        if (oldShieldCharge != playerShieldBehaviour.ShieldCharge)
        {
            fullChargedBar.transform.localScale = new Vector3(originalScale.x * playerShieldBehaviour.ShieldCharge, fullChargedBar.transform.localScale.y, fullChargedBar.transform.localScale.z);
            overChargedBar.transform.localScale = fullChargedBar.transform.localScale;
            oldShieldCharge = playerShieldBehaviour.ShieldCharge;
        }
    }
}
