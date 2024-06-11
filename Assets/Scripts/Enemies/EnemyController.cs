using UnityEngine;

namespace Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public bool isWalking;
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private static readonly int Alternate = Animator.StringToHash("alternateAttack");
        private static readonly int Damage = Animator.StringToHash("getDamage");
        private static readonly int Death1 = Animator.StringToHash("death");

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
                ToggleWalking();

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                Attack();
            
            if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                AttackAlternate();
            
            if(Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                GetDamage();
            
            if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                Death();            
            
        }

        public void ToggleWalking()
        {
            isWalking = !isWalking;
            animator.SetBool(IsWalking, isWalking);    
        }

        public void Attack()
        {
            animator.SetTrigger(Attack1);
        }

        public void AttackAlternate()
        {
            animator.SetTrigger(Alternate);
        }

        public void GetDamage()
        {
            animator.SetTrigger(Damage);
        }

        public void Death()
        {
            animator.SetTrigger(Death1);
        }
    }
}
