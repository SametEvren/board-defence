using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Defence.Bastet
{
    public class BastetAttack : DefenceItemAttack
    {
        [SerializeField] private ParticleSystem attackFxPrefab;
        protected override void AttackEnemies()
        {
            StartCoroutine(nameof(AttackCooldownRoutine));

            foreach (var slot in affectedBoardSlots)
            {
                var transformOfSlot = slot.transform.position;
                var instantiatedFx = Instantiate(attackFxPrefab, transformOfSlot, Quaternion.identity);
                instantiatedFx.Play();
                Destroy(instantiatedFx.gameObject,1f);
            }
            
            StartCoroutine(GiveDamage());
        }

        protected override void AdjustAttackVFX()
        {
            return;
        }
    }
}
