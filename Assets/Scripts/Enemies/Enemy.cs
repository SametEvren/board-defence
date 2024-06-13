using System;
using System.Linq;
using Board;
using Defence;
using Player;
using UnityEngine;
using Zenject;

namespace Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyAttack))]
    public abstract class Enemy : MonoBehaviour, ISlotOccupier
    {
        [SerializeField] protected EnemyType enemyType;
        [SerializeField] protected EnemyData enemyData;
        
        public SlotOccupantType OccupantType => SlotOccupantType.Enemy;
        public event Action<ISlotOccupier> OnRemovedFromSlot;
        public event Action<Enemy> OnEnemyVanished;

        private Vector2Int _currentBoardCoordinates;
        
        private EnemyMovement _enemyMovement;
        private EnemyAttack _enemyAttack;
        private float _currentHealth;
        
        private BoardController _boardController;
        private EnemySpawnController _enemySpawnController;
        private PlayerController _playerController;

        [Inject]
        public void Construct(BoardController boardController, 
            EnemySpawnController enemySpawnController, 
            PlayerController playerController)
        {
            _boardController = boardController;
            _enemySpawnController = enemySpawnController;
            _playerController = playerController;
        }
        
        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyAttack = GetComponent<EnemyAttack>();
        }

        private void Start()
        {
            _enemyMovement.OnReachedPlayer += DisappearAndGiveDamage;
        }

        private void DisappearAndGiveDamage()
        {
            _playerController.TakeDamage(enemyData.damage);
            _enemyMovement.KillMovementSequence();
            OnRemovedFromSlot?.Invoke(this);
            OnEnemyVanished?.Invoke(this);
            _enemySpawnController.ReleaseEnemy(enemyType, gameObject);
        }

        public void InitializeEnemy(Vector2Int coordinates)
        {
            _currentHealth = enemyData.health;
            _currentBoardCoordinates = coordinates;
            var spawnSlot = _boardController.BoardSlots[coordinates.x, coordinates.y];
            spawnSlot.OccupySlot(this);
            transform.position = spawnSlot.transform.position;
            transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
            _enemyMovement.OnStoppedByEnemy += HandleEnemyEncounter;
            _enemyMovement.Initialize(_currentBoardCoordinates, enemyData.speed, (targetSlot) =>
            {
                OnRemovedFromSlot?.Invoke(this);
                targetSlot.OccupySlot(this);
                _currentBoardCoordinates = targetSlot.BoardCoordinates;
            });
        }

        private void HandleEnemyEncounter(BoardSlot encounterSlot)
        {
            DefenceItem defender = (DefenceItem) encounterSlot.CurrentOccupants.FirstOrDefault(o => o.OccupantType == SlotOccupantType.Defence);
            if(defender == null) return;

            _enemyAttack.StartAttacking(defender, enemyData.damage);
        }

        public void TakeDamage(float damage)
        {
            if(_currentHealth <= 0) return;
            
            _currentHealth -= damage;
            //TODO: Particles etc.
            
            if (_currentHealth <= 0)
            {
                OnDefeat();
            }
        }

        private void OnDefeat()
        {
            _enemyMovement.KillMovementSequence();
            OnRemovedFromSlot?.Invoke(this);
            OnEnemyVanished?.Invoke(this);
            _enemySpawnController.ReleaseEnemy(enemyType, gameObject);
        }
    }
}
