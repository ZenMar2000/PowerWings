﻿#region Script Synopsis
//A monobehavior that is attached to any object that receives collisions from bullet/laser shots and instantiates explosions if set and applies damage to the object.
//Examples: Any object that receives damage (player, enemy, etc).
//Learn more about the collision system at: https://neondagger.com/variabullet2d-system-guide/#collision-system
#endregion

using System.Collections;
using UnityEngine;

namespace ND_VariaBULLET
{
    public class ShotCollisionDamageShield : ShotCollision, IShotCollidable
    {
        [Tooltip("Enables indicating damage by flickering color (via DamageColor setting) when HP is reducing.")]
        public bool DamageFlicker;

        [Range(5, 40)]
        [Tooltip("Sets the duration frames for the DamageFlicker effect upon collision.")]
        public int FlickerDuration = 6;

        [Tooltip("Sets the color the object flickers to when HP is reducing and DamageFlicker is enabled.")]
        public Color DamageColor;
        private Color NormalColor;
        [SerializeField] private SpriteRenderer rend;

        [Tooltip("Used for update the BulletAccumulator every time a bullet get parried")]
        private PlayerProjectileEmitterBehaviour PlayerProjectileEmitterBehaviour;

        void Start()
        {
            rend = GetComponent<SpriteRenderer>();
            NormalColor = rend.color;
            PlayerProjectileEmitterBehaviour = GameInfo.EmitterBehaviour;
        }

        public new IEnumerator OnLaserCollision(CollisionArgs sender)
        {
            if (CollisionFilter.collisionAccepted(sender.gameObject.layer, CollisionList) && !CalcObject.IsOutBounds(sender.point))
            {
                AbsorbBullet();
                CollisionFilter.setExplosion(LaserExplosion, ParentExplosion, this.transform, new Vector2(sender.point.x, sender.point.y), 0, this);
                yield return setFlicker();
            }
        }
        public void OnTriggerEnter2D(Collider2D collision)
        {

            if (CollisionFilter.collisionAccepted(collision.gameObject.layer, CollisionList) && !CalcObject.IsOutBounds(collision.transform.position))
            {
                AbsorbBullet();

                CollisionFilter.setExplosion(BulletExplosion, ParentExplosion, this.transform, new Vector2(collision.transform.position.x, collision.transform.position.y - 0.5f), 0, this);
                Destroy(collision.gameObject);
            }
        }
        public new IEnumerator OnCollisionEnter2D(Collision2D collision)
        {
            if (CollisionFilter.collisionAccepted(collision.gameObject.layer, CollisionList) && !CalcObject.IsOutBounds(collision.contacts[0].point))
            {
                AbsorbBullet();
                CollisionFilter.setExplosion(BulletExplosion, ParentExplosion, this.transform, collision.contacts[0].point, 0, this);
                yield return setFlicker();
            }
        }

        protected void AbsorbBullet()
        {
            if (PlayerProjectileEmitterBehaviour.BulletsAccumulator >= 0)
            {
                if (PlayerProjectileEmitterBehaviour.BulletsAccumulator == 0)
                    PlayerProjectileEmitterBehaviour.BulletsAccumulator = 1;
                else
                    PlayerProjectileEmitterBehaviour.BulletsAccumulator *= 2;
            }

            PlayerProjectileEmitterBehaviour.CalculateDamageMultiplier();
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
    }
}