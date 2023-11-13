#region Script Synopsis
//A monobehavior that is attached to any object that receives collisions from bullet/laser shots and instantiates explosions if set and applies damage to the object.
//Examples: Any object that receives damage (player, enemy, etc).
//Learn more about the collision system at: https://neondagger.com/variabullet2d-system-guide/#collision-system
#endregion

using System.Collections;
using UnityEngine;

namespace ND_VariaBULLET
{
    public class ShotCollisionDamage : ShotCollision, IShotCollidable
    {
        [Tooltip("If it's player, change logic for HP")]
        public bool IsPlayer;

        [Tooltip("Sets the name of the explosion prefab to be instantiated when HP = 0.")]
        public string DeathExplosion;

        [Tooltip("Health Points. Reduces according to incoming IDamager.DMG value upon collision.")]
        public float HP = 10;

        [Range(0.1f, 8f)]
        [Tooltip("Changes the size of the last explosion (when HP = 0).")]
        public float FinalExplodeFactor = 2;

        [Tooltip("Enables indicating damage by flickering color (via DamageColor setting) when HP is reducing.")]
        public bool DamageFlicker;

        [Range(5, 40)]
        [Tooltip("Sets the duration frames for the DamageFlicker effect upon collision.")]
        public int FlickerDuration = 6;

        [Tooltip("Sets the color the object flickers to when HP is reducing and DamageFlicker is enabled.")]
        public Color DamageColor;
        private Color NormalColor;
        [SerializeField] private SpriteRenderer rend;

        [SerializeField] private PlayerProjectileEmitterBehaviour playerProjectileEmitterBehaviour;

        void Start()
        {
            //rend = GetComponent<SpriteRenderer>();
            NormalColor = rend.color;
        }

        public new IEnumerator OnLaserCollision(CollisionArgs sender)
        {
            if (CollisionFilter.collisionAccepted(sender.gameObject.layer, CollisionList) && !CalcObject.IsOutBounds(sender.point))
            {
                setDamage(sender.damage);
                CollisionFilter.setExplosion(LaserExplosion, ParentExplosion, this.transform, new Vector2(sender.point.x, sender.point.y), 0, this);
                yield return setFlicker();
            }
        }

        public new IEnumerator OnCollisionEnter2D(Collision2D collision)
        {
            if (CollisionFilter.collisionAccepted(collision.gameObject.layer, CollisionList) && !CalcObject.IsOutBounds(collision.contacts[0].point))
            {
                setDamage(collision.gameObject.GetComponent<IDamager>().DMG);
                CollisionFilter.setExplosion(BulletExplosion, ParentExplosion, this.transform, collision.contacts[0].point, 0, this);
                yield return setFlicker();
            }
        }

        protected void setDamage(float damage)
        {
            if (IsPlayer) SetPlayerDamage();
            else SetEnemyDamage(damage);

            CheckIfDefeated();
        }

        protected IEnumerator setFlicker()
        {
            if (rend == null)
            {
                Utilities.Warn("No SpriteRenderer attached. Cannot flicker during damage.", this);
                yield return null;
            }

            if (DamageFlicker)
            {
                bool flicker = false;
                for (int i = 0; i < FlickerDuration * 2; i++)
                {
                    flicker = !flicker;

                    if (flicker)
                        rend.color = DamageColor;
                    else
                        rend.color = NormalColor;

                    yield return null;
                };

                rend.color = NormalColor;
            }
        }
        private void SetPlayerDamage()
        {
           
            if (playerProjectileEmitterBehaviour.BulletsAccumulator > 0)
            {
                playerProjectileEmitterBehaviour.BulletsAccumulator = (long)(playerProjectileEmitterBehaviour.BulletsAccumulator * 0.25);

                if (playerProjectileEmitterBehaviour.IsShooting)
                {
                    playerProjectileEmitterBehaviour.MultiWavesRepeat = (int)(playerProjectileEmitterBehaviour.MultiWavesRepeat * 0.25);
                    playerProjectileEmitterBehaviour.SingleWaveProjectiles = (int)(playerProjectileEmitterBehaviour.SingleWaveProjectiles * 0.25);
                }

                playerProjectileEmitterBehaviour.CalculateDamageMultiplier();

            }
            else
            {
                HP = -1;
            }
        }

        private void SetEnemyDamage(float damage)
        {
            HP -= damage;
        }

        private void CheckIfDefeated()
        {
            if (HP < 0)
            {
                if (DeathExplosion != "")
                {
                    string explosion = DeathExplosion;
                    GameObject finalExplode = GlobalShotManager.Instance.ExplosionRequest(explosion, this);

                    finalExplode.transform.position = this.transform.position;
                    finalExplode.transform.parent = null;
                    finalExplode.transform.localScale = new Vector2(finalExplode.transform.localScale.x * FinalExplodeFactor, finalExplode.transform.localScale.y * FinalExplodeFactor);
                }

                Destroy(transform.parent.gameObject);
            }
        }
    }

}