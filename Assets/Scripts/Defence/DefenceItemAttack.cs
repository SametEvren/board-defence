using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Defence
{
    public abstract class DefenceItemAttack : MonoBehaviour
    {
        [SerializeField] protected List<Enemy> _enemiesInRange = new List<Enemy>();
        protected DefenceItemData _itemData;
        protected bool _isInCooldown;

        public void RemoveEnemyFromTargets(Enemy target)
        {
            if (_enemiesInRange.Contains(target))
            {
                _enemiesInRange.Remove(target);
            }
        }

        public void AddEnemyAsTarget(Enemy newTarget)
        {
            if (!_enemiesInRange.Contains(newTarget))
            {
                _enemiesInRange.Add(newTarget);
            }

            if (!_isInCooldown)
                DoNextAttackCycle();
        }

        public void Initialize(DefenceItemData itemData)
        {
            _itemData = itemData;

            AdjustAttackVFX();
            DoNextAttackCycle();
        }

        public void ClearEnemiesInRange()
        {
            _enemiesInRange.Clear(); 
        }

        protected void DoNextAttackCycle()
        {
            if (_enemiesInRange is { Count: > 0 }) AttackEnemies();
        }

        protected IEnumerator AttackCooldownRoutine()
        {
            _isInCooldown = true;
            yield return new WaitForSeconds(_itemData.interval);
            DoNextAttackCycle();
            _isInCooldown = false;
        }

        protected abstract void AttackEnemies();
        protected abstract void AdjustAttackVFX();
    }
}