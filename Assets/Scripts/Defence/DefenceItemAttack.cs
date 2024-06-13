using System.Collections;
using System.Collections.Generic;
using Board;
using Enemies;
using UIScripts;
using UnityEngine;
using Zenject;

namespace Defence
{
    public abstract class DefenceItemAttack : MonoBehaviour
    {
        [SerializeField] protected List<Enemy> enemiesInRange = new ();
        [SerializeField] protected List<BoardSlot> affectedBoardSlots;
        [SerializeField] protected float damageDelay;
        protected DefenceItemData _itemData;
        protected bool _isInCooldown;

        private DamageUI _damageUI;

        [Inject]
        private void Construct(DamageUI damageUI)
        {
            _damageUI = damageUI;
        }
        

        public void RemoveEnemyFromTargets(Enemy target)
        {
            if (enemiesInRange.Contains(target))
            {
                enemiesInRange.Remove(target);
            }
        }

        public void AddEnemyAsTarget(Enemy newTarget)
        {
            if (!enemiesInRange.Contains(newTarget) && newTarget.isActiveAndEnabled)
            {
                enemiesInRange.Add(newTarget);
            }

            if (!_isInCooldown)
                DoNextAttackCycle();
        }

        public void Initialize(DefenceItemData itemData, List<BoardSlot> boardSlots)
        {
            _itemData = itemData;
            affectedBoardSlots = boardSlots;
            
            AdjustAttackVFX();
            DoNextAttackCycle();
        }

        public void ClearEnemiesInRange()
        {
            enemiesInRange.Clear(); 
        }

        protected void DoNextAttackCycle()
        {
            if (enemiesInRange is { Count: > 0 }) AttackEnemies();
        }

        protected IEnumerator AttackCooldownRoutine()
        {
            _isInCooldown = true;
            yield return new WaitForSeconds(_itemData.interval);
            DoNextAttackCycle();
            _isInCooldown = false;
        }

        protected IEnumerator GiveDamage()
        {
            yield return new WaitForSeconds(damageDelay);
            var enemiesToDamage = new List<Enemy>(enemiesInRange);
         
            foreach (var enemy in enemiesToDamage)
            {
                enemy.TakeDamage(_itemData.damage);
                DamagePopUp.Create(_damageUI.damagePopUpPrefab, enemy.gameObject.transform.position,
                    _itemData.damage.ToString(), PopUpType.DefenceItemDamage);
            }
        }
        
        protected abstract void AttackEnemies();
        protected abstract void AdjustAttackVFX();
    }
}