using Board;
using DG.Tweening;
using Particles;
using Zenject;

namespace Defence.Bastet
{
    public class BastetAttack : DefenceItemAttack
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
                var instantiatedFx = _particlePool.GetAttackVFX(DefenceItemType.Bastet);
                instantiatedFx.transform.position = slotPosition;
                instantiatedFx.Play();
                DOVirtual.DelayedCall(1f, () =>
                {
                    _particlePool.ReleaseAttackVFX(DefenceItemType.Bastet, instantiatedFx);
                });
            }
            
            StartCoroutine(GiveDamage());
        }

        protected override void AdjustAttackVFX()
        {
            return;
        }
    }
}
