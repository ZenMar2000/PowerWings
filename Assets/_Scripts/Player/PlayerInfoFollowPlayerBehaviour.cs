using System;
using TMPro;
using UnityEngine;

public class PlayerInfoFollowPlayerBehaviour : MonoBehaviour
{
    public PlayerProjectileEmitterBehaviour playerProjectileEmitterBehaviour;
    [Space(10)]
    [SerializeField] private float lerpDuration = 0.02f;
    [SerializeField] private Transform followTarget;
    [SerializeField] private TMP_Text StoredBulletsText;
    [SerializeField] private TMP_Text CurrentDamageMultiplierText;
    private float t => Time.deltaTime / lerpDuration;

    #region Unity functions
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, followTarget.position, t);
        StoredBulletsText.text = FormatStoredBulletsText();
        CurrentDamageMultiplierText.text = FormatCurrentDamangeMultiplierText();

    }

    #endregion

    #region Utility private functions

    private string FormatStoredBulletsText()
    {
        return Convert.ToString(playerProjectileEmitterBehaviour.BulletsAccumulator);
    }

    private string FormatCurrentDamangeMultiplierText()
    {
        return "x" + (Convert.ToString(Math.Round(playerProjectileEmitterBehaviour.DamageMultiplier, 2)));
    }


    #endregion
}
