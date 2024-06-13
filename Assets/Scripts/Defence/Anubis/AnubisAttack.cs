using UnityEngine;

namespace Defence.Anubis
{
    public class AnubisAttack : DefenceItemAttack
    {
        [SerializeField] private ParticleSystem attackFxPrefab;
        protected override void AttackEnemies()
        {
            StartCoroutine(nameof(AttackCooldownRoutine));

            foreach (var slot in affectedBoardSlots)
            {
                var slotPosition = slot.transform.position;
                var instantiatedFx = Instantiate(attackFxPrefab, slotPosition + Vector3.up * 0.25f, Quaternion.identity);
                instantiatedFx.Play();
                Destroy(instantiatedFx.gameObject,1f);
            }
            
            StartCoroutine(GiveDamage());
        }

        protected override void AdjustAttackVFX()
        {
        }
    }
}