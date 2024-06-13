using UnityEngine;

namespace Defence.Pharaoh
{
    public class PharaohAttack : DefenceItemAttack
    {
        [SerializeField] private ParticleSystem attackParticle;
        [SerializeField] private float particleScaleFactor;
        
        protected override void AttackEnemies()
        {
            StartCoroutine(nameof(AttackCooldownRoutine));

            attackParticle.Play();

            StartCoroutine(GiveDamage());
        }
        
        protected override void AdjustAttackVFX()
        {
            attackParticle.transform.localScale = Vector3.one * (1 + particleScaleFactor * (_itemData.range - 1));
        }
    }
}