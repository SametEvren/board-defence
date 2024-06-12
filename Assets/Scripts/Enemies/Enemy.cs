using System;
using System.Linq;
using Board;
using Defence;
using DG.Tweening;
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

        private Vector2Int _currentBoardCoordinates;
        
        private EnemyMovement _enemyMovement;
        private EnemyAttack _enemyAttack;

        private BoardController _boardController;
        private EnemySpawnController _enemySpawnController;
        private float _currentHealth;

        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyAttack = GetComponent<EnemyAttack>();
        }

        [Inject]
        public void Construct(BoardController boardController, EnemySpawnController enemySpawnController)
        {
            _boardController = boardController;
            _enemySpawnController = enemySpawnController;
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
            _enemySpawnController.ReleaseEnemy(enemyType, gameObject);
            OnRemovedFromSlot?.Invoke(this);
        }
    }
}