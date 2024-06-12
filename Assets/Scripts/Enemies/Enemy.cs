using System;
using Board;
using UnityEngine;
using Zenject;

namespace Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    public abstract class Enemy : MonoBehaviour, ISlotOccupier
    {
        [SerializeField] protected EnemyData enemyData;
        public EnemyData EnemyData => enemyData;
        
        public SlotOccupantType OccupantType => SlotOccupantType.Enemy;
        public event Action<ISlotOccupier> OnRemovedFromSlot;

        private Vector2Int _currentBoardCoordinates;
        
        private EnemyMovement _enemyMovement;

        private BoardController _boardController;

        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
        }

        [Inject]
        public void Construct(BoardController boardController)
        {
            _boardController = boardController;
        }

        public void InitializeEnemy(Vector2Int coordinates)
        {
            _currentBoardCoordinates = coordinates;
            var spawnSlot = _boardController.BoardSlots[coordinates.x, coordinates.y];
            spawnSlot.OccupySlot(this);
            transform.position = spawnSlot.transform.position;
            transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
            _enemyMovement.Initialize(_currentBoardCoordinates, enemyData.speed, (targetSlot) =>
            {
                OnRemovedFromSlot?.Invoke(this);
                targetSlot.OccupySlot(this);
                _currentBoardCoordinates = targetSlot.BoardCoordinates;
            });
        }
    }
}