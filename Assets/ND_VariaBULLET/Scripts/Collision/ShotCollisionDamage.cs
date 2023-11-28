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
        private PlayerProjectileEmitterBehaviour playerProjectileEmitterBehaviour;

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

        void Start()
        {
            if (GameInfo.PlayerEmitterBehaviour != null)
            {
                playerProjectileEmitterBehaviour = GameInfo.PlayerEmitterBehaviour;
            }
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

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionFilter.collisionAccepted(collision.gameObject.layer, CollisionList) && !CalcObject.IsOutBounds(collision.transform.position))
            {
                if (collision.gameObject.GetComponent<DamagerBody>() != null)
                {
                    DamagerBody body = collision.gameObject.GetComponent<DamagerBody>();
                    if (!IsPlayer)
                    {
                        setDamage(HP + 1);
                    }
                    else
                    {
                        setDamage(body.DamagePerHit, true);
                    }
                }
                else
                {
                    if (collision.gameObject.GetComponent<IDamager>() != null)
                    {
                        setDamage(collision.gameObject.GetComponent<IDamager>().DMG);
                        CollisionFilter.setExplosion(BulletExplosion, ParentExplosion, this.transform, collision.transform.position, 0, this);
                        Destroy(collision.gameObject);

                    }
                }
            }
        }

        protected void setDamage(float damage, bool isContactWithEnemy = false)
        {
            if (IsPlayer) SetPlayerDamage(isContactWithEnemy);
            else SetEnemyDamage(damage * playerProjectileEmitterBehaviour.DamageMultiplier);

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
        private void SetPlayerDamage(bool isContactWithEnemy)
        {

            if (playerProjectileEmitterBehaviour.BulletsAccumulator > 0)
            {
                float reductionPercent;

                if (playerProjectileEmitterBehaviour.IsShooting)
                {
                    if (!isContactWithEnemy)
                    {
                        reductionPercent = 0.75f;
                    }
                    else
                    {
                        reductionPercent = 0.5f;
                    }

                    playerProjectileEmitterBehaviour.MultiWavesRepeat = (int)(playerProjectileEmitterBehaviour.MultiWavesRepeat * reductionPercent);
                    playerProjectileEmitterBehaviour.SingleWaveProjectiles = (int)(playerProjectileEmitterBehaviour.SingleWaveProjectiles * reductionPercent);
                    playerProjectileEmitterBehaviour.BulletsAccumulator = (long)(playerProjectileEmitterBehaviour.BulletsAccumulator * reductionPercent);

                }
                else
                {
                    if (!isContactWithEnemy)
                    {
                        reductionPercent = 0.25f;
                    }
                    else
                    {
                        reductionPercent = 0.125f;
                    }

                    playerProjectileEmitterBehaviour.BulletsAccumulator = (long)(playerProjectileEmitterBehaviour.BulletsAccumulator * reductionPercent);

                    if (playerProjectileEmitterBehaviour.BulletsAccumulator >= GameInfo.WarningValue || playerProjectileEmitterBehaviour.BulletsAccumulator <= playerProjectileEmitterBehaviour.maxStandardProjectileCharge)
                    {
                        playerProjectileEmitterBehaviour.CalculateDamageMultiplier();
                    }
                }
            }
            else
            {
                HP = -1;
            }
        }

        private void SetEnemyDamage(float damage)
        {
            GameInfo.AddScore(0.75f + playerProjectileEmitterBehaviour.DamageMultiplier * 0.25f);
            HP -= damage;
        }

        public void CheckIfDefeated()
        {
            if (HP <= 0)
            {
                if (DeathExplosion != "")
                {
                    string explosion = DeathExplosion;
                    GameObject finalExplode = GlobalShotManager.Instance.ExplosionRequest(explosion, this);

                    finalExplode.transform.position = this.transform.position;
                    finalExplode.transform.parent = null;
                    finalExplode.transform.localScale = new Vector2(finalExplode.transform.localScale.x * FinalExplodeFactor, finalExplode.transform.localScale.y * FinalExplodeFactor);
                }

                if (IsPlayer) Destroy(transform.parent.parent.gameObject);
                else Destroy(transform.parent.gameObject);


            }
        }
    }

}