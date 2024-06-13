using Board;
using DG.Tweening;
using Particles;
using UnityEngine;
using Zenject;

namespace Defence.Anubis
{
    public class AnubisAttack : DefenceItemAttack
    {
        private ParticlePool _particlePool;

        [Inject]
        private void Construct(ParticlePool particlePool)
        {
            _particlePool = particlePool;
        }
        
        protected override void AttackEnemies()
        {
            StartCoroutine(nameof(AttackCooldownRoutine));

            foreach (var slot in affectedBoardSlots)
            {
                var slotPosition = slot.transform.position;
                var instantiatedFx = _particlePool.GetAttackVFX(DefenceItemType.Anubis);
                instantiatedFx.transform.position = slotPosition + Vector3.up * 0.25f;
                instantiatedFx.Play();
                
                DOVirtual.DelayedCall(1f, () =>
                {
                    _particlePool.ReleaseAttackVFX(DefenceItemType.Anubis, instantiatedFx);
                });
            }
            
            StartCoroutine(GiveDamage());
        }

        protected override void AdjustAttackVFX()
        {
        }
    }
}