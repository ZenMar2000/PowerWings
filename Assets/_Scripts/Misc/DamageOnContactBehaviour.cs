using ND_VariaBULLET;
using UnityEngine;

public class DamageOnContactBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerProjectileEmitterBehaviour playerEmitter;

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Enemy")
        {
            playerEmitter.BulletsAccumulator = (long)(playerEmitter.BulletsAccumulator * 0.125f);
            collision.gameObject.GetComponent<ShotCollisionDamage>().HP = -1;
        }
    }
}
