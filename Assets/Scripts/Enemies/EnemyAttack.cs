using Board;
using Defence;
using UIScripts;
using UnityEngine;
using Zenject;

namespace Enemies
{
    [RequireComponent(typeof(Animator))]
    public sealed class EnemyAttack : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Attack = Animator.StringToHash("attack");
        private static readonly int AlternateAttack = Animator.StringToHash("alternateAttack");

        private DefenceItem _currentTarget;
        private float _currentDamage;
        
        private DamageUI _damageUI;

        [Inject]
        private void Construct(DamageUI damageUI)
        {
            _damageUI = damageUI;
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void StartAttacking(DefenceItem defender, float damage)
        {
            _currentTarget = defender;
            _currentDamage = damage;
            defender.OnRemovedFromSlot += OnDefeatedTarget;
            
            DoAttackAnimation();
        }

        private void DoAttackAnimation()
        {
            var rand = Random.Range(0, 2);

            switch (rand)
            {
                case 0:
                    _animator.SetTrigger(Attack);
                    break;
                case 1:
                    _animator.SetTrigger(AlternateAttack);
                    break;
            }
        }

        private void OnDefeatedTarget(ISlotOccupier _)
        {
            _currentTarget.OnRemovedFromSlot -= OnDefeatedTarget;
            _currentTarget = null;
            StopAttacking();
        }

        private void StopAttacking()
        {
            //TODO: Stop attack animation
        }

        public void GiveDamage()
        {
            _currentTarget.TakeDamage(_currentDamage);
            DamagePopUp.Create(_damageUI.damagePopUpPrefab, _currentTarget.gameObject.transform.position,
                _currentDamage.ToString(), PopUpType.EnemyDamage);
        }

        public void FinishWindDown()
        {
            if(_currentTarget != null && _currentTarget.isActiveAndEnabled)
                DoAttackAnimation();
        }
    }
}