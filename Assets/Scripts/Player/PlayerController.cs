using System;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float _playerHealth = 100;
        public float _fullHealth;
        public event Action<float,float> OnDamageTaken;
        public event Action OnPlayerDestroyed;
        private void Awake()
        {
            _fullHealth = _playerHealth;
        }

        public void TakeDamage(float damage)
        {
            _playerHealth -= damage;
            
            if(_playerHealth <= 0)
                OnPlayerDestroyed?.Invoke();
            
            _playerHealth = Mathf.Clamp(_playerHealth, 0, _fullHealth);
            
            OnDamageTaken?.Invoke(_playerHealth, _fullHealth);
            
            
        }
    }
}
