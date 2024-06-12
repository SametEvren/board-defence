using System.Collections.Generic;
using Enemies;
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
                var transformOfSlot = slot.transform.position;
                var instantiatedFx = Instantiate(attackFxPrefab, transformOfSlot, Quaternion.identity);
                instantiatedFx.Play();
                Destroy(instantiatedFx.gameObject,1f);
            }
            
            var enemiesToDamage = new List<Enemy>(enemiesInRange);
            foreach (var enemy in enemiesToDamage)
            {
                enemy.TakeDamage(_itemData.damage);
            }
        }

        protected override void AdjustAttackVFX()
        {
            return;
        }
    }
}