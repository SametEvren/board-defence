using System;
using System.Linq;
using Board;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Sequence = DG.Tweening.Sequence;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, ISlotOccupier
    {
        [SerializeField] protected EnemyData enemyData;
        protected Vector2Int _currentBoardCoordinates;
        public SlotOccupantType OccupantType => SlotOccupantType.Enemy;
        public event Action<ISlotOccupier> OnRemovedFromSlot;

        protected BoardController BoardController;
        protected Sequence MovementSequence;

        [Inject]
        public void Construct(BoardController boardController)
        {
            BoardController = boardController;
        }

        protected void TriggerSlotRemoveAction()
        {
            OnRemovedFromSlot?.Invoke(this);
        }

        public void InitializeEnemy(Vector2Int coordinates)
        {
            MovementSequence?.Kill();
            MovementSequence = null;

            _currentBoardCoordinates = coordinates;
            var spawnSlot = BoardController.BoardSlots[coordinates.x, coordinates.y];
            spawnSlot.OccupySlot(this);
            transform.position = spawnSlot.transform.position;
            MakeNextMove();
        }
        
        protected void MakeNextMove()
        {
            var gridSize = new Vector2Int(
                BoardController.BoardSlots.GetLength(0),
                BoardController.BoardSlots.GetLength(1));
            var nextCoordinates = new Vector2Int(
                _currentBoardCoordinates.x,
                _currentBoardCoordinates.y - 1
            );

            if (nextCoordinates.y >= gridSize.y || nextCoordinates.y < 0) return;

            var nextSlot = BoardController.BoardSlots[nextCoordinates.x, nextCoordinates.y];
            
            MoveToSlot(nextSlot);
        }

        private void MoveToSlot(BoardSlot targetSlot)
        {
            var currentOccupants = targetSlot.CurrentOccupants;
            if (currentOccupants != null && currentOccupants.Any(o => o.OccupantType == SlotOccupantType.Defence))
                return;
            
            BeginMovement(targetSlot);
        }

        protected abstract void BeginMovement(BoardSlot targetSlot);
    }
}