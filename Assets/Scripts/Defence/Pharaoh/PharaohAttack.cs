using System.Collections.Generic;
using DG.Tweening;
using Enemies;
using UnityEngine;

namespace Defence.Pharaoh
{
    public class PharaohAttack : DefenceItemAttack
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private float _particleScaleFactor;
        
        protected override void AttackEnemies()
        {
            StartCoroutine(nameof(AttackCooldownRoutine));

            _particleSystem.Play();

            var enemiesToDamage = new List<Enemy>(_enemiesInRange);
         
            foreach (var enemy in enemiesToDamage)
            {
                enemy.TakeDamage(_itemData.damage);
            }
        }

        protected override void AdjustAttackVFX()
        {
            _particleSystem.transform.localScale = Vector3.one * (1 + _particleScaleFactor * (_itemData.range - 1));
        }
    }
}