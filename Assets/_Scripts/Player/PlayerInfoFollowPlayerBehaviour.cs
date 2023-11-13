using UnityEngine;
using TMPro;
using System;

public class PlayerInfoFollowPlayerBehaviour : MonoBehaviour
{

    [SerializeField] PlayerProjectileEmitterBehaviour playerProjectileEmitterBehaviour;
    [Space(10)]
    [SerializeField] private float lerpDuration = 3;
    [SerializeField] private Transform followTarget;
    [SerializeField] private TMP_Text StoredBulletsText;
    [SerializeField] private TMP_Text CurrentDamageMultiplierText;


    private float t => Time.deltaTime / lerpDuration;

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, followTarget.position,t);
        StoredBulletsText.text = (Convert.ToString(playerProjectileEmitterBehaviour.BulletsAccumulator));
        CurrentDamageMultiplierText.text = ("x" +(Convert.ToString(Math.Round(playerProjectileEmitterBehaviour.DamageMultiplier, 2))));
    }
}
