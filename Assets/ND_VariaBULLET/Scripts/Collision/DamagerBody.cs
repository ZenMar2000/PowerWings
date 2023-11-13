#region Script Synopsis
    //A special case script for applying damage from one body to another during collision.
    //Attach to body of player or enemy in order to transmit collision damage information during collision.
    //Learn more about the collision system at: https://neondagger.com/variabullet2d-system-guide/#collision-system
#endregion

using UnityEngine;

namespace ND_VariaBULLET
{
    public class DamagerBody : MonoBehaviour, IDamager
    {
        [Tooltip("Sets the amount of HP reduction produced by this object when collides with a ShotCollidable object.")]
        public float DamagePerHit;

        [Space(10)]

        [Tooltip("Set if the script is attached to a player. Handles bonus damage logic.")]
        public bool IsPlayer = false;

        private PlayerProjectileEmitterBehaviour playerProjectileEmitter;

        private void Start()
        {
            if (IsPlayer)
            {
                playerProjectileEmitter = GetComponentInChildren<PlayerProjectileEmitterBehaviour>();
            }
        }

        public float DMG { 
            get 
            {
                if (IsPlayer)
                {
                    return playerProjectileEmitter.DamageMultiplier * DamagePerHit;
                }
                else 
                { 
                    return DamagePerHit; 
                }
            }
        }
    }
}